import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { tap } from "rxjs";
import { SharedService } from "./sharedService";
@Injectable({
  providedIn: 'root'
})
export class authService{
  private apiUrl = 'https://localhost:7141/api/auth';


  constructor(private http: HttpClient, private sharedService: SharedService) { }

  
  login(username: string, password: string) {
    return this.http.post<{ token: string; user: any }>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.sharedService.setSelectedUser(response.user);
          console.log('Login successful, token:', response.token);
          console.log('User:', response.user);
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn() {
    //LATER ADD EXPIRATION CHECKING AND MAKE SURE TOKEN IS VALID NOT JUST EXISTS
    return !!this.getToken();
  }

}