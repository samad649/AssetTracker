import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { ProfileService } from '../../services/profileService';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { User as UserModel } from '../../models/user';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-account-card',
  imports: [CommonModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard implements OnInit {
  profile: ProfileModel | null = null;
  user: UserModel | null = null;
  accounts$!: Observable<AccountModel[]>;

  constructor(private sharedService: SharedService, private profileService: ProfileService) {
    this.user = this.sharedService.getSelectedUser();
    console.log(this.user);
  }

  ngOnInit():void {
    this.profile = this.sharedService.getSelectedProfile();
    console.log(this.user?.profileId)
    this.accounts$ = this.profileService.getAccounts(this.user?.profileId ?? '');
  }
}
