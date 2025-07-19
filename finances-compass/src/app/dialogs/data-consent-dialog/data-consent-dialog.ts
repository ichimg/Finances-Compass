import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RegisterRequest } from '../../entities/register.request';
import { AuthenticationService } from '../../services/authentication.service';
import { NotificationService } from '../../services/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-data-consent-dialog',
  templateUrl: './data-consent-dialog.html',
  styleUrls: ['./data-consent-dialog.css'],
})
export class DataConsentDialog implements OnInit {
  registerRequest!: RegisterRequest;

  constructor(
    public dialogRef: MatDialogRef<DataConsentDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private authService: AuthenticationService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerRequest = this.data.registerRequest;
  }
  onToggleChange(event: any) {
    console.log(event.checked);
    this.registerRequest.isDataConsent = event.checked;
  }

  onContinueClick() {
    this.authService.register(this.registerRequest).subscribe((response) => {
      switch (response.statusCode) {
        case 200:
          this.notificationService.showSuccess('You are now registered!');
          this.router.navigateByUrl('login', {
            state: { isRedirectFromRegister: true },
          });
          this.dialogRef.close();
          break;

        case 400:
          this.notificationService.showError('Register failed');
          this.dialogRef.close();
          break;

        case 409:
          this.notificationService.showError(response.message);
          this.dialogRef.close();
          break;

        default:
          this.notificationService.showError('Something went wrong');
          this.dialogRef.close();
          break;
      }
    });
  }
}
