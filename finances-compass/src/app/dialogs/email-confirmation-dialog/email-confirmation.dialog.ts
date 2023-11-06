import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-email-confirmation-dialog',
  templateUrl: './email-confirmation.dialog.html',
  styleUrls: ['./email-confirmation.dialog.css']
})
export class EmailConfirmationDialog {

  constructor(public dialogRef: MatDialogRef<EmailConfirmationDialog>){}

  closeDialog(): void {
    this.dialogRef.close();
  }
}
