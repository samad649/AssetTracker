import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { ProfileService } from '../../services/profileService';
import { Profile } from '../../models/profile';
@Component({
  standalone: true,
  selector: 'app-login',
  imports: [MatButtonModule,CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login implements OnInit{

  profileData !: Profile;
  showData:boolean = false;

  constructor(private profileService: ProfileService) {}
  
  onClick(){
    this.showData = !this.showData;
  }
  ngOnInit(): void {
    this.profileService.getProfile().subscribe(
      (data: Profile) => {
        this.profileData = data;
        console.log(this.profileData);
      },
      (error) => {
        console.error('Error fetching profile data:', error);
      }
    );
  }
}
