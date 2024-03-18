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
import { IncomesService } from '../../services/incomes.service';

@Component({
  selector: 'app-add-expense-dialog',
  templateUrl: './add-expense-or-income-dialog.html',
  styleUrls: ['./add-expense-or-income-dialog.css'],
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
export class AddExpenseOrIncomeDialog implements OnInit {
  expenseCategories!: Category[];
  incomeCategories!: Category[];
  calendarApi!: Calendar;
  constructor(
    public dialogRef: MatDialogRef<AddExpenseOrIncomeDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private notificationService: NotificationService,
    private expensesService: ExpensesService,
    private incomesService: IncomesService
  ) {}

  async ngOnInit(): Promise<void> {
    this.calendarApi = this.data.calendarApi;
    if (this.data.isExpense) {
      this.expenseCategories = this.data.expenseCategories;
    } else {
      this.incomeCategories = this.data.incomeCategories;
    }
  }

  form = new FormGroup({
    amount: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    date: new FormControl('', Validators.required),
    categorySelect: new FormControl('', Validators.required),
    note: new FormControl('', Validators.maxLength(300)),
  });

  async onSubmit(): Promise<void> {
    if (this.data.isExpense) {
      await this.createExpense();
    } else {
      await this.createIncome();
    }
  }

  async createExpense(): Promise<void> {
    const createExpenseRequest: Expense = Object.assign({
      amount: this.form.value.amount,
      date: this.form.value.date,
      category: this.form.value.categorySelect,
      note: this.form.value.note,
    });

    const response = await lastValueFrom(
      this.expensesService.createExpense(createExpenseRequest)
    );
    switch (response.statusCode) {
      case 201:
        createExpenseRequest.guid = response.payload;
        const date = new Date(createExpenseRequest.date.toString());
        const utcDate = new Date(
          date.getTime() - date.getTimezoneOffset() * 60000
        );

        this.notificationService.showSuccess('Expense added successfully!');
        let expense: EventInput = {
          id: createExpenseRequest.guid,
          extendedProps: {
            amount: createExpenseRequest.amount,
            categoryName: createExpenseRequest.category,
            note: createExpenseRequest.note,
            isExpense: true,
          },
          title: `Expense -${
            this.getCurrency() === 'RON'
              ? createExpenseRequest.amount + ' ' + this.getCurrency()
              : this.getCurrency() + createExpenseRequest.amount
          }`,
          date: `${utcDate.toISOString().split('T', 1)[0]}`,
          color: '#FF5733',
        };
        this.data.chartData.datasets[0].data[0] =
          parseFloat(this.data.chartData.datasets[0].data[0]) +
          parseFloat(createExpenseRequest.amount);
        this.calendarApi.addEvent(expense);
        this.data.chart.update();
        this.data.balance =
          (parseFloat(this.data.balance) -
          parseFloat(createExpenseRequest.amount)).toFixed(2);
        this.data.balanceTitle = `${
          this.getCurrency() === 'RON'
            ? this.data.balance + ' ' + this.getCurrency()
            : this.getCurrency() + this.data.balance
        }`;

        this.closeDialog();
        break;

      default:
        this.notificationService.showError('Something went wrong');
        break;
    }
  }

  async createIncome(): Promise<void> {
    const createIncomeRequest: Expense = Object.assign({
      amount: this.form.value.amount,
      date: this.form.value.date,
      category: this.form.value.categorySelect,
      note: this.form.value.note,
    });

    const response = await lastValueFrom(
      this.incomesService.createIncome(createIncomeRequest)
    );
    switch (response.statusCode) {
      case 201:
        createIncomeRequest.guid = response.payload;
        const date = new Date(createIncomeRequest.date.toString());
        const utcDate = new Date(
          date.getTime() - date.getTimezoneOffset() * 60000
        );

        this.notificationService.showSuccess('Income added successfully!');
        let expense: EventInput = {
          id: createIncomeRequest.guid,
          extendedProps: {
            amount: createIncomeRequest.amount,
            categoryName: createIncomeRequest.category,
            note: createIncomeRequest.note,
            isExpense: false,
          },
          title: `Income +${
            this.getCurrency() === 'RON'
              ? createIncomeRequest.amount + ' ' + this.getCurrency()
              : this.getCurrency() + createIncomeRequest.amount
          }`,
          date: `${utcDate.toISOString().split('T', 1)[0]}`,
          color: 'green',
        };

        this.calendarApi.addEvent(expense);
        this.data.chartData.datasets[0].data[0] =
          parseFloat(this.data.chartData.datasets[1].data[0]) +
          parseFloat(createIncomeRequest.amount);
        this.calendarApi.addEvent(expense);
        this.data.chart.update();
        this.data.balance =
          (parseFloat(this.data.balance) +
          parseFloat(createIncomeRequest.amount)).toFixed(2);
        this.data.balanceTitle = `${
          this.getCurrency() === 'RON'
            ? this.data.balance + ' ' + this.getCurrency()
            : this.getCurrency() + this.data.balance
        }`;

        this.closeDialog();
        break;

      default:
        this.notificationService.showError('Something went wrong');
        break;
    }
  }

  closeDialog(): void {
    this.dialogRef.close({title: this.data.balanceTitle, remainingBalance: this.data.balance});
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
