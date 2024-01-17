import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

export const authGuard: CanActivateFn = async (_route, _state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  const notificationService = inject(NotificationService)
  const accessToken = localStorage.getItem('accessToken');

  let isTokenExpired = await authService.isTokenExpired();
  if (accessToken && await !isTokenExpired) {
    return true;
  }

  const isRefreshSuccess =  await authService.refreshTokens(accessToken!);
  if (!isRefreshSuccess) {
    authService.logout();
    notificationService.showWarning('You need to log in!');
    return router.parseUrl('login');
  }

  return isRefreshSuccess;
};