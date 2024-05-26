import { NotificationService } from 'src/app/services/notification.service';
import { PaginationService } from 'src/app/services/pagination.service';
import { UsersService } from 'src/app/services/users.service';
import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { UserModel } from 'src/app/entities/user-friend.model';
import { FriendRequestDto } from 'src/app/entities/friend-request.dto';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-friends-dialog',
  templateUrl: './friends-dialog.dialog.html',
  styleUrls: ['./friends-dialog.dialog.css'],
  providers: [PaginationService],
})
export class FriendsDialog {
  userFriends!: UserModel[];
  friendRequests!: UserModel[];
  userFriendsTotalCount: number = 0;
  friendRequestsTotalCount: number = 0;

  constructor(
    public dialogRef: MatDialogRef<FriendsDialog>,
    private usersService: UsersService,
    private paginationService: PaginationService,
    private notificationService: NotificationService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.usersService
      .getFriendsPaginated(
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe((response) => {
        this.userFriends = response.payload.items;
        this.userFriendsTotalCount = response.payload.totalCount;
      });

    this.usersService
      .getAllFriendRequests(
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe((response) => {
        this.friendRequests = response.payload.items;
        this.friendRequestsTotalCount = response.payload.totalCount;
      });
  }

  async acceptFriendRequest(friendRequest: any) {
    const loggedEmail = this.authService.getEmail();
    let friendRequestDto: FriendRequestDto = {
      requesterUserEmail: loggedEmail!,
      selectedUserEmail: friendRequest.email,
    };

    const response = await this.usersService.acceptFriendRequest(
      friendRequestDto
    );

    switch (response.statusCode) {
      case 200:
        this.notificationService.showSuccess('Friend request accepted');
        friendRequest.isPendingFriendRequest = false;
        this.userFriends.push(friendRequest);
        this.userFriendsTotalCount++;
        this.friendRequestsTotalCount--;
        const index = this.friendRequests.indexOf(friendRequest);
        this.friendRequests.splice(index, 1);
        break;

      default:
        this.notificationService.showError('Please try again later');
        break;
    }
  }

  async rejectFriendRequest(friendRequest: any) {
    const loggedEmail = this.authService.getEmail();
    let friendRequestDto: FriendRequestDto = {
      requesterUserEmail: loggedEmail!,
      selectedUserEmail: friendRequest.email,
    };

    const response = await this.usersService.rejectFriendRequest(
      friendRequestDto
    );

    switch (response.statusCode) {
      case 200:
        this.notificationService.showSuccess('Friend request rejected');
        friendRequest.isPendingFriendRequest = false;
        this.friendRequestsTotalCount--;
        const index = this.friendRequests.indexOf(friendRequest);
        this.friendRequests.splice(index, 1);
        break;

      default:
        this.notificationService.showError('Please try again later');
        break;
    }
  }

  loadMoreFriends(): void {
    if (this.userFriends!.length < this.userFriendsTotalCount) {
      this.paginationService.increasePageNumber();

      this.usersService
        .getFriendsPaginated(
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe(
          (response) => {
            this.userFriends = this.userFriends!.concat(response.payload.items);
          },
          () => {
            this.notificationService.showError('Something went wrong');
          }
        );
    }
  }

  loadMoreFriendRequests(): void {
    if (this.friendRequests!.length < this.friendRequestsTotalCount) {
      this.paginationService.increasePageNumber();

      this.usersService
        .getAllFriendRequests(
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe(
          (response) => {
            this.friendRequests = this.friendRequests!.concat(
              response.payload.items
            );
          },
          () => {
            this.notificationService.showError('Something went wrong');
          }
        );
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
