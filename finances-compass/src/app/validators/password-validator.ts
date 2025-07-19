import { AbstractControl, ValidatorFn } from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  const passwordRegex: RegExp = /^(?=.*[A-Z])(?=.*\d).{8,}$/;

  return (control: AbstractControl): { [key: string]: any } | null => {
    const value = control.value;
    if (!value) {
      return null; 
    }

    return passwordRegex.test(value) ? null : { invalidPassword: true };
  };
}