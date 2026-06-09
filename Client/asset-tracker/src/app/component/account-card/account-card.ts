import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profileService';
import { Profile as ProfileModel } from '../../models/profile';
import { Account as AccountModel } from '../../models/account';
import { Observable } from 'rxjs';
import { NzCardModule } from 'ng-zorro-antd/card';
@Component({
  selector: 'app-account-card',
  imports: [CommonModule, NzCardModule],
  templateUrl: './account-card.html',
  styleUrl: './account-card.scss'
})
export class AccountCard implements OnInit {
  profile: ProfileModel | null = null;
  accounts$!: Observable<AccountModel[]>;

  constructor(
    private profileService: ProfileService
  ) {}

  ngOnInit(): void {
    this.accounts$ = this.profileService.getAccounts();
}
}