import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PlaidItemService {
    private apiUrl = 'https://localhost:7141/api/PlaidItem';

    constructor(private http: HttpClient) {}

    addPlaidItem(publicToken: string, metadata: any) {
        return this.http.post<any>(
        `${this.apiUrl}/addPlaidItem`,
        { 
        publicToken: publicToken, 
        institutionId: metadata.institution.institution_id,
        institution: metadata.institution.name 
        }
        );
    }
  }