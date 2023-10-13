import { NotificationService } from './../services/notification.service';
import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { EMPTY } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    if (this.authService.isAuthenticated()) {
      if (this.authService.isTokenExpired()) {
        this.notificationService.showError("Session expired. Please log in again.");
        this.router.navigate(['']);

        return EMPTY;
      }
      request = request.clone({
        setHeaders:{
        Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });
    }

    return next.handle(request);
  }
}
