import { countryValidator } from './../../validators/country-validator';
import { Component } from '@angular/core';
import { countries } from '../../country-data-store';
import { OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';
import { Countries } from '../../entities/country.model';
import { RegisterRequest } from '../../entities/register.request';
import { AuthenticationService } from '../../services/authentication.service';
import { NotificationService } from '../../services/notification.service';
import { Router } from '@angular/router';
import { passwordValidator } from '../../validators/password-validator';
import { passwordMatchingValidator as passwordMatchingValidator } from '../../validators/confirm-password-validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  countries: Countries[] = countries;
  countryFormControl = new FormControl('', [Validators.required, countryValidator(), Validators.maxLength(100)]);
  emailFormControl = new FormControl('', [Validators.required, Validators.email, Validators.maxLength(320)])
  passwordFormControl = new FormControl('', [Validators.required, passwordValidator(), Validators.maxLength(100)]);

  filteredOptions!: Observable<Countries[]>;

  constructor(
    private authService: AuthenticationService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.filteredOptions = this.countryFormControl.valueChanges.pipe(
      startWith(''),
      map((value) => this.filter(value || ''))
    );
  }


  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    lastName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    country: this.countryFormControl,
    state: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    city: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    streetAddress: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    username: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    email: this.emailFormControl,
    phoneNumber: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    password: this.passwordFormControl,
    confirmPassword: new FormControl('', [Validators.required, passwordValidator(), Validators.maxLength(100)]),
  },
  { validators: passwordMatchingValidator });

  private filter(value: string): Countries[] {
    const filterValue = value.toLowerCase();

    return this.countries.filter((country) =>
      country.name.toLowerCase().includes(filterValue)
    );
  }

  onSubmit(): void {
    const registerRequest: RegisterRequest = Object.assign({
      firstName: this.registerForm.value.firstName,
      lastName: this.registerForm.value.lastName,
      country: this.registerForm.value.country,
      state: this.registerForm.value.state,
      city: this.registerForm.value.city,
      streetAddress: this.registerForm.value.streetAddress,
      username: this.registerForm.value.username,
      email: this.registerForm.value.email,
      phoneNumber: this.registerForm.value.phoneNumber,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      clientURI: 'http://localhost:4200/emailconfirmation'
    });

    this.authService.register(registerRequest).subscribe((response) => {
      switch (response.statusCode) {
        case 200:
          this.notificationService.showSuccess('You are now registered!');
          this.router.navigateByUrl('login', { state: { isRedirectFromRegister: true } });
          break;

        case 400:
          this.notificationService.showError('Register failed');
          break;

        case 409:
          this.notificationService.showError(response.message);
          break;

        default:
          this.notificationService.showError('Something went wrong');
          break;
      }
    });
  }
}


