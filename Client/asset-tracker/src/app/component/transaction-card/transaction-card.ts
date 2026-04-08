import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profileService';
import { SharedService } from '../../services/sharedService';
import { Transaction as TransactionModel } from '../../models/transaction';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-transaction-card',
  imports: [CommonModule],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss'
})
export class TransactionCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts: AccountModel[] = [];
  transactions: TransactionModel[][] = []; // 2D array
  loading = true;

  constructor(
    private profileService: ProfileService,
    private sharedService: SharedService
  ) {}

  ngOnInit() {
    // Index of Account and Transaction Array are linked,
    this.profile = this.sharedService.getSelectedProfile();

    this.profileService.getAccounts(this.profile?.profileId ?? '').pipe(
      switchMap(accounts => {
        this.accounts = accounts;
        const requests = accounts.map(account =>
          this.profileService.getTransactions(account.accountId ?? '')
        );
        return forkJoin(requests);
      })
    ).subscribe({
      next: (allTransactions) => {
        this.transactions = allTransactions; // keep as 2D array
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }
}