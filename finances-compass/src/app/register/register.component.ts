import { Component } from '@angular/core';
import { countries } from '../country-data-store';
import { OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, map, startWith } from 'rxjs';
import { Countries } from '../interfaces/country.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  countries: Countries[] = countries;
  formControl = new FormControl('');

  registrationForm!: FormGroup;
  filteredOptions!: Observable<Countries[]>;

  registerData = {
    firstName: '',
    lastName: '',
    country: '',
    state: '',
    city: '',
    postalCode: '',
    streetAddress: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    iban: ''
  };

  constructor(private formBuilder: FormBuilder) {
     }

  ngOnInit() {
    this.filteredOptions = this.formControl.valueChanges.pipe(
      startWith(''),
      map((value) => this.filter(value || ''))
    );
    this.registrationForm = this.formBuilder.group({
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      state: [null, Validators.required],
      city: [null, Validators.required],
      postalCode: [null, Validators.required],
      streetAddress: [null, Validators.required],
      email: [null, Validators.email, Validators.required],
      phoneNumber: [null, Validators.required],
      password: [null, Validators.required],
      confirmPassword: [null, Validators.required],
      iban: [null, Validators.required]
    });

    this.registrationForm = this.formBuilder.group({

    });
 
  }

  private filter(value: string): Countries[] {
    const filterValue = value.toLowerCase();

    return this.countries.filter((country) =>
      country.name.toLowerCase().includes(filterValue)
    );
  }

  onSubmit(): void{
    console.log(this.registerData);
  }

}
