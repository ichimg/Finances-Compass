import { NotificationService } from '../../services/notification.service';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { FormControl, Validators } from '@angular/forms';
import { EmailMatcher } from '../../error-matchers/email-matcher';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { EmailConfirmationDialog } from 'src/app/dialogs/email-confirmation-dialog/email-confirmation.dialog';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginData = {
    email: '',
    password: '',
  };

  hidePassword: boolean = true;
  isRedirectFromRegister: boolean = false;

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  passwordFormControl = new FormControl('', [
    Validators.required
  ]);

  emailMatcher = new EmailMatcher();

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private notificationService: NotificationService,
    public dialog: MatDialog
  ) {
    this.openEmailConfirmDialog();
  }

  ngOnInit(): void {}
  togglePasswordVisibility() {
    this.hidePassword = !this.hidePassword;
  }

  onSubmitLogin() {
    this.authService
      .login(this.loginData.email, this.loginData.password)
      .subscribe(
        (response) => {
          switch (response.statusCode) {
            case 200:
              localStorage.setItem('email', response.payload.email);
              localStorage.setItem('accessToken', response.payload.accessToken);
              localStorage.setItem(
                'refreshToken',
                response.payload.refreshToken
              );
              localStorage.setItem('currencyPreference', response.payload.currencyPreference);
              localStorage.setItem('userFirstName', response.payload.firstName);
              localStorage.setItem('isDataConsent', response.payload.isDataConsent.toString());

              this.notificationService.showSuccess("You're logged in");
              this.router.navigate(['dashboard']);
              break;

            case 404:
              this.notificationService.showError("Account doesn't exist");
              break;

            case 401:
              this.notificationService.showError('Invalid credentials');
              break;

            case 403:
              this.notificationService.showWarning(
                'Please confirm your e-mail before login'
              );
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        },
        () => {
          this.notificationService.showError('Something went wrong');
        }
      );
  }

  openEmailConfirmDialog(): void {
    const navigation = this.router.getCurrentNavigation();
    if (navigation!.extras.state) {
      this.isRedirectFromRegister =
        navigation!.extras.state['isRedirectFromRegister'];
    }
    if (this.isRedirectFromRegister) {
      const dialogRef = this.dialog.open(EmailConfirmationDialog);
    }
  }
}
