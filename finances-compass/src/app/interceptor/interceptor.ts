import { NotificationService } from './../services/notification.service';
import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    });

    return next.handle(request);
  }
}
