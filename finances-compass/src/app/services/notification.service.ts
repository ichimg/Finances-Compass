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

  showSuccess(message: string){
    this.toast = {
      message: message,
      title: 'Success',
      type: 'success',
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };

    this.displayNotification();
  }

  showWarning(message: string){
    this.toast = {
      message: message,
      title: 'Warning',
      type: 'warning',
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };

    this.displayNotification();
  }

  showError(message: string){
    this.toast = {
      message: message,
      title: 'Error',
      type: 'error',
      ic: {
        timeOut: 3000,
        closeButton: true
      } as IndividualConfig,
    };

    this.displayNotification();
  }

    showInfo(message: string){
      this.toast = {
        message: message,
        title: 'Info',
        type: 'info',
        ic: {
          timeOut: 3000,
          closeButton: true
        } as IndividualConfig,
      };
    

    this.displayNotification();
    }

  private displayNotification(): void {
    this.toastr.show(
      this.toast.message,
      this.toast.title,
      this.toast.ic,
      'toast-' + this.toast.type
    );
  }
  
}

