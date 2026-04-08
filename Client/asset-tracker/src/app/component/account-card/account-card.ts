import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { Profile as ProfileModel } from '../../models/profile';
import { ProfileService } from '../../services/profileService';
import { Account as AccountModel } from '../../models/account';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-account-card',
  imports: [CommonModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard {
profile: ProfileModel | null = null;
accounts$ : Observable<AccountModel[]>;
constructor(private sharedService: SharedService, private profileService: ProfileService) {
  this.profile = this.sharedService.getSelectedProfile();
  this.accounts$ = this.profileService.getAccounts(this.profile?.profileId ?? '');
}

}
