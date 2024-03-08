import { lastValueFrom } from 'rxjs';
import { Component, Inject, OnInit } from '@angular/core';
import { Category } from '../../entities/category.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Calendar, EventInput } from '@fullcalendar/core';
import { Expense } from '../../entities/expense.model';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import { MY_DATE_FORMAT } from 'src/app/configurations/my-date-format';
import { ExpensesService } from '../../services/expenses.service';
import { CategoriesService } from '../../services/categories.service';

@Component({
  selector: 'app-add-or-edit-expense-dialog',
  templateUrl: './add-or-edit-expense-dialog.html',
  styleUrls: ['./add-or-edit-expense-dialog.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE],
    },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMAT },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
  ],
})
export class AddOrEditExpenseDialog implements OnInit {
  categories!: Category[];
  calendarApi!: Calendar;
  constructor(
    public dialogRef: MatDialogRef<AddOrEditExpenseDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private notificationService: NotificationService,
    private expensesService: ExpensesService,
    private categoriesService: CategoriesService
  ) {}

  async ngOnInit(): Promise<void> {
    this.calendarApi = this.data.calendarApi;
    const response = await lastValueFrom(this.categoriesService.getAll());
    if (response.statusCode === 200) {
      this.categories = response.payload;
    }
    else {
      this.notificationService.showError('Something went wrong');
    }
  }

  expenseForm = new FormGroup({
    amount: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    date: new FormControl('', Validators.required),
    categorySelect: new FormControl('', Validators.required),
    note: new FormControl('', Validators.maxLength(300)),
  });

  async onSubmit(): Promise<void> {
    if (this.data.selectedExpense !== undefined) {
      this.editExpense();
    } else {
      await this.createExpense();
    }
  }

  async createExpense(): Promise<void> {
    const createExpenseRequest: Expense = Object.assign({
      amount: this.expenseForm.value.amount,
      date: this.expenseForm.value.date,
      category: this.expenseForm.value.categorySelect,
      note: this.expenseForm.value.note,
    });

    console.log(createExpenseRequest);

    const response = await lastValueFrom(
      this.expensesService.createDebt(createExpenseRequest)
    );
    console.log("statusCode", response.statusCode);
    switch (response.statusCode) {
      case 201:
        createExpenseRequest.guid = response.payload;
       // this.data.expenses.push(createExpenseRequest);
        this.notificationService.showSuccess('Expense added successfully!');
        let expense: EventInput = {
          id: createExpenseRequest.guid,
          extendedProps: {
            categoryName: createExpenseRequest.category,
            note: createExpenseRequest.note
          },
          title: `Expense - ${
            this.getCurrency() === 'RON'
              ? createExpenseRequest.amount + ' ' + this.getCurrency()
              : this.getCurrency() + createExpenseRequest.amount
          }`,
          date: `${createExpenseRequest.date.toISOString().split('T', 1)[0]}`,
          color: 'red',
        };

        this.calendarApi.addEvent(expense);
        this.closeDialog();
        break;

      default:
        this.notificationService.showError('Something went wrong');
        break;
    }
  }

  editExpense(): void {}

  closeDialog(): void {
    this.dialogRef.close(); //this.debts);
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
