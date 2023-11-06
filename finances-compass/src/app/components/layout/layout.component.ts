import { UsersService } from './../../services/users.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { DebtsService } from '../../services/debts.service';
import { SidebarService } from 'src/app/services/sidebar.service';
import { MatSidenav } from '@angular/material/sidenav';
import { UserFriend } from 'src/app/entities/user-friend.model';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  providers: [DebtsService],
})
export class LayoutComponent implements OnInit {
  @ViewChild('leftSidebar') public leftSidebar!: MatSidenav;
  @ViewChild('rightSidebar') public rightSidebar!: MatSidenav;

  userFriends!: UserFriend[];
  constructor(
    private sidebarService: SidebarService,
    private usersService: UsersService
  ) {

  }

  ngOnInit(): void {
    this.sidebarService.leftSideNavToggleSubject.subscribe(()=> {
      this.leftSidebar.toggle();
    });

    this.sidebarService.rightSideNavToggleSubject.subscribe(()=> {
      this.rightSidebar.toggle();
    });

    
  }

  

  openSearchUsersDialog(): void {

  }
}
