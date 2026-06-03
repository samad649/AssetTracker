import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { Profile as ProfileModel } from '../../models/profile';
import { Observable } from 'rxjs';
import { TransactionService } from '../../services/transactionService';
import { Transaction } from '../../models/transaction';

@Component({
  selector: 'app-transaction-card',
  imports: [CommonModule],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss'
})
export class TransactionCard implements OnInit {
  profile$!: Observable<ProfileModel | null>;
  transactions: Transaction[] = [];
  constructor(
       private sharedService: SharedService,
       private transactionService: TransactionService
  ) {}
  ngOnInit() {
    this.profile$ = this.sharedService.selectedProfile$;
    
    this.profile$.subscribe(profile => {
      if (!profile) return;
      
      this.transactionService.getTransactionsByProfile(profile.profileId).subscribe({
        next: (data) => this.transactions = data,
        error: (err) => console.error(err)
      });
    });
  }
}