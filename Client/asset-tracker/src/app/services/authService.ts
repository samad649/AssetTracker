import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { tap } from "rxjs";
@Injectable({
  providedIn: 'root'
})
export class authService{
  private apiUrl = 'https://localhost:7141/api/auth';


  constructor(private http: HttpClient) { }

  
  login(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap(response => localStorage.setItem('token', response.token))
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