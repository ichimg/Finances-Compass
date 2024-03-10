import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Income } from '../entities/income.model';

@Injectable({
  providedIn: 'root'
})
export class IncomesService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  createIncome(createIncomeRequest: Income) {
    const email = localStorage.getItem('email');
    const createIncomeEndpoint = `${this.apiUrl}/create-income?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.post<any>(createIncomeEndpoint, createIncomeRequest, {
      headers: headers,
    });
  }

  deleteIncome(id: string) {
    const email = localStorage.getItem('email');
    const deleteIncomeEndpoint = `${this.apiUrl}/delete-income?id=${id}&email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.delete<any>(deleteIncomeEndpoint, {
      headers: headers,
    });
  }

  updateIncome(editIncomeRequest: Income) {
    const email = localStorage.getItem('email');
    const editIncomeEndpoint = `${this.apiUrl}/edit-income?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(editIncomeEndpoint, editIncomeRequest, {
      headers: headers,
    });
  }
}
