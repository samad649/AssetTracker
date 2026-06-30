import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { UserService } from '../../services/userService';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})


export class Register {
private fb = inject(NonNullableFormBuilder);

    validateForm = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName:  ['', [Validators.required]],
    email:     ['', [Validators.required, Validators.email]],
    username:  ['', [Validators.required]],
    password:  ['', [Validators.required, Validators.minLength(8)]],
  });

  constructor(private userService: UserService, private router: Router) {}
  
      submitForm(): void {
      
        if (this.validateForm.invalid) {
          Object.values(this.validateForm.controls).forEach(control => {
            control.markAsTouched();
          });
          return;
        }
      const userPayload = this.validateForm.value;   

      this.userService.registerUser(userPayload).subscribe({
        next: (res) => {
          console.log('User created:', res);
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('Registration failed:', err);
        }
      });


    }

}
