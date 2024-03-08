import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}
  
  getAll() {
    const getAllEndpoint = `${this.apiUrl}/get-categories`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }
}
