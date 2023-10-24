import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginResponse } from '../interfaces/login-response';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { RegisterRequest } from '../interfaces/register-request';
import { lastValueFrom } from 'rxjs';

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
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  }

  register(registerRequest: RegisterRequest){
    const registerEndpoint = `${this.apiUrl}/register`;

    console.log(registerRequest)
    return this.httpClient.post<any>(registerEndpoint, registerRequest, {headers: this.headers});
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');

    return token ? true : false;
  }

  isTokenExpired(): boolean {
    return this.jwtHelper.isTokenExpired(localStorage.getItem('accessToken'));
  }

  async refreshTokens(token: string): Promise<boolean>  {
    const refreshToken: string | null = localStorage.getItem('refreshToken');
  
    console.log(`token: ${token}`)
    console.log(`refreshToken: ${refreshToken}`)
      if (!token || !refreshToken) {
        return false;
      }
  
      const tokenModel = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
      const refreshEndpoint = `${this.apiUrl}/refresh-token`;
    
      let isRefreshSuccess: boolean;
      try {
  
        const response = await lastValueFrom(this.httpClient.post<any>(refreshEndpoint, tokenModel, {headers: this.headers}));
        console.log(response);
        const newToken = (<any>response).payload.accessToken;
        const newRefreshToken = (<any>response).payload.refreshToken;
        const email = (<any>response).payload.email;
        localStorage.setItem("accessToken", newToken);
        localStorage.setItem("refreshToken", newRefreshToken);
        isRefreshSuccess = true;
      }
      catch (ex) {
        console.log(ex);
        isRefreshSuccess = false;
      }
      return isRefreshSuccess;
  };
  
}
