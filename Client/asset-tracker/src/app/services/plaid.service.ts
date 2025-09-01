import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlaidService {
  private apiUrl = 'http://localhost:5235/PlaidLinkToken/create';

  constructor(private http: HttpClient) { }

  createLinkToken(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
