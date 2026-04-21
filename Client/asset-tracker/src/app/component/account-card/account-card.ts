import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedService } from '../../services/sharedService';
import { ProfileService } from '../../services/profileService';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { Observable } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

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
  this.accounts$ = this.sharedService.selectedProfile$.pipe(
    filter(profile => !!profile?.profileId),
    switchMap(profile => 
      this.profileService.getAccounts(profile!.profileId)
    )
  );
}
}