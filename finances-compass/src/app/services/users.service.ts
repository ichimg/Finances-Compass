import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { FriendsResponse } from '../entities/friends.response';

@Injectable()
export class UsersService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getAllFriends() {
    const getAllEndpoint = `${this.apiUrl}/friends`;

    const email = localStorage.getItem('email') || ''; 
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<FriendsResponse>(getAllEndpoint, { headers: headers });
  }

}
