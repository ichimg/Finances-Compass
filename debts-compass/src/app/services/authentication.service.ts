import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { LoginResponse } from '../interfaces/login-response';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  login(email: string, password: string) { 
    const loginEndpoint = `${this.apiUrl}/login`;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<LoginResponse>(loginEndpoint, {email: email, password: password}, { headers: headers });
  }

  logout(): void {
    localStorage.removeItem('email');
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');

    return token ? true : false;
  }

  isTokenExpired(): boolean {
    return this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }
  
}
