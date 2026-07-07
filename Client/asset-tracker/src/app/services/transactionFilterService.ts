
import { Transaction } from "../models/transaction";
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'  // ✅ add this
})
export class transactionFilterService {

    sortListByDate(transactions: Transaction[], order: 'asc' | 'desc' = 'desc'): Transaction[] {
        return [...transactions].sort((a, b) => {
            const dateA = a.date ? new Date(a.date).getTime() : 0;
            const dateB = b.date ? new Date(b.date).getTime() : 0;
            return order === 'desc' ? dateB - dateA : dateA - dateB;
        });
    }
    filterByAccount(transactions: Transaction[], accountId: string): Transaction[] {
        return transactions.filter(transaction => transaction.accountId === accountId);
    }
}
