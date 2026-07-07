import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { Profile as ProfileModel } from '../../models/profile';
import { Observable } from 'rxjs';
import { TransactionService } from '../../services/transactionService';
import { Transaction } from '../../models/transaction';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { PlaidItemService } from '../../services/plaidItemService';
import { transactionFilterService } from '../../services/transactionFilterService';
@Component({
  selector: 'app-transaction-card',
  imports: [CommonModule, NzTableModule, NzDropDownModule, NzIconModule, NzButtonComponent],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss'
})
export class TransactionCard implements OnInit {
    profile$!: Observable<ProfileModel | null>;
    transactions: Transaction[] = [];
    profileId: string = '';
    items: any[] = []; 
      constructor(
              private sharedService: SharedService,
              private transactionService: TransactionService,
              private plaidItemService: PlaidItemService,
              private transactionFilterService: transactionFilterService  
          ) {}
    ngOnInit() {
        this.profile$ = this.sharedService.selectedProfile$;

        this.profile$.subscribe(profile => {
            if (!profile) return;
            this.profileId = profile.profileId;
            this.getTransactionsByProfile(this.profileId);
            this.loadItems(this.profileId);  

        });
    }

      loadItems(profileId: string) {
          this.plaidItemService.getItemsByProfile(profileId).subscribe({
              next: (items) => {
                  this.items = items;
                  console.log('Items:', this.items);  // ✅ log after data arrives
              },
              error: (err) => console.error(err)
          });
      }

    syncTransactions(itemId: string) {
        this.plaidItemService.sync(itemId).subscribe({
            next: () => {
                console.log('Synced:', itemId);
                this.getTransactionsByProfile(this.profileId);
            },
            error: (err) => console.error(err)
        });
    }
    resetSync(itemId: string) {
        this.plaidItemService.resetSync(itemId).subscribe({
            next: () => {
                console.log('Reset Synced:', itemId);
                this.getTransactionsByProfile(this.profileId);
            },
            error: (err) => console.error(err)
        });
    }
    getTransactionsByProfile(profileId: string) {
        this.transactionService.getTransactionsByProfile(profileId).subscribe({
            next: (data) => {
            this.transactions = this.transactionFilterService.sortListByDate(data, 'asc');  
            console.log('Transactions:', this.transactions); 
            },
            error: (err) => console.error(err)
        });

        
    }
}