import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

declare var Plaid: any; // ✅ fix for global Plaid

@Injectable({
  providedIn: 'root'
})
export class PlaidService {

  private apiUrl = 'https://localhost:7141/api/plaid';

  constructor(private http: HttpClient) {}

  createLinkToken(userId: string) {
    return this.http.post<any>(`${this.apiUrl}/createLinkToken`, { userId });
  }

  openPlaidLink(linkToken: string) {
    const handler = Plaid.create({
      token: linkToken,

      onSuccess: (public_token: string, metadata: any) => {
        console.log('Success:', public_token);
        //BACKEND SEND Data
      },

      onLoad: () => {
        console.log('Plaid loaded');
      },

      onExit: (err: any, metadata: any) => {
        console.log('Exit:', err, metadata);
      },

      onEvent: (eventName: string, metadata: any) => {
        console.log('Event:', eventName);
      }
    });

    handler.open(); 
  }
}