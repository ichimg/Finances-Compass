import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification.service';
import { SidebarService } from 'src/app/services/sidebar.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  authenticationService: AuthenticationService;
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private notificationService: NotificationService,
    private sidebarService: SidebarService
  ) {
    this.authenticationService = authService;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['login']);
    this.notificationService.showSuccess('Logout successful!');
  }

  onLogoClick(): void {
    this.router.navigate(['login']);
  }

  toggleLeftSidebar(): void {
    this.sidebarService.toggleLeftSidebar();
  }

  toggleRightSidebar(): void {
    this.sidebarService.toggleRightSidebar();
  }
}
