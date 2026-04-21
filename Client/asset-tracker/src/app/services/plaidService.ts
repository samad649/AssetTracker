import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authService } from './authService';
import { tap } from 'rxjs';
declare var Plaid: any; 

@Injectable({
  providedIn: 'root'
})
export class PlaidService {

  private apiUrl = 'https://localhost:7141/api/plaid';

  constructor(private http: HttpClient, private authService: authService) {}

createLinkToken(userId: string) {
  return this.http.post<any>(
    `${this.apiUrl}/createLinkToken`,
    { userId: userId }  
  );
}
ExchangePublicToken(userId: string, publicToken: string) {
  console.log('Sending request:', { userId, publicToken });

  return this.http.post<any>(
    `${this.apiUrl}/exchangePublicToken`,
    { userId: userId, publicToken: publicToken }
  ).pipe(
    tap(response => {
      console.log('Response from backend:', response);
    })
  );
}
  openPlaidLink(linkToken: string) {
    const handler = Plaid.create({
      token: linkToken,

      onSuccess: (public_token: string, metadata: any) => {
        console.log('Success:', public_token);
        console.log(metadata);
        this.ExchangePublicToken(this.authService.getUserId(), public_token);

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