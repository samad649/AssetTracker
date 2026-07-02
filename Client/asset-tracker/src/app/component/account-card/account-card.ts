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
import { NzModalModule } from 'ng-zorro-antd/modal';
@Component({
  selector: 'app-account-card',
  imports: [CommonModule, NzCardModule, NzIconModule, NzModalModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts$!: Observable<AccountModel[]>;
  selectedAccount: any = null;
  isModalVisible = false;
  constructor(
    private profileService: ProfileService,
    private authService: authService,
    private plaidService: PlaidService
  ) {}

  ngOnInit() {
      this.profileService.refreshAccounts();  
      this.accounts$ = this.profileService.accounts$;  
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
openModal(account: any) {
  console.log('Opening modal for account:', account);
    this.selectedAccount = account;
    this.isModalVisible = true;
}

closeModal() {
    this.isModalVisible = false;
    this.selectedAccount = null;
}

    // After deletion
    removeAccount() {
        if (this.selectedAccount) {
            this.plaidService.DeleteItem(this.selectedAccount.itemId).subscribe({
                next: () => {
                    this.closeModal();
                    this.profileService.refreshAccounts();  
                },
                error: (err) => console.error('Error:', err.error)
            });
        }
    }
}