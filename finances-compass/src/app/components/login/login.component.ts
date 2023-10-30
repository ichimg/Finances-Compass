import { NotificationService } from '../../services/notification.service';
import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { FormControl, Validators } from '@angular/forms';
import { EmailMatcher } from '../../error-matchers/email-matcher';
import { passwordValidator } from '../../validators/password-validator';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginData = {
    email: '',
    password: '',
  };

  hidePassword: boolean = true;

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  passwordFormControl = new FormControl('', [
    Validators.required,
    passwordValidator(),
  ]);

  emailMatcher = new EmailMatcher();

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

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
                'Please confirm yor e-mail before login'
              );
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        },
        (error) => {
          this.notificationService.showError('Something went wrong');
        }
      );
  }
}
