import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { countries } from "../country-data-store";

export function countryValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const country = control.value;
        if (!country) {
          return null; 
        }

     return countries.find(c => c.name === country) ? null : { invalidCountry: true };

  }
}