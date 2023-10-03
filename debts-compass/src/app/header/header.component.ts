import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  authenticationService: AuthenticationService;
  constructor(private authService: AuthenticationService, private router: Router, private notificationService: NotificationService)
  { 
    this.authenticationService = authService;
  }

  logout(): void{
    this.authService.logout();
    this.router.navigate(['/login']);
    this.notificationService.openNotification('Logout successful!', 'success');
  }

}
