import { Component } from '@angular/core';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { ProfileService } from '../../services/profileService';
import { SharedService } from '../../services/sharedService';
import { Profile as ProfileModel } from '../../models/profile';
import { CommonModule, AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { authService } from '../../services/authService';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NzDropDownModule, NzMenuModule, CommonModule, AsyncPipe],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {
  profiles$: Observable<ProfileModel[]>;

  constructor(private profileService: ProfileService, private sharedService: SharedService, private authService: authService) {
    this.profiles$ = this.profileService.getAllProfiles();
  }

  onSelectProfile(profile: ProfileModel) {
    console.log(profile);
    this.sharedService.setSelectedProfile(profile);
  }
}