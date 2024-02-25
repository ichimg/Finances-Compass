import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-view-debt',
  templateUrl: './view-debt.dialog.html',
  styleUrls: ['./view-debt.dialog.css']
})
export class ViewDebtDialog implements OnInit {
  
  constructor( public dialogRef: MatDialogRef<ViewDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

    ngOnInit(): void {
    }
    closeDialog(): void {
      this.dialogRef.close();
    }

    getCurrency(): string {
      let currency = localStorage.getItem('currencyPreference');
      if (currency === 'EUR') {
        return '€';
      }
  
      if (currency === 'USD') {
        return '$';
      }
  
      return 'RON';
    }
}
