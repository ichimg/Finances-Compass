import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  const notificationService = inject(NotificationService)

  if (authService.isAuthenticated() && !authService.isTokenExpired()) {
    return true;
  }

  authService.logout();
  notificationService.showWarning('You need to log in!');
  return router.parseUrl('');
};
