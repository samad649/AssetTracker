import { Component, inject} from '@angular/core';
import {Validators, ReactiveFormsModule} from '@angular/forms';
import { NonNullableFormBuilder } from '@angular/forms';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { SharedService } from '../../services/sharedService';
import { CommonModule } from '@angular/common';
import { authService } from '../../services/authService';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { Router } from '@angular/router';
import { Profile as ProfileModel } from '../../models/profile';
import { Observable } from 'rxjs';
import { OnInit } from '@angular/core';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [NzDropDownModule, NzMenuModule, CommonModule, NzFormModule, NzButtonModule, NzCheckboxModule, NzInputModule, ReactiveFormsModule, NzDescriptionsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login implements OnInit {
  profile$!: Observable<ProfileModel | null>;
  user$ !: Observable<any>;
  UserId!: string;
  isLoggedIn = false;
  private fb = inject(NonNullableFormBuilder);

  validateForm = this.fb.group({
    username: this.fb.control('', [Validators.required]),
    password: this.fb.control('', [Validators.required])
  });

  constructor(private sharedService: SharedService, private authService: authService, private router: Router) {
    this.authService.isLoggedIn$.subscribe(loggedIn => {
        this.isLoggedIn = loggedIn;
    });  
  }
  ngOnInit(): void {
      this.user$ = this.authService.selectedUser$;
      this.UserId = this.authService.getUserId();
      this.profile$ = this.sharedService.selectedProfile$;
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
      },
      error: (err) => {
        console.error('login failed', err);
      }
    }); 
   }
   logout(){
    this.authService.logout();
   }

}