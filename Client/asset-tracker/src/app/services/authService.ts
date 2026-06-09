import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { tap } from "rxjs";
import { BehaviorSubject } from "rxjs";
import { map } from "rxjs";
import { User } from "../models/user"; // add your user model

@Injectable({
  providedIn: 'root'
})
export class authService {
  private apiUrl = 'https://localhost:7141/api/auth';
  private currentUserId = new BehaviorSubject<string | null>(null);
  private currentUser = new BehaviorSubject<User | null>(null);
  selectedUser$ = this.currentUser.asObservable();
  isLoggedIn$ = this.currentUserId.asObservable().pipe(
    map(() => this.isLoggedIn()) 
  );

  private userApiUrl = 'https://localhost:7141/api/user';

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    
    if (token) {
      if (this.isTokenExpired(token)) {
        localStorage.removeItem('token');
      } else {
        const userId = this.decodeToken(token);
        this.currentUserId.next(userId);
        // restore user on page refresh
        this.loadUser();
      }
    }
  }

  login(username: string, password: string) {
    return this.http.post<{ token: string; user: any }>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          const userId = this.decodeToken(response.token);
          this.currentUserId.next(userId);
          this.loadUser(); // fetch and store user on login
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserId.next(null);
    this.currentUser.next(null); // clear user on logout
  }

  private loadUser() {
    this.getUser().subscribe({
      next: (user) => this.currentUser.next(user),
      error: (err) => console.error('Failed to fetch user', err)
    });
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    if (token && this.isTokenExpired(token)) {
      localStorage.removeItem('token');
      this.currentUserId.next(null);
      return null;
    }
    return token;
  }

  getUserId(): string {
    return this.currentUserId.getValue() || '';
  }

  getUser() {
    const userId = this.getUserId();
    return this.http.get<User>(`${this.userApiUrl}/${userId}`);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload));
      const exp = decoded.exp; 
      const now = Math.floor(Date.now() / 1000); 
      return exp < now; 
    } catch {
      return true; 
    }
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