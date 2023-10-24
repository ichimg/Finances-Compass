import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ViewDebtsResponse } from '../interfaces/view-debts-response';

@Injectable()
export class DebtsService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getAllReceivingDebts() {
    const getAllEndpoint = `${this.apiUrl}/view-receiving-debts`;

    const email = localStorage.getItem('email') || ''; 
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<ViewDebtsResponse>(getAllEndpoint, { headers: headers });
  }

  getAllUserDebts() {
    const getAllEndpoint = `${this.apiUrl}/view-user-debts`;

    const email = localStorage.getItem('email') || ''; 
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<ViewDebtsResponse>(getAllEndpoint, { headers: headers });
  }
}
