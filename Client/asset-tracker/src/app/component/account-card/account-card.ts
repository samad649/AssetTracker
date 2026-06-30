import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profileService';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { Observable } from 'rxjs';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { authService } from '../../services/authService';
import { PlaidService } from '../../services/plaidService';
@Component({
  selector: 'app-account-card',
  imports: [CommonModule, NzCardModule, NzIconModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts$!: Observable<AccountModel[]>;

  constructor(
    private profileService: ProfileService,
    private authService: authService,
    private plaidService: PlaidService
  ) {}

  ngOnInit(): void {
    this.accounts$ = this.profileService.getAccounts();
}

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