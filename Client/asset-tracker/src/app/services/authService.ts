import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { tap } from "rxjs";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class authService {
  private apiUrl = 'https://localhost:7141/api/auth';
  private currentUserId = new BehaviorSubject<string | null>(null);
  private userApiUrl = 'https://localhost:7141/api/user';

  constructor(private http: HttpClient) {
    // Restore session on page refresh using token
    const token = localStorage.getItem('token');
    if (token) {
      const userId = this.decodeToken(token);
      this.currentUserId.next(userId);
    }
  }

  login(username: string, password: string) {
    return this.http.post<{ token: string; user: any }>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          const userId = this.decodeToken(response.token);
          this.currentUserId.next(userId);
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserId.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): string {
    return this.currentUserId.getValue() || '';
  }

  getUser() {
    const userId = this.getUserId();
    return this.http.get<any>(`${this.userApiUrl}/${userId}`);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private decodeToken(token: string): string | null {
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload));
      return decoded.userId || null;
    } catch {
      return null;
    }
  }
}