import { UsersService } from './../../services/users.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { DebtsService } from '../../services/debts.service';
import { MatSidenav } from '@angular/material/sidenav';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { UserModel } from '../../entities/user-friend.model';
import { SearchUsersDialog } from '../../dialogs/search-users/search-users.dialog';
import { SidebarService } from '../../services/sidebar.service';
import { Router } from '@angular/router';
import { FriendsDialog } from 'src/app/dialogs/friends-dialog/friends-dialog.dialog';
import { SettingsDialog } from '../../dialogs/settings-dialog/settings-dialog';
import { SimilarUsersDialog } from '../../dialogs/similar-users-dialog/similar-users-dialog';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  providers: [DebtsService],
})
export class LayoutComponent implements OnInit {
  @ViewChild('leftSidebar') public leftSidebar!: MatSidenav;
  @ViewChild('rightSidebar') public rightSidebar!: MatSidenav;

  userFriends!: UserModel[];
  isDataConsent!: boolean;

  sidenavMode: 'side' | 'over' = 'side';

  constructor(
    private sidebarService: SidebarService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.breakpointObserver
      .observe([Breakpoints.Handset])
      .subscribe((result) => {
        if (result.matches) {
          this.sidenavMode = 'over';
        } else {
          this.sidenavMode = 'side';
        }
      });
  }

  ngOnInit(): void {
    this.sidebarService.leftSideNavToggleSubject.subscribe(() => {
      this.leftSidebar?.toggle();
    });

    this.sidebarService.rightSideNavToggleSubject.subscribe(() => {
      this.rightSidebar?.toggle();
    });

    this.router.events.subscribe(() => {
      if (
        this.router.url === '/expenses' ||
        (this.router.url === '/dashboard' && window.innerWidth <= 768)
      ) {
        this.sidenavMode = 'over';
      } else {
        this.sidenavMode = 'side';
      }
    });

    this.isDataConsent = localStorage.getItem('isDataConsent') === 'true';
  }

  goToExpenses(): void {
    if (this.leftSidebar?.opened) {
      this.leftSidebar?.toggle();
      window.location.assign('/expenses');
    }

    if (this.rightSidebar?.opened) {
      this.rightSidebar?.toggle();
      window.location.assign('/expenses');
    }
  }

  openSearchUsersDialog(): void {
    this.dialog.open(SearchUsersDialog, { width: '450px' });
  }

  openFriendsDialog(): void {
    this.dialog.open(FriendsDialog, { width: '400px' });
  }

  openSimilarUsersDialog(): void {
    this.dialog.open(SimilarUsersDialog, { width: '400px' });
  }

  openSettingsDialog(): void {
    this.dialog.open(SettingsDialog, { width: '400px' });
  }
}
