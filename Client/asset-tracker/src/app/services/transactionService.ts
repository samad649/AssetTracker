import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = 'https://localhost:7141/api/Transaction';

  constructor(private http: HttpClient) {}

  // POST /api/Transaction/sync/{profileId}
  syncTransactions(profileId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/sync/${profileId}`, {});
  }

  // POST /api/Transaction/sync/{profileId}/{itemId}
  syncTransactionsForItem(profileId: string, itemId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/sync/${profileId}/${itemId}`, {});
  }

  // GET /api/Transaction/get/{profileId}/{accountId}
  getTransactionsByAccount(profileId: string, accountId: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/get/${profileId}/${accountId}`);
  }

  // GET /api/Transaction/get/{profileId}
  getTransactionsByProfile(profileId: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/get/${profileId}`);
  }
}