import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileService } from '../../services/profileService';
import { authService } from '../../services/authService';
import { Profile as ProfileModel } from '../../models/profile';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-transaction-card',
  imports: [CommonModule],
  templateUrl: './transaction-card.html',
  styleUrl: './transaction-card.scss'
})
export class TransactionCard implements OnInit {
  profile$!: Observable<ProfileModel>;
  UserId!: string;
  constructor(
        private profileService: ProfileService,
        private authService: authService
  ) {}
ngOnInit() {
this.UserId = this.authService.getUserId();
this.profile$ = this.profileService.getProfile(this.UserId);

}
}