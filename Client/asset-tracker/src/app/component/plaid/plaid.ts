import { Component } from '@angular/core';
import { PlaidService } from '../../services/plaidService';
@Component({
  selector: 'app-plaid',
  imports: [],
  templateUrl: './plaid.html',
  styleUrl: './plaid.scss'
})
export class Plaid {

  constructor(private plaidService: PlaidService) {}

  connectBank() {
  const userId = '123'; // replace with your actual logged-in user

  this.plaidService.createLinkToken(userId).subscribe({
    next: (res: any) => {
      const linkToken = res.link_token;

      // 🔥 open Plaid widget
      this.plaidService.openPlaidLink(linkToken);
    },
    error: (err) => console.error(err)
  });
}
}
