import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-view-debt',
  templateUrl: './view-debt.dialog.html',
  styleUrls: ['./view-debt.dialog.css']
})
export class ViewDebtDialog implements OnInit {
  currency!: string;
  
  constructor( public dialogRef: MatDialogRef<ViewDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

    ngOnInit(): void {
      this.currency = localStorage.getItem('currencyPreference')!;
    }
    closeDialog(): void {
      this.dialogRef.close();
    }
}
