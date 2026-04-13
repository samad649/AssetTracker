import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Profile } from '../models/profile';
import { Account } from '../models/account';
import { User } from '../models/user';
import { ProfileService } from './profileService';
@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(private profileService: ProfileService) {}
  // Current Profile variable as a BehaviorSubject
  // Holds a value and emits any changes to subscribers
  private selectedProfileSubject = new BehaviorSubject<Profile | null>(null);
  private selectedUserSubject = new BehaviorSubject<User | null>(null);

  // Makes an observable which is read only vs Read&Write as a BehaviorSubject
  selectedProfile$ = this.selectedProfileSubject.asObservable();
  selectedUser$ = this.selectedUserSubject.asObservable();

  setSelectedUser(user: User) {
      this.selectedUserSubject.next(user);
      console.log("Selected user set:", user);
      if (!user.profileId) return;

      this.profileService.getProfile(user.profileId).subscribe({
        next: (profile) => {
          this.selectedProfileSubject.next(profile);
          console.log("Selected profile set:", profile);
        },
        error: (err) => console.error(err)
      });
    }
  
  getSelectedUser() {
    return this.selectedUserSubject.getValue();
  }
  setSelectedProfile(profile: Profile) {
    this.selectedProfileSubject.next(profile);
  }

  getSelectedProfile(): Profile | null {
    return this.selectedProfileSubject.getValue();
  }

  // Selected Account
  private selectedAccountSubject = new BehaviorSubject<Account | null>(null);
  selectedAccount$ = this.selectedAccountSubject.asObservable();

  setSelectedAccount(account: Account) {
    this.selectedAccountSubject.next(account);
  }

  getSelectedAccount(): Account | null {
    return this.selectedAccountSubject.getValue();
  }

  // Clear all state
  clearState() {
    this.selectedProfileSubject.next(null);
    this.selectedAccountSubject.next(null);
  }
}