import { Category } from './../../entities/category.model';
import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { MatDialog } from '@angular/material/dialog';
import { AddExpenseOrIncomeDialog } from '../../dialogs/add-expense-or-income-dialog/add-expense-or-income-dialog';
import { Expense } from '../../entities/expense.model';
import { ExpensesService } from '../../services/expenses.service';
import { ViewOrEditExpenseDialog } from '../../dialogs/view-or-edit-expense-dialog/view-or-edit-expense-dialog';
import { a1 } from '@fullcalendar/core/internal-common';
import { lastValueFrom } from 'rxjs';
import { CategoriesService } from '../../services/categories.service';
import { NotificationService } from '../../services/notification.service';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css'],
})
export class ExpensesComponent implements OnInit {
  @ViewChild('calendar') calendarComponent!: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    height: 500,
    aspectRatio: 1.5,
    firstDay: 1,
    eventClick: (info) => this.onEventClick(info),
    datesSet: async (dateInfo) => {
      let midDate = new Date(
        (dateInfo.start.getTime() + dateInfo.end.getTime()) / 2
      );

      await this.getExpensesAndIncomes(
        midDate.getFullYear(),
        midDate.getMonth() + 1
      );
      let calendar = this.calendarComponent.getApi();
      calendar.removeAllEventSources();
      calendar.addEventSource(this.expensesAndIncomes);
      this.chart.chart?.update();
    },
  };

  expenses!: Expense[];
  expenseCategories!: Category[];
  incomeCategories!: Category[];
  expensesAndIncomes!: any[];
  balanceTitle!: string;
  balance!: number;

  data: any = {
    labels: [''],
    datasets: [
      {
        label: 'Expense',
        data: [50],
        backgroundColor: '#FF5733',
        barThickness: 150,
        hoverBackgroundColor: '#cc4427',
      },
      {
        label: 'Income',
        data: [50],
        backgroundColor: 'green',
        barThickness: 150,
        hoverBackgroundColor: '#085410',
      },
    ],
  };

  options: any = {
    maintainAspectRatio: true,
    responsive: true,
    indexAxis: 'y',
    scales: {
      x: {
        stacked: true,
        display: false,
      },
      y: {
        stacked: true,
        display: false,
      },
    },
    plugins: {
      legend: {
        display: true,
        position: 'bottom',
        align: 'center',
        labels: {
          textAlign: 'center',
        },
      },
    },
  };
  @ViewChild(BaseChartDirective, { static: false }) chart!: BaseChartDirective;

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

  async getExpensesAndIncomes(year: number, month: number): Promise<void> {
    try {
      const response = await lastValueFrom(
        this.expensesService.getAllExpensesAndIncomes(year, month)
      );

      if (response.body.statusCode === 200) {
        this.expensesAndIncomes = response.body.payload.map((item: any) => {
          if (item.isExpense) {
            return {
              title: `Expense -${
                this.getCurrency() === 'RON'
                  ? item.amount + ' ' + this.getCurrency()
                  : this.getCurrency() + item.amount
              }`,
              date: item.date.split('T')[0],
              color: '#FF5733',
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
              date: item.date.split('T')[0],
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
      let totalExpenses = JSON.parse(
        response.headers.get('X-Total')!
      ).TotalAmountExpenses;
      let totalIncomes = JSON.parse(
        response.headers.get('X-Total')!
      ).TotalAmountIncomes;
      this.balance = parseFloat((totalIncomes - totalExpenses).toFixed(2));
      this.data.datasets[0].data[0] = totalExpenses;
      this.data.datasets[1].data[0] = totalIncomes;

      this.balanceTitle = `${
        this.getCurrency() === 'RON'
          ? this.balance + ' ' + this.getCurrency()
          : this.getCurrency() + this.balance
      }`;
      console.log(response);
    } catch (error) {
      console.log(error);
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
      this.expenseCategories = response.payload.filter(
        (category: Category) => category.name != 'Debts'
      );
    } else {
      this.notificationService.showError('Something went wrong');
    }
  }

  async getIncomeCategories(): Promise<void> {
    const response = await lastValueFrom(this.categoriesService.getAllIncome());
    if (response.statusCode === 200) {
      this.incomeCategories = response.payload.filter(
        (category: Category) => category.name != 'Debts'
      );
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
    const dialogRef = this.dialog
      .open(ViewOrEditExpenseDialog, {
        width: '25%',
        data: {
          calendarApi: calendar,
          item: info.event,
          expenseCategories: this.expenseCategories,
          incomeCategories: this.incomeCategories,
          titleDate: formattedDate,
          chartData: this.data,
          chart: this.chart.chart,
          balanceTitle: this.balanceTitle,
          balance: this.balance,
        },
      })
      .afterClosed()
      .subscribe((resp) => {
        this.balanceTitle = resp.title;
        this.balance = resp.remainingBalance;
      });
  }

  addExpense(): void {
    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog
      .open(AddExpenseOrIncomeDialog, {
        data: {
          calendarApi: calendar,
          expenses: this.expenses,
          expenseCategories: this.expenseCategories,
          isExpense: true,
          chartData: this.data,
          chart: this.chart.chart,
          balanceTitle: this.balanceTitle,
          balance: this.balance,
        },
      })
      .afterClosed()
      .subscribe((resp) => {
        this.balanceTitle = resp.title;
        this.balance = resp.remainingBalance;
      });
  }

  addIncome(): void {
    let calendar = this.calendarComponent.getApi();
    const dialogRef = this.dialog
      .open(AddExpenseOrIncomeDialog, {
        data: {
          calendarApi: calendar,
          expenses: this.expenses,
          incomeCategories: this.incomeCategories,
          isExpense: false,
          chartData: this.data,
          chart: this.chart.chart,
          balanceTitle: this.balanceTitle,
          balance: this.balance,
        },
      })
      .afterClosed()
      .subscribe((resp) => {
        this.balanceTitle = resp.title;
        this.balance = resp.remainingBalance;
      });
  }
}
