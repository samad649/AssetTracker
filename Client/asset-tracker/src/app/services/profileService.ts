import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Profile } from '../models/profile';
import { Account } from '../models/account';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private apiUrl = 'https://localhost:7141/api/Profile';
  
  constructor(private http: HttpClient) { }

  getAllProfiles(): Observable<Profile[]> {
    return this.http.get<Profile[]>(`${this.apiUrl}/all`).pipe(
      catchError(this.handleError('getAllProfiles'))
    );
  }

  getProfile(id: string): Observable<Profile> {
    return this.http.get<Profile>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError('getProfile'))
    );
  }

  getAccounts(id: string): Observable<Account[]> {
    return this.http.get<Account[]>(`${this.apiUrl}/${id}/accounts`).pipe(
      catchError(this.handleError('getAccounts'))
    );
  }

  getTransactions(id: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/accounts/${id}/transactions`).pipe(
      catchError(this.handleError('getTransactions'))
    );
  }

  private handleError(operation: string) {
    return (error: any): Observable<never> => {
      switch (error.status) {
        case 400:
          console.error(`${operation} - Bad request:`, error.message);
          break;
        case 401:
          console.error(`${operation} - Unauthorized`);
          break;
        case 404:
          console.error(`${operation} - Not found`);
          break;
        case 500:
          console.error(`${operation} - Server error`);
          break;
        default:
          console.error(`${operation} - Unknown error:`, error.message);
      }
      return throwError(() => error);
    };
  }
}