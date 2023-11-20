import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

export const authGuard: CanActivateFn = async (_route, _state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  const notificationService = inject(NotificationService)
  const token = localStorage.getItem('accessToken');

  let isTokenExpired = await authService.isTokenExpired();
  if (token && await !isTokenExpired) {
    return true;
  }

  const isRefreshSuccess =  await authService.refreshTokens(token!);
  if (!isRefreshSuccess) {
    authService.logout();
    notificationService.showWarning('You need to log in!');
    return router.parseUrl('login');
  }

  return isRefreshSuccess;
};