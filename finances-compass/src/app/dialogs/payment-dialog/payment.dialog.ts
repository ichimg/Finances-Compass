import { Component, OnInit, Inject } from '@angular/core';
import { PaypalService } from '../../services/paypal.service';
import { IPayPalConfig } from 'ngx-paypal';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.dialog.html',
  styleUrls: ['./payment.dialog.css'],
  providers: [PaypalService]
})
export class PaymentDialog implements OnInit{
  paypalConfig!: IPayPalConfig

  constructor(
    public dialogRef: MatDialogRef<PaymentDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any, private paypalService: PaypalService) {

    }

    ngOnInit(): void {
      this.paypalConfig = this.paypalService.initPaypalConfig(this.data.debt, this.dialogRef);
    }

    closeDialog(): void {
      this.dialogRef.close();
    }
}
