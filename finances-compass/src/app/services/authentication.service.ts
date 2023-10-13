import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginResponse } from '../interfaces/login-response';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { RegisterRequest } from '../interfaces/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  apiUrl = environment.apiUrl;
  readonly headers = new HttpHeaders({
    'Content-Type': 'application/json'
  });

  constructor(private httpClient: HttpClient, private jwtHelper: JwtHelperService) { }

  login(email: string, password: string) { 
    const loginEndpoint = `${this.apiUrl}/login`;

    return this.httpClient.post<LoginResponse>(loginEndpoint, {email: email, password: password}, { headers: this.headers });
  }

  logout(): void {
    localStorage.removeItem('email');
    localStorage.removeItem('token');
  }

  register(registerRequest: RegisterRequest){
    const registerEndpoint = `${this.apiUrl}/register`;

    console.log(registerRequest)
    return this.httpClient.post<any>(registerEndpoint, registerRequest, {headers: this.headers});
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');

    return token ? true : false;
  }

  isTokenExpired(): boolean {
    return this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }
  
}
