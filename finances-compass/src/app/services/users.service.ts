import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class UsersService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllFriends(pageNumber: number, pageSize: number) {
    const getAllEndpoint = 
    `${this.apiUrl}/friends?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(getAllEndpoint, { headers: headers, observe: 'response' });
  }

  getUsersBySearchQuery(query: string, pageNumber: number, pageSize: number)
  {
    const searchUsersEndpoint = 
    `${this.apiUrl}/search-users?searchQuery=${query}&pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(searchUsersEndpoint, { headers: headers, observe: 'response' });
  }
}
