import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../entities/user-friend.model';
import { UsersService } from '../../services/users.service';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { AuthenticationService } from '../../services/authentication.service';
import { FriendRequestDto } from '../../entities/friend-request.dto';
import { FriendRequest } from '../../entities/friend-request.model';

@Component({
  selector: 'app-similar-users-dialog',
  templateUrl: './similar-users-dialog.html',
  styleUrls: ['./similar-users-dialog.css'],
})
export class SimilarUsersDialog implements OnInit {
  users!: UserModel[];

  constructor(
    public dialogRef: MatDialogRef<SimilarUsersDialog>,
    private usersService: UsersService,
    private notificationService: NotificationService,
    private authService: AuthenticationService
  ) {}

  async ngOnInit(): Promise<void> {
    await this.getSimilarUsers();
  }

  async getSimilarUsers(): Promise<void> {
    let email = this.authService.getEmail();
    try {
      const response = await this.usersService.getSimilarUsers(email, 10);
      console.log(response.payload);
      if (response.statusCode === 200) {
        this.users = response.payload;
      }
    } catch (error) {
      this.notificationService.showError('Something went wrong');
    }
  }

  addOrCancelFriendRequest(user: any) {
    const loggedEmail = this.authService.getEmail();

    if (user.friendStatus !== 'Pending') {
      let addFriendRequest: FriendRequest = {
        requesterUserEmail: loggedEmail!,
        receiverUserEmail: user.email,
        status: 'Pending',
      };

      this.usersService.addFriend(addFriendRequest).subscribe(
        (response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess('Friend request sent');
              user.friendStatus = 'Pending';
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
    } else if (user.friendStatus === 'Pending') {
      let deleteFriendRequest: FriendRequestDto = {
        requesterUserEmail: loggedEmail!,
        selectedUserEmail: user.email,
      };

      this.usersService.cancelFriendRequest(deleteFriendRequest).subscribe(
        (response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess('Cancelled friend request');
              user.friendStatus = 'None';
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

  closeDialog(): void {
    this.dialogRef.close();
  }
}
