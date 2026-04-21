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
    if (this.authService.isLoggedIn()) {
      this.loadUserData();
    }
  }

  // ---------------PROFILES------------------ //
  private selectedProfileSubject = new BehaviorSubject<Profile | null>(null);
  selectedProfile$ = this.selectedProfileSubject.asObservable();

  // ---------------ACCOUNTS------------------ //
  private selectedAccountSubject = new BehaviorSubject<Account | null>(null);
  selectedAccount$ = this.selectedAccountSubject.asObservable();

  // Single entry point — everything loads from userId
  loadUserData() {
    const userId = this.authService.getUserId();
    if (!userId) return;

    this.authService.getUser().subscribe({
      next: (user) => {
        if (user?.profileId) {
          this.profileService.getProfile(user.profileId).subscribe({
            next: (profile) => {
              this.selectedProfileSubject.next(profile);
              console.log('Profile loaded:', profile);
            },
            error: (err) => console.error('Failed to fetch profile', err)
          });
        }
      },
      error: (err) => console.error('Failed to fetch user', err)
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