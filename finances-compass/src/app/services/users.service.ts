import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FriendRequest } from '../entities/friend-request.model';
import { DeleteFriendRequest } from '../entities/delete-friend-request.model';

@Injectable()
export class UsersService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllFriends(pageNumber: number, pageSize: number) {
    const getAllEndpoint = `${this.apiUrl}/friends?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(getAllEndpoint, {
      headers: headers,
      observe: 'response',
    });
  }

  getUsersBySearchQuery(query: string, pageNumber: number, pageSize: number) {
    const searchUsersEndpoint = `${this.apiUrl}/search-users?searchQuery=${query}&pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(searchUsersEndpoint, {
      headers: headers,
      observe: 'response',
    });
  }

  addFriend(friendRequest: FriendRequest) {
    const addFriendEndpoint = `${this.apiUrl}/add-friend`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post<any>(addFriendEndpoint, friendRequest, {
      headers: headers,
    });
  }

  cancelFriendRequest(deleteFriendRequest: DeleteFriendRequest) {
    const addFriendEndpoint = `${this.apiUrl}/cancel-friend`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.delete<any>(addFriendEndpoint, {
      body: deleteFriendRequest,
      headers: headers,
    });
  }
}
