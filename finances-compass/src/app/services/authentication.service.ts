import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginResponse } from '../entities/login.response';
import {JwtHelperService} from '@auth0/angular-jwt';
import { RegisterRequest } from '../entities/register.request';
import { lastValueFrom } from 'rxjs';
import { CustomEncoder } from '../custom-encoder';

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
  
      if (!token || !refreshToken) {
        return false;
      }
  
      const tokenModel = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
      const refreshEndpoint = `${this.apiUrl}/refresh-token`;
    
      let isRefreshSuccess: boolean;
      try {
  
        const response = await lastValueFrom(this.httpClient.post<any>(refreshEndpoint, tokenModel, {headers: this.headers}));
        console.log(`Refresh response: ${(<any>response).payload}`);
        const newToken = (<any>response).payload.accessToken;
        const newRefreshToken = (<any>response).payload.refreshToken;
        localStorage.setItem('accessToken', newToken);
        localStorage.setItem('refreshToken', newRefreshToken);
        isRefreshSuccess = true;
      }
      catch (ex) {
        console.log(ex);
        isRefreshSuccess = false;
      }

      console.log(`refresh success: ${isRefreshSuccess}`)
      return isRefreshSuccess;
  };

  public confirmEmail = (route: string, token: string, email: string) => {
    let params = new HttpParams({ encoder: new CustomEncoder() })
    params = params.append('token', token);
    params = params.append('email', email);
    return this.httpClient.get(this.createCompleteRoute(route, this.apiUrl), { params: params });
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
  
}
