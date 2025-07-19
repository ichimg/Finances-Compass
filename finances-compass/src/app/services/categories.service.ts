import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}
  
  getAllExpense() {
    const getAllEndpoint = `${this.apiUrl}/get-expense-categories`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }

  getAllIncome() {
    const getAllEndpoint = `${this.apiUrl}/get-income-categories`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }
}
