import { NotificationService } from './../../services/notification.service';
import { AuthenticationService } from './../../services/authentication.service';
import { UserModel } from './../../entities/user-friend.model';
import { MatDialogRef } from '@angular/material/dialog';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { PaginationService } from '../../services/pagination.service';
import { FriendRequest } from '../../entities/friend-request.model';
import { FriendRequestDto } from '../../entities/friend-request.dto';

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
  lastSearchInput!: string;

  @ViewChild('searchInput') searchInput!: ElementRef;

  constructor(
    private dialogRef: MatDialogRef<SearchUsersDialog>,
    private usersService: UsersService,
    private paginationService: PaginationService,
    private authService: AuthenticationService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {}

  searchUsers(searchInput: any): void {
    if (this.lastSearchInput === searchInput || searchInput === "") {
      return;
    }
    this.usersService
      .getUsersBySearchQuery(
        searchInput,
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe(
        (response) => {
          this.users = response.payload.items;

          this.searchedUsers = this.users!.map((user) => {
            const tuple = this.mapFriendStatusIcon(user.friendStatus);
            return {
              userDetails: user,
              iconName: tuple[0],
              iconColor: tuple[1],
              isButtonDisabled: tuple[2],
            };
          });

          this.usersTotalCount = response.payload.totalCount;
          this.lastSearchInput = searchInput;
        },
        () => {
          this.notificationService.showError('Something went wrong');
        }
      );
  }

  loadMoreUsers(): void {
    if (this.users!.length < this.usersTotalCount) {
      this.paginationService.increasePageNumber();

      this.usersService
        .getUsersBySearchQuery(
          this.searchInput.nativeElement.value,
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe(
          (response) => {
            this.users = this.users!.concat(response.payload.items);

            this.searchedUsers = this.users!.map((user) => {
              const tuple = this.mapFriendStatusIcon(user.friendStatus);
              return {
                userDetails: user,
                iconName: tuple[0],
                iconColor: tuple[1],
                isButtonDisabled: tuple[2],
              };
            });
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

  addOrCancelFriendRequest(user: any) {
    const loggedEmail = this.authService.getEmail();

    if (user.iconName === 'person_add') {
      let addFriendRequest: FriendRequest = {
        requesterUserEmail: loggedEmail!,
        receiverUserEmail: user.userDetails.email,
        status: 'Pending',
      };

      this.usersService.addFriend(addFriendRequest).subscribe(
        (response) => {
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
    } else if (user.iconName === 'remove_circle') {
      let deleteFriendRequest: FriendRequestDto = {
        requesterUserEmail: loggedEmail!,
        selectedUserEmail: user.userDetails.email,
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

  async acceptFriendRequest(user: any) {
    const loggedEmail = this.authService.getEmail();
    let friendRequestDto: FriendRequestDto = {
      requesterUserEmail: loggedEmail!,
      selectedUserEmail: user.userDetails.email,
    };

    const response = await this.usersService.acceptFriendRequest(
      friendRequestDto
    );

    switch (response.statusCode) {
      case 200:
        this.notificationService.showSuccess('Friend request accepted');
        user.userDetails.isPendingFriendRequest = false;
        user.userDetails.friendStatus = 'Accepted';
        user.iconName = 'how_to_reg';
        user.iconColor = 'green';
        user.isButtonDisabled = true;
        break;

      default:
        this.notificationService.showError('Please try again later');
        break;
    }
  }

  async rejectFriendRequest(user: any) {
    const loggedEmail = this.authService.getEmail();
    let friendRequestDto: FriendRequestDto = {
      requesterUserEmail: loggedEmail!,
      selectedUserEmail: user.userDetails.email,
    };

    const response = await this.usersService.rejectFriendRequest(
      friendRequestDto
    );

    switch (response.statusCode) {
      case 200:
        this.notificationService.showSuccess('Friend request rejected');
        user.userDetails.isPendingFriendRequest = false;
        user.userDetails.friendStatus = 'Rejected';
        user.iconName = '';
        user.iconColor = '';
        user.isButtonDisabled = true;
        break;

      default:
        this.notificationService.showError('Please try again later');
        break;
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
