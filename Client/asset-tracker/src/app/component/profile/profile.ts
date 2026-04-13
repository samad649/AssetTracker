import { Component, inject} from '@angular/core';
import {Validators, ReactiveFormsModule} from '@angular/forms';
import { NonNullableFormBuilder } from '@angular/forms';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { ProfileService } from '../../services/profileService';
import { SharedService } from '../../services/sharedService';
import { Profile as ProfileModel } from '../../models/profile';
import { CommonModule, AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { authService } from '../../services/authService';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NzDropDownModule, NzMenuModule, CommonModule, AsyncPipe, NzFormModule, NzButtonModule, NzCheckboxModule, NzInputModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {

  private fb = inject(NonNullableFormBuilder);

  validateForm = this.fb.group({
    username: this.fb.control('', [Validators.required]),
    password: this.fb.control('', [Validators.required])
  });
  profiles$: Observable<ProfileModel[]>;

  constructor(private profileService: ProfileService, private sharedService: SharedService, private authService: authService, private router: Router) {
    this.profiles$ = this.profileService.getAllProfiles();
  }

    submitForm() {
    if (this.validateForm.invalid) {
      // mark all fields as touched so errors show
      Object.values(this.validateForm.controls).forEach(control => {
        control.markAsTouched();
      });
      return;
    }

    const username = this.validateForm.get('username')!.value;
    const password = this.validateForm.get('password')!.value;

    console.log(username, password);
this.authService.login(username, password).subscribe({
  next: () => {
    console.log('logged in');
    this.router.navigate(['/accounts']); // redirect after login
  },
  error: (err) => {
    console.error('login failed', err);
  }
});  }
  onSelectProfile(profile: ProfileModel) {
    console.log(profile);
    this.sharedService.setSelectedProfile(profile);
  }
}