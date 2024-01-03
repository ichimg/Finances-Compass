import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { DebtsService } from '../../services/debts.service';
import { Debt } from '../../entities/debt';

@Component({
  selector: 'app-delete-confirmation',
  templateUrl: './delete-confirmation.dialog.html',
  styleUrls: ['./delete-confirmation.dialog.css'],
})
export class DeleteConfirmationDialog {
  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmationDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private notificationService: NotificationService,
    private debtsService: DebtsService
  ) {}

  deleteDebt(): void {
    this.debtsService.deleteDebt(this.data.debt.guid).subscribe((response) => {
      switch (response.statusCode) {
        case 200:
          this.notificationService.showSuccess('Debt deleted');
          const index = this.data.debtsList.indexOf(this.data.debt);
          this.data.debtsList.splice(index, 1);
          this.closeDialog();
          break;

        case 204:
          alert('Debt already deleted');
          window.location.reload();
          break;

        default:
          this.notificationService.showError('Something went wrong!');
          this.closeDialog();
          break;
      }
    });


  }

  closeDialog(): void {
    this.dialogRef.close(this.data.debtsList);
  }
}
