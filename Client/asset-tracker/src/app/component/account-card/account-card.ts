import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { ProfileService } from '../../services/profileService';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-account-card',
  imports: [CommonModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts$!: Observable<AccountModel[]>;

  constructor(
    private sharedService: SharedService,
    private profileService: ProfileService
  ) {}

  ngOnInit(): void {
    // Wait for profile — everything flows from here
    this.sharedService.selectedProfile$.subscribe(profile => {
      if (profile?.profileId) {
        this.profile = profile;
        this.accounts$ = this.profileService.getAccounts(profile.profileId);
      }
    });
  }
}