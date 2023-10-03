import { Injectable } from '@angular/core';
import { IndividualConfig, ToastrService } from 'ngx-toastr';


export interface toastPayload {
  message: string;
  title: string;
  ic: IndividualConfig;
  type: string;
}

@Injectable({
  providedIn: 'root',
})

export class NotificationService {
  toast!: toastPayload
  constructor(private toastr: ToastrService) {
  
    this.toastr.toastrConfig.enableHtml = true;
  }

  // Toast types: success, error, warning, info
  openNotification(message: string, notificationType: string) {

    switch(notificationType)
    {
      case 'success':
    this.toast = {
      message: message,
      title: notificationType.charAt(0).toUpperCase() + notificationType.slice(1),
      type: notificationType,
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };
    break;

    case 'error':
    this.toast = {
      message: message,
      title: notificationType.charAt(0).toUpperCase() + notificationType.slice(1),
      type: notificationType,
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };
    break;

    case 'warning':
    this.toast = {
      message: message,
      title: notificationType.charAt(0).toUpperCase() + notificationType.slice(1),
      type: notificationType,
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };
    break;

    case 'info':
    this.toast = {
      message: message,
      title: notificationType.charAt(0).toUpperCase() + notificationType.slice(1),
      type: notificationType,
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };
    break;

    default:
      console.log('WRONG NOTIFICATION TYPE!!');
      break;
  }

    this.toastr.show(
      this.toast.message,
      this.toast.title,
      this.toast.ic,
      'toast-' + this.toast.type
    );
  }
}
