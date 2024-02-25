import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FriendRequest } from '../entities/friend-request.model';
import { FriendRequestDto } from '../entities/friend-request.dto';
import { lastValueFrom } from 'rxjs';

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

  cancelFriendRequest(deleteFriendRequest: FriendRequestDto) {
    const cancelFriendEndpoint = `${this.apiUrl}/cancel-friend`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.delete<any>(cancelFriendEndpoint, {
      body: deleteFriendRequest,
      headers: headers,
    });
  }

  async acceptFriendRequest(friendRequestDto: FriendRequestDto) {
    const acceptFriendRequestEndpoint = `${this.apiUrl}/accept-friend-request`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    console.log(friendRequestDto);

    return await lastValueFrom(this.http.put<any>(acceptFriendRequestEndpoint, friendRequestDto, {
      headers: headers,
    }));
  }

  async rejectFriendRequest(friendRequestDto: FriendRequestDto) {
    const rejectFriendRequestEndpoint = `${this.apiUrl}/reject-friend-request`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return await lastValueFrom(this.http.put<any>(rejectFriendRequestEndpoint, friendRequestDto, {
      headers: headers,
    }));
  }
}
