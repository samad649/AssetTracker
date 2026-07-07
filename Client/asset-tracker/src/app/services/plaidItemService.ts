import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PlaidItemService {
    private apiUrl = 'https://localhost:7141/api/PlaidItem';

    constructor(private http: HttpClient) {}

      addPlaidItem(publicToken: string, metadata: any) {
          return this.http.post(
              `${this.apiUrl}/addPlaidItem`,
              { 
                  publicToken: publicToken, 
                  institutionId: metadata.institution.institution_id,
                  institution: metadata.institution.name 
              },
              { responseType: 'text' }  
          );
      }
            sync(itemId: string) {
            return this.http.post(
                `${this.apiUrl}/sync/${itemId}`, 
                {},
                { responseType: 'text' }
            );
        }
      resetSync(itemId: string) {
            return this.http.post(
                `${this.apiUrl}/reset-sync/${itemId}`, 
                {},
                { responseType: 'text' }
            );
        }
        getItemsByProfile(profileId: string) {
            return this.http.get<any[]>(`${this.apiUrl}/${profileId}`);
        }
}
  