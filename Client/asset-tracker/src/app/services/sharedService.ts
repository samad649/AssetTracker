import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Profile } from '../models/profile';
import { Account } from '../models/account';
import { ProfileService } from './profileService';
import { authService } from './authService';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

 constructor(private profileService: ProfileService, private authService: authService) {
  this.authService.isLoggedIn$.subscribe(isLoggedIn => {
    if (isLoggedIn) this.loadUserData();
    else this.clearState();
  });
}

  // ---------------PROFILES------------------ //
  private selectedProfileSubject = new BehaviorSubject<Profile | null>(null);
  selectedProfile$ = this.selectedProfileSubject.asObservable();

  // ---------------ACCOUNTS------------------ //
  private selectedAccountSubject = new BehaviorSubject<Account | null>(null);
  selectedAccount$ = this.selectedAccountSubject.asObservable();

loadUserData() {
  const userId = this.authService.getUserId();
  if (!userId) return;

  this.profileService.getProfile(userId).subscribe({
    next: (profile) => {
      this.selectedProfileSubject.next(profile);
      console.log('Profile loaded:', profile);
    },
    error: (err) => console.error('Failed to fetch profile', err)
  });
}

  getSelectedProfile(): Profile | null {
    return this.selectedProfileSubject.getValue();
  }

  getSelectedAccount(): Account | null {
    return this.selectedAccountSubject.getValue();
  }

  clearState() {
    this.selectedProfileSubject.next(null);
    this.selectedAccountSubject.next(null);
  }
}