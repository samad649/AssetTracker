import { Component } from '@angular/core';
import { PlaidService } from '../../services/plaidService';
import { authService } from '../../services/authService';
@Component({
  selector: 'app-plaid',
  imports: [],
  templateUrl: './plaid.html',
  styleUrl: './plaid.scss'
})
export class Plaid {

  constructor(private plaidService: PlaidService, private authService: authService) {}

  connectBank() {
   const userId = this.authService.getUserId();
   this.plaidService.createLinkToken(userId).subscribe({
    next: (res: any) => {
      const linkToken = res.link_token;

      this.plaidService.openPlaidLink(linkToken);
    },
    error: (err) => console.error(err)
  });
}
}
