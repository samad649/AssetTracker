import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profileService';
import { SharedService } from '../../services/sharedService';
import { Transaction as TransactionModel } from '../../models/transaction';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { forkJoin, of} from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-transaction-card',
  imports: [CommonModule],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss'
})
export class TransactionCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts: AccountModel[] = [];
  transactions: TransactionModel[][] = [];
  loading = true;

  constructor(
    private profileService: ProfileService,
    private sharedService: SharedService
  ) {}
ngOnInit() {
  this.sharedService.selectedProfile$.pipe(

    filter(profile => !!profile?.profileId),

    switchMap(profile => 
      this.profileService.getAccounts(profile!.profileId)
    ),

    switchMap(accounts => {
      this.accounts = accounts;

      if (accounts.length === 0) return of([]); 

      const requests = accounts.map(account =>
        this.profileService.getTransactions(account.accountId ?? '')
      );

      return forkJoin(requests);
    })

  ).subscribe({
    next: (allTransactions) => {
      this.transactions = allTransactions;
      this.loading = false;
    },
    error: (err) => {
      console.error(err);
      this.loading = false;
    }
  });
}
}