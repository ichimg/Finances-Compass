import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPayPalConfig } from 'ngx-paypal';
import { NotificationService } from './notification.service';
import { environment } from '../../environments/environment';
import { lastValueFrom } from 'rxjs';
import { Debt } from '../entities/debt';
import { MatDialogRef } from '@angular/material/dialog';
import { PaymentDialog } from '../dialogs/payment-dialog/payment.dialog';

@Injectable()
export class PaypalService {
  readonly apiUrl = environment.apiUrl;
  readonly intent = "capture";
  constructor(private httpClient: HttpClient, private notificationService: NotificationService) { }

  initPaypalConfig(debt: Debt, dialogRef: MatDialogRef<PaymentDialog>): IPayPalConfig {
    let currency: string | null = localStorage.getItem('currencyPreference');
    let amount: string = debt.amount;
    // make transfer in EUR for RON currencies
    if(currency === 'RON') {
      amount = (parseFloat(debt.amount) * parseFloat(debt.eurExchangeRate)).toString();
    }

    let paypalConfig: IPayPalConfig = {
      currency: localStorage.getItem('currencyPreference')! === 'RON' ? 'EUR' : currency!,
      clientId: 'AU_KG8qslN7KEbDT6PR45YTl1oWAYw5_8uOenV3VjjSPvWS6I-qGXOEHoDUg95d53ZdJy0xDqqANJ8hP',
      createOrderOnServer: async () => {
        try {
          const order = await lastValueFrom(this.httpClient.post<any>(`${this.apiUrl}/create-paypal-order`, {
            intent: this.intent,
            payeeEmail: debt.email,
            currencyCode: localStorage.getItem('currencyPreference')! === 'RON' ? 'EUR' : currency!,
            value: amount.toString()
          }));
          return order.payload;
        } catch (error) {
          console.error('Error creating PayPal order:', error);
          this.notificationService.showError("Payment not made. Please try again later!");
        }
      },
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical'
      },
      authorizeOnServer: async (approveData) => {
        try {
          const details = await lastValueFrom(this.httpClient.post<any>(`${this.apiUrl}/complete-paypal-order`, { 
            intent: this.intent,
            orderId: approveData.orderID,
            debtId: debt.guid
           }));
          this.notificationService.showSuccess("Payment made");
          debt.isPaid = true;
          dialogRef.close();
        } catch (error) {
          console.error('Error authorizing PayPal transaction:', error);
        }
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
        this.notificationService.showWarning("Payment canceled");
      },
      onError: err => {
        console.log('OnError', err);
        this.notificationService.showError("Please try again later")
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
      },
    };

    return paypalConfig;
}
}
