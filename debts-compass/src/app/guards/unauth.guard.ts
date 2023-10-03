import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

export const unauthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  const notificationService = inject(NotificationService)

  if (authService.isAuthenticated()) {
    router.navigate(['']);
    return false;
  }

  return true;
};
