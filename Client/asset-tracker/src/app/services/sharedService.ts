import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Profile } from '../models/profile';
import { Account } from '../models/account';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  // Current Profile variable as a BehaviorSubject
  // Holds a value and emits any changes to subscribers
  private selectedProfileSubject = new BehaviorSubject<Profile | null>(null);
  // Makes an observable which is read only vs Read&Write as a BehaviorSubject
  selectedProfile$ = this.selectedProfileSubject.asObservable();

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