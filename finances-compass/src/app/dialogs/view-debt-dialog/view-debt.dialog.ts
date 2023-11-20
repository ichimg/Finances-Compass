import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-view-debt',
  templateUrl: './view-debt.dialog.html',
  styleUrls: ['./view-debt.dialog.css']
})
export class ViewDebtDialog {

  constructor( public dialogRef: MatDialogRef<ViewDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}


    closeDialog(): void {
      this.dialogRef.close();
    }
}
