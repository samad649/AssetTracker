import { Component } from '@angular/core';
import { CommonModule, JsonPipe } from '@angular/common';
import { PlaidService } from '../../services/plaid.service';

@Component({
  selector: 'app-plaid-link',
  templateUrl: './plaid-link.component.html',
  standalone: true,
  imports: [CommonModule, JsonPipe]
})
export class PlaidLinkComponent {
  response: any;

  constructor(private plaidService: PlaidService) {}

  initializePlaidLink() {
    this.plaidService.createLinkToken().subscribe(res => {
      this.response = res;
      const handler = (window as any).Plaid.create({
        token: this.response.link_token,
        onSuccess: (public_token: string, metadata: any) => {
          // Handle success
        },
        onExit: (err: any, metadata: any) => {
          // Handle exit
        }
      });
      handler.open();
    });
  }
}