import { NotificationService } from './../services/notification.service';
import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { FormControl, Validators } from '@angular/forms';
import { EmailMatcher } from 'src/app/error-matchers/email-matcher';
import { passwordValidator } from '../validators/password-validator';
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
      .subscribe((response) => {
        switch(response.statusCode) {

          case 200:
          localStorage.setItem('email', response.payload.email);
          localStorage.setItem('token', response.payload.token);

          this.notificationService.openNotification("You're logged in!", 'success');
          this.router.navigate(['']);
          break;

          case 401:
          this.notificationService.openNotification('Invalid credentials', 'error');
          break;

          default:
          this.notificationService.openNotification('Something went wrong', 'error');
          break;
        }
        
      },
      (error) => {
        this.notificationService.openNotification('Something went wrong', 'error');
      })

      }
  }

