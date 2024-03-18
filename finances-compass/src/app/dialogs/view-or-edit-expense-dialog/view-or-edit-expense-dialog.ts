import { ExpensesService } from './../../services/expenses.service';
import { Calendar, EventInput } from '@fullcalendar/core';
import { Component, Inject, OnInit } from '@angular/core';
import { Category } from '../../entities/category.model';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialog,
} from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DeleteConfirmationDialog } from '../delete-confirmation-dialog/delete-confirmation.dialog';
import { Expense } from '../../entities/expense.model';
import { IncomesService } from '../../services/incomes.service';

@Component({
  selector: 'app-view-or-edit-expense-dialog',
  templateUrl: './view-or-edit-expense-dialog.html',
  styleUrls: ['./view-or-edit-expense-dialog.css'],
})
export class ViewOrEditExpenseDialog implements OnInit {
  expenseCategories!: Category[];
  incomeCategories!: Category[];
  calendarApi!: Calendar;
  isEditingModal: boolean = false;
  isEdited: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<ViewOrEditExpenseDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private notificationService: NotificationService,
    private expensesService: ExpensesService,
    private incomesService: IncomesService,
    private dialog: MatDialog
  ) {}

  async ngOnInit(): Promise<void> {
    this.calendarApi = this.data.calendarApi;
    this.expenseCategories = this.data.expenseCategories;
    this.incomeCategories = this.data.incomeCategories;
  }

  form = new FormGroup({
    amount: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    categorySelect: new FormControl('', Validators.required),
    note: new FormControl('', Validators.maxLength(300)),
  });

  closeDialog(): void {
    this.dialogRef.close({
      title: this.data.balanceTitle,
      remainingBalance: this.data.balance,
    });
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

  async activateEditForm(): Promise<void> {
    this.isEditingModal = true;
    this.fillEditModalForm();
  }

  async delete(): Promise<void> {
    const dialogRef = this.dialog
      .open(DeleteConfirmationDialog, {
        width: '600px',
        data: { item: this.data.item },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result.isSuccess) {
          let event = this.calendarApi.getEventById(this.data.item.id);
          event?.remove();
          console.log(result.amount);

          if (this.data.item.extendedProps['isExpense']) {
            this.data.chartData.datasets[0].data[0] =
              parseFloat(this.data.chartData.datasets[0].data[0]) -
              parseFloat(result.amount);
            this.data.chart.update();
            this.data.balance = (
              parseFloat(this.data.balance) + parseFloat(result.amount)
            ).toFixed(2);
            this.data.balanceTitle = `${
              this.getCurrency() === 'RON'
                ? this.data.balance + ' ' + this.getCurrency()
                : this.getCurrency() + this.data.balance
            }`;

            console.log(this.data.balanceTitle);
          } else {
            this.data.chartData.datasets[1].data[0] =
              parseFloat(this.data.chartData.datasets[1].data[0]) -
              parseFloat(result.amount);
            this.data.chart.update();
            this.data.balance = (
              parseFloat(this.data.balance) - parseFloat(result.amount)
            ).toFixed(2);
            this.data.balanceTitle = `${
              this.getCurrency() === 'RON'
                ? this.data.balance + ' ' + this.getCurrency()
                : this.getCurrency() + this.data.balance
            }`;
          }

          this.closeDialog();
        }
      });
  }

  async edit(): Promise<void> {
    if (this.data.item.extendedProps['isExpense']) {
      const editExpenseRequest: Expense = Object.assign({
        guid: this.data.item.id,
        amount: this.form.value.amount,
        category: this.form.value.categorySelect,
        note: this.form.value.note
      });

      this.expensesService
        .updateExpense(editExpenseRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess(
                'Expense updated successfully!'
              );

              this.data.chartData.datasets[0].data[0] =
                parseFloat(this.data.chartData.datasets[1].data[0]) -
                parseFloat(this.data.item.extendedProps['amount']);
              this.data.chartData.datasets[0].data[0] =
                parseFloat(this.data.chartData.datasets[1].data[0]) +
                parseFloat(editExpenseRequest.amount);
              this.data.balance =
                parseFloat(this.data.balance) +
                parseFloat(this.data.item.extendedProps['amount']);
              this.data.balance = (
                parseFloat(this.data.balance) -
                parseFloat(editExpenseRequest.amount)
              ).toFixed(2);
              this.data.balanceTitle = `${
                this.getCurrency() === 'RON'
                  ? this.data.balance + ' ' + this.getCurrency()
                  : this.getCurrency() + this.data.balance
              }`;
              this.data.chart.update();

              let event = this.calendarApi.getEventById(
                editExpenseRequest.guid
              );
              const date = new Date(event?.start?.toDateString()!);
              const utcDate = new Date(
                date.getTime() - date.getTimezoneOffset() * 60000
              );
              event?.remove();

              let expense: EventInput = {
                id: editExpenseRequest.guid,
                extendedProps: {
                  amount: editExpenseRequest.amount,
                  categoryName: editExpenseRequest.category,
                  note: editExpenseRequest.note,
                  isExpense: true,
                },
                title: `Expense -${
                  this.getCurrency() === 'RON'
                    ? editExpenseRequest.amount + ' ' + this.getCurrency()
                    : this.getCurrency() + editExpenseRequest.amount
                }`,
                date: `${utcDate.toISOString().split('T', 1)[0]}`,
                color: '#FF5733',
              };

              this.calendarApi.addEvent(expense);
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    } else {
      const editIncomeRequest: Expense = Object.assign({
        guid: this.data.item.id,
        amount: this.form.value.amount,
        category: this.form.value.categorySelect,
        note: this.form.value.note
      });

      this.incomesService
        .updateIncome(editIncomeRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 200:
              this.notificationService.showSuccess(
                'Income updated successfully!'
              );

              this.data.chartData.datasets[1].data[0] =
                parseFloat(this.data.chartData.datasets[1].data[0]) -
                parseFloat(this.data.item.extendedProps['amount']);
              this.data.chartData.datasets[1].data[0] =
                parseFloat(this.data.chartData.datasets[1].data[0]) +
                parseFloat(editIncomeRequest.amount);
              this.data.balance =
                parseFloat(this.data.balance) -
                parseFloat(this.data.item.extendedProps['amount']);
              this.data.balance = (
                parseFloat(this.data.balance) +
                parseFloat(editIncomeRequest.amount)
              ).toFixed(2);
              this.data.balanceTitle = `${
                this.getCurrency() === 'RON'
                  ? this.data.balance + ' ' + this.getCurrency()
                  : this.getCurrency() + this.data.balance
              }`;
              this.data.chart.update();

              let event = this.calendarApi.getEventById(editIncomeRequest.guid);
              const date = new Date(event?.start?.toDateString()!);
              const utcDate = new Date(
                date.getTime() - date.getTimezoneOffset() * 60000
              );
              event?.remove();

              let expense: EventInput = {
                id: editIncomeRequest.guid,
                extendedProps: {
                  amount: editIncomeRequest.amount,
                  categoryName: editIncomeRequest.category,
                  note: editIncomeRequest.note,
                  isExpense: false
                },
                title: `Income +${
                  this.getCurrency() === 'RON'
                    ? editIncomeRequest.amount + ' ' + this.getCurrency()
                    : this.getCurrency() + editIncomeRequest.amount
                }`,
                date: `${utcDate.toISOString().split('T', 1)[0]}`,
                color: 'green',
              };

              this.calendarApi.addEvent(expense);
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    }
  }

  fillEditModalForm(): void {
    this.form.controls['amount'].setValue(
      this.data.item.extendedProps['amount']
    );
    this.form.controls['categorySelect'].setValue(
      this.data.item.extendedProps['categoryName']
    );
    this.form.controls['note'].setValue(this.data.item.extendedProps['note']);

    this.listenOnFormChanges();
  }

  listenOnFormChanges() {
    this.form.controls['amount'].valueChanges.subscribe((newValue) => {
      this.isEdited = true;
    });
    this.form.controls['categorySelect'].valueChanges.subscribe((newValue) => {
      this.isEdited = true;
    });
    this.form.controls['note'].valueChanges.subscribe((newValue) => {
      this.isEdited = true;
    });
  }
}
