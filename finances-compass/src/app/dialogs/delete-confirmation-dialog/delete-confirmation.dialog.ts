import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { DebtsService } from '../../services/debts.service';
import { Debt } from '../../entities/debt';
import { ExpensesService } from '../../services/expenses.service';
import { IncomesService } from '../../services/incomes.service';

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
    private debtsService: DebtsService,
    private expensesService: ExpensesService,
    private incomesService: IncomesService
  ) {}

  delete(): void {
    if (this.data.debt !== undefined) {
      this.debtsService
        .deleteDebt(this.data.debt.guid)
        .subscribe((response) => {
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

    if (this.data.item.extendedProps['isExpense']) {
      this.expensesService
        .deleteExpense(this.data.item.id)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess('Expense deleted');
              this.closeDialog();
              break;

            case 204:
              alert('Expense already deleted');
              window.location.reload();
              break;

            default:
              this.notificationService.showError('Something went wrong!');
              this.closeDialog(false);
              break;
          }
        });
    }
    else {
      this.incomesService
      .deleteIncome(this.data.item.id)
      .subscribe((response) => {
        switch (response.statusCode) {
          case 200:
            this.notificationService.showSuccess('Income deleted');
            this.closeDialog();
            break;

          case 204:
            alert('Income already deleted');
            window.location.reload();
            break;

          default:
            this.notificationService.showError('Something went wrong!');
            this.closeDialog(false);
            break;
        }
      });
    }
  }

  closeDialog(isSuccess: boolean = true): void {
    if (this.data.debt !== undefined) {
      this.dialogRef.close(this.data.debtsList);
    }

    if(this.data.item !== undefined) {
      this.dialogRef.close(isSuccess);
    }
  }
}
