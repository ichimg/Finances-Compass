import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Expense } from '../entities/expense.model';

@Injectable({
  providedIn: 'root',
})
export class ExpensesService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  createExpense(createExpenseRequest: Expense) {
    const email = localStorage.getItem('email');
    const createExpenseEndpoint = `${this.apiUrl}/create-expense?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.post<any>(
      createExpenseEndpoint,
      createExpenseRequest,
      {
        headers: headers,
      }
    );
  }

  deleteExpense(id: string) {
    const email = localStorage.getItem('email');
    const deleteExpenseEndpoint = `${this.apiUrl}/delete-expense?id=${id}&email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.delete<any>(deleteExpenseEndpoint, {
      headers: headers,
    });
  }

  updateExpense(editExpenseRequest: Expense) {
    const email = localStorage.getItem('email');
    const editExpenseEndpoint = `${this.apiUrl}/edit-expense?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(editExpenseEndpoint, editExpenseRequest, {
      headers: headers,
    });
  }

  getAllExpensesAndIncomesByMonth(year: number, month: number) {
    const getAllEndpoint = `${this.apiUrl}/get-expenses-incomes?year=${year}&month=${month}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
      observe: 'response',
    });
  }

  getAllExpensesAndIncomes() {
    const getAllEndpoint = `${this.apiUrl}/get-expenses-incomes-count`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }

  getAmounts() {
    const getAllEndpoint = `${this.apiUrl}/get-expenses-incomes`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }
}
