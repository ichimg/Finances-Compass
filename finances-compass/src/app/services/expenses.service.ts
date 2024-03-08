import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Expense } from '../entities/expense.model';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  createDebt(createExpenseRequest: Expense) {
    const email = localStorage.getItem('email');
    const createExpenseEndpoint = `${this.apiUrl}/create-expense?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.post<any>(createExpenseEndpoint, createExpenseRequest, {
      headers: headers,
    });
  }
}
