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

  getAllFriends() {
    const getAllEndpoint = `${this.apiUrl}/friends?GetAll=${true}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }

  getFriendsPaginated(pageNumber: number, pageSize: number) {
    const getAllEndpoint = `${this.apiUrl}/friends?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(getAllEndpoint, {
      headers: headers,
    });
  }

  getAllFriendRequests(pageNumber: number, pageSize: number) {
    const getAllEndpoint = `${this.apiUrl}/friend-requests?pageNumber=${pageNumber}&pageSize=${pageSize}`;

    const email = localStorage.getItem('email') || '';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    }).set('email', email);

    return this.http.get<any>(getAllEndpoint, {
      headers: headers,
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

    return await lastValueFrom(
      this.http.put<any>(acceptFriendRequestEndpoint, friendRequestDto, {
        headers: headers,
      })
    );
  }

  async rejectFriendRequest(friendRequestDto: FriendRequestDto) {
    const rejectFriendRequestEndpoint = `${this.apiUrl}/reject-friend-request`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return await lastValueFrom(
      this.http.put<any>(rejectFriendRequestEndpoint, friendRequestDto, {
        headers: headers,
      })
    );
  }

  getRegisteredYear(email: string) {
    const endpoint = `${this.apiUrl}/get-dashboard-year?email=${email}`;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.get<any>(endpoint, {
      headers: headers,
    });
  }

  async changeDashboardYear(email: string, year: number) {
    const endpoint = `${this.apiUrl}/change-dashboard-year?email=${email}&year=${year}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return await lastValueFrom(
      this.http.put<any>(
        endpoint,
        {},
        {
          headers: headers,
        }
      )
    );
  }

  getCurrencyPreference(email: string) {
    const endpoint = `${this.apiUrl}/get-currency-preference?email=${email}`;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.get<any>(endpoint, {
      headers: headers,
    });
  }

  async changeCurrencyPreference(email: string, currency: string) {
    const endpoint = `${this.apiUrl}/change-currency-preference?email=${email}&currency=${currency}`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return await lastValueFrom(
      this.http.put<any>(
        endpoint,
        {},
        {
          headers: headers,
        }
      )
    );
  }

  async getSimilarUsers(email: string | null, numRecommendations: number) {
    const endpoint = `${this.apiUrl}/get-similar-users?email=${email}&numRecommendations=${numRecommendations}`;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return await lastValueFrom(this.http.get<any>(endpoint, {
      headers: headers,
    }));
  }
}
