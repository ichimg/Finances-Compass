import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusTransform'
})
export class StatusTransformPipe implements PipeTransform {

  transform(value: string): string {
    if (value === 'Accepted') {
      return 'Awaiting payment';
    }
    return value;
  }

}
