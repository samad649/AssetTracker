import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PlaidItemService } from './plaidItemService';
import { ProfileService } from './profileService';
declare var Plaid: any; 

@Injectable({
  providedIn: 'root'
})
export class PlaidService {

  private apiUrl = 'https://localhost:7141/api/Plaid';

  constructor(private http: HttpClient, private plaidItemService: PlaidItemService, private profileService: ProfileService) {}

createLinkToken(userId: string) {
  return this.http.post<any>(
    `${this.apiUrl}/createLinkToken`,
    { userId: userId }  
  );
}

DeleteItem(itemId: string) {
  return this.http.delete(`${this.apiUrl}/deleteItem/${itemId}`);
}

openPlaidLink(linkToken: string) {
  const handler = Plaid.create({
    token: linkToken,

      onSuccess: (public_token: string, metadata: any) => {
        console.log('Success:', public_token);
        console.log(metadata);
        this.plaidItemService.addPlaidItem(public_token, metadata).subscribe({
          next: (response) => {
            console.log('Item added successfully:', response);
            this.profileService.refreshAccounts();  // ✅ add this

          },
          error: (error) => {
            console.error('Error adding item:', error);
          }
        });

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