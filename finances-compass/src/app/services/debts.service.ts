import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ViewDebtsResponse } from '../entities/view-debts.response';
import { Debt } from '../entities/debt';

@Injectable({
  providedIn: 'root',
})
export class DebtsService {
  apiUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) {}

  getAllReceivingDebts() {
    const getAllEndpoint = `${this.apiUrl}/view-receiving-debts`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<ViewDebtsResponse>(getAllEndpoint, {
      headers: headers,
    });
  }

  getAllUserDebts() {
    const getAllEndpoint = `${this.apiUrl}/view-user-debts`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<ViewDebtsResponse>(getAllEndpoint, {
      headers: headers,
    });
  }

  createDebt(createDebtRequest: Debt) {
    const email = localStorage.getItem('email');
    const createDebtEndpoint = `${this.apiUrl}/create-debt?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.post<any>(createDebtEndpoint, createDebtRequest, {
      headers: headers,
    });
  }

  deleteDebt(id: string) {
    const email = localStorage.getItem('email');
    const deleteDebtEndpoint = `${this.apiUrl}/delete-debt?id=${id}&email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.delete<any>(deleteDebtEndpoint, {
      headers: headers,
    });
  }

  updateDebt(editDebtRequest: Debt) {
    const email = localStorage.getItem('email');
    const editDebtEndpoint = `${this.apiUrl}/edit-debt?email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(editDebtEndpoint, editDebtRequest, {
      headers: headers,
    });
  }

  approveDebt(debtId: string) {
    const email = localStorage.getItem('email');
    const approveDebtEndpoint = `${this.apiUrl}/approve-debt?debtId=${debtId}&email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(approveDebtEndpoint, { headers: headers });
  }

  rejectDebt(debtId: string) {
    const email = localStorage.getItem('email');
    const approveDebtEndpoint = `${this.apiUrl}/reject-debt?debtId=${debtId}&email=${email}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(approveDebtEndpoint, { headers: headers });
  }

  markDebtPaid(debtId: string) {
    const markPaidDebtEndpoint = `${this.apiUrl}/mark-paid?debtId=${debtId}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient.put<any>(markPaidDebtEndpoint, { headers: headers });
  }

  getAllLoansAndDebts() {
    const getAllEndpoint = `${this.apiUrl}/get-loans-debts-count`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.httpClient.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }
}