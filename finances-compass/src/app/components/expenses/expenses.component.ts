import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { MatDialog } from '@angular/material/dialog';
import { AddExpenseOrIncomeDialog } from '../../dialogs/add-expense-or-income-dialog/add-expense-or-income-dialog';
import { Expense } from '../../entities/expense.model';
import { ExpensesService } from '../../services/expenses.service';
import { ViewOrEditExpenseDialog } from '../../dialogs/view-or-edit-expense-dialog/view-or-edit-expense-dialog';
import { a1 } from '@fullcalendar/core/internal-common';
import { Category } from '../../entities/category.model';
import { lastValueFrom } from 'rxjs';
import { CategoriesService } from '../../services/categories.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css'],
})
export class ExpensesComponent implements AfterViewInit, OnInit {
  @ViewChild('calendar') calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    height: 500,
    aspectRatio: 1.5,
    dateClick: (arg) => this.onDateClick(arg),
    // events: [
    //   {
    //     title: 'Expense -500 RON',
    //     date: '2024-03-01',
    //     color: 'red',
    //     id: 'guid1',
    //     extendedProps: {
    //       amount: 500,
    //       categoryName: 'Food',
    //       note: 'testing',
    //     },
    //   },
    //   {
    //     title: 'Income +500 RON',
    //     date: '2024-03-01',
    //     color: 'green',
    //     id: 'guid1',
    //     extendedProps: {
    //       amount: 500,
    //       categoryName: 'Food',
    //       note: 'testing',
    //     },
    //   },
    //   { title: 'event 2', date: '2019-04-02' },
    // ],
    firstDay: 1,
    eventClick: (info) => this.onEventClick(info),
  };

  expenses!: Expense[];
  expenseCategories!: Category[];
  incomeCategories!: Category[];
  expensesAndIncomes!: any[];

  constructor(
    private dialog: MatDialog,
    private expensesService: ExpensesService,
    private categoriesService: CategoriesService,
    private notificationService: NotificationService
  ) {}

  async ngOnInit(): Promise<void> {
    await this.getExpenseCategories();
    await this.getIncomeCategories();
  }

  async ngAfterViewInit(): Promise<void> {
    let calendar = this.calendarComponent.getApi();
    await this.getExpensesAndIncomes();
    calendar.addEventSource(this.expensesAndIncomes);
  }

  async getExpensesAndIncomes(): Promise<void> {
    try {
      const response = await lastValueFrom(
        this.expensesService.getAllExpensesAndIncomes()
      );

      if (response.statusCode === 200) {
        this.expensesAndIncomes = response.payload.map((item: any) => {
          if (item.isExpense) {
            return {
              title: `Expense -${
                this.getCurrency() === 'RON'
                  ? item.amount + ' ' + this.getCurrency()
                  : this.getCurrency() + item.amount
              }`,
              date: item.date.split("T")[0],
              color: 'red',
              id: item.id,
              extendedProps: {
                amount: item.amount,
                categoryName: item.category,
                note: item.note,
                isExpense: item.isExpense,
              },
            };
          } else {
            return {
              title: `Income +${
                this.getCurrency() === 'RON'
                  ? item.amount + ' ' + this.getCurrency()
                  : this.getCurrency() + item.amount
              }`,
              date: item.date.split("T")[0],
              color: 'green',
              id: item.id,
              extendedProps: {
                amount: item.amount,
                categoryName: item.category,
                note: item.note,
                isExpense: item.isExpense,
              },
            };
          }
        });
      }
    } catch (error) {
      this.notificationService.showError('Something went wrong');
    }
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

  async getExpenseCategories(): Promise<void> {
    const response = await lastValueFrom(
      this.categoriesService.getAllExpense()
    );
    if (response.statusCode === 200) {
      this.expenseCategories = response.payload;
    } else {
      this.notificationService.showError('Something went wrong');
    }
  }

  async getIncomeCategories(): Promise<void> {
    const response = await lastValueFrom(this.categoriesService.getAllIncome());
    if (response.statusCode === 200) {
      this.incomeCategories = response.payload;
    } else {
      this.notificationService.showError('Something went wrong');
    }
  }

  onEventClick(info: a1) {
    const dateStr = info.event.start?.toDateString();
    let date = new Date(dateStr!);
    var formattedDate =
      date.toLocaleDateString('en-US', { weekday: 'long' }) +
      ', ' +
      date
        .toLocaleDateString('ro-RO', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\./g, '/');

    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog.open(ViewOrEditExpenseDialog, {
      width: '25%',
      data: {
        calendarApi: calendar,
        item: info.event,
        expenseCategories: this.expenseCategories,
        incomeCategories: this.incomeCategories,
        titleDate: formattedDate,
      },
    });
  }

  addExpense(): void {
    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog.open(AddExpenseOrIncomeDialog, {
      data: {
        calendarApi: calendar,
        expenses: this.expenses,
        expenseCategories: this.expenseCategories,
        isExpense: true,
      },
    });
  }

  addIncome(): void {
    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog.open(AddExpenseOrIncomeDialog, {
      data: {
        calendarApi: calendar,
        expenses: this.expenses,
        incomeCategories: this.incomeCategories,
        isExpense: false,
      },
    });
  }

  onDateClick(arg: DateClickArg) {
    let calendarApi = this.calendarComponent.getApi();
    console.log('ce are');
    //this.calendarOptions.events = [{ title: 'Expense - 500 RON', date: arg.dateStr }];
    calendarApi.addEvent({
      title: 'Expense - 500 RON',
      date: arg.dateStr,
      color: 'red',
    });
    let event: EventInput = { title: 'Expense - 500 RON', date: arg.dateStr };
  }
}
