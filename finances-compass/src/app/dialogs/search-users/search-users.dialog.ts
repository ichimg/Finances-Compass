import { NotificationService } from './../../services/notification.service';
import { AuthenticationService } from './../../services/authentication.service';
import { UserModel } from './../../entities/user-friend.model';
import { MatDialogRef } from '@angular/material/dialog';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UsersService } from '../../services/users.service';
import { PaginationService } from '../../services/pagination.service';
import { FriendRequest } from '../../entities/friend-request.model';
import { DeleteFriendRequest } from '../../entities/delete-friend-request.model';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.dialog.html',
  styleUrls: ['./search-users.dialog.css'],
  providers: [PaginationService],
})
export class SearchUsersDialog implements OnInit {
  users!: UserModel[] | null;
  usersTotalCount!: number;
  searchedUsers!: any[];

  @ViewChild('searchInput') searchInput!: ElementRef;

  constructor(
    private dialogRef: MatDialogRef<SearchUsersDialog>,
    private http: HttpClient,
    private usersService: UsersService,
    private paginationService: PaginationService,
    private authService: AuthenticationService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {}

  searchUsers(searchInput: any): void {
    this.usersService
      .getUsersBySearchQuery(
        searchInput,
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe((response) => {
        this.users = response.body.payload;

        this.searchedUsers = this.users!.map((user) => {
          const tuple = this.mapFriendStatusIcon(user.friendStatus);
          return {
            userDetails: user,
            iconName: tuple[0],
            iconColor: tuple[1],
            isButtonDisabled: tuple[2],
          };
        });

        this.usersTotalCount = JSON.parse(
          response.headers.get('X-Pagination')!
        ).TotalCount;
      },
      () => {
        this.notificationService.showError('Something went wrong');
      }
    );
  }

  loadMoreUsers(): void {
    if (this.users!.length < this.paginationService.totalCount) {
      this.paginationService.change(this.usersTotalCount);

      this.usersService
        .getUsersBySearchQuery(
          this.searchInput.nativeElement.value,
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe((response) => {
          this.users = this.users!.concat(response.body.payload);

          this.searchedUsers = this.users!.map((user) => {
            const tuple = this.mapFriendStatusIcon(user.friendStatus);
            return {
              userDetails: user,
              iconName: tuple[0],
              iconColor: tuple[1],
              isButtonDisabled: tuple[2],
            };
          });

          this.usersTotalCount = JSON.parse(
            response.headers.get('X-Pagination')!
          ).TotalCount;
        },
        () => {
          this.notificationService.showError('Something went wrong');
        }
      );
    }
  }

  onSearchInputChange(event: Event) {
    this.paginationService.resetPageNumber();
    this.users = null;
  }

  addFriend(user: any) {
    const loggedEmail = this.authService.getEmail();

    if (user.iconName === 'person_add') {
      let addFriendRequest: FriendRequest = {
        requesterUserEmail: loggedEmail!,
        receiverUserEmail: user.userDetails.email,
        status: 'Pending',
      };

      this.usersService.addFriend(addFriendRequest).subscribe((response) => {
        switch (response.statusCode) {
          case 200:
            this.notificationService.showSuccess('Friend request sent');
            user.iconName = 'remove_circle';
            user.iconColor = 'red';
            break;

          default:
            this.notificationService.showError('Friend request sent failed');
            break;
        }
      },
        () => {
          this.notificationService.showError('Something went wrong');
        }
      );
    } 
    
    else if (user.iconName === 'remove_circle') {
      let deleteFriendRequest: DeleteFriendRequest = {
        requesterUserEmail: loggedEmail!,
        receiverUserEmail: user.userDetails.email,
      };

      this.usersService.cancelFriendRequest(deleteFriendRequest).subscribe(
        (response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess('Cancelled friend request');
              user.iconName = 'person_add';
              user.iconColor = 'black';
              break;

            default:
              this.notificationService.showError(
                'Cancel friend request failed'
              );
              break;
          }
        },
        () => {
          this.notificationService.showError('Something went wrong');
        }
      );
    }
  }

  mapFriendStatusIcon(friendStatus: string): [string, string, boolean] {
    switch (friendStatus) {
      case 'None':
        return ['person_add', 'black', false];

      case 'Accepted':
        return ['how_to_reg', 'green', true];

      case 'Pending':
        return ['remove_circle', 'red', false];

      case 'Rejected':
        return ['', '', true];

      default:
        return ['', '', true];
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
