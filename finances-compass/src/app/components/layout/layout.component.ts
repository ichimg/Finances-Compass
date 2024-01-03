import { UsersService } from './../../services/users.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { DebtsService } from '../../services/debts.service';
import { MatSidenav } from '@angular/material/sidenav';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { UserModel } from '../../entities/user-friend.model';
import { SearchUsersDialog } from '../../dialogs/search-users/search-users.dialog';
import { SidebarService } from '../../services/sidebar.service';

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

  sidenavMode: 'side' | 'over' = 'side';

  constructor(
    private sidebarService: SidebarService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog
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
  }

  openSearchUsersDialog(): void {
    this.dialog.open(SearchUsersDialog);

  }
}
