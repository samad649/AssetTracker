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
   const userStr = localStorage.getItem('user') || '{}'; 
  const user = JSON.parse(userStr);
  const userId = user.userId || '';
  this.plaidService.createLinkToken(userId).subscribe({
    next: (res: any) => {
      const linkToken = res.link_token;

      this.plaidService.openPlaidLink(linkToken);
    },
    error: (err) => console.error(err)
  });
}
}
