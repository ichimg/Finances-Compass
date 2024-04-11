import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { lastValueFrom } from 'rxjs';
import { ExpensesService } from 'src/app/services/expenses.service';
import { NotificationService } from 'src/app/services/notification.service';
import { OverallBalanceStats } from '../../entities/overall-balance-stats.model';
import { DebtsService } from '../../services/debts.service';
import { LoansDebtsStats } from '../../entities/loans-debts-stats.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  username: string = 'Gabi';

  // Overall balance
  public overallBalanceChart: any;
  overallBalanceOptions: any;
  overallBalanceData: any;
  totalExpensesAndIncomes: any;
  overallBalanceStats: OverallBalanceStats = {
    totalExpenses: '',
    totalIncomes: '',
  };

  // Loans & Debts
  public loansDebtsChart: any;
  loansDebtsOptions: any;
  loansDebtsData: any;
  totalLoansAndDebts: any;
  loansDebtsStats: LoansDebtsStats = {
    totalLoans: '',
    totalDebts: '',
  };

  constructor(
    private expensesService: ExpensesService,
    private debtsService: DebtsService,
    private notificationService: NotificationService
  ) {}

  async ngOnInit(): Promise<void> {
    await this.populateOverallBalanceChart();
    await this.populateLoansDebtsChart();
  }

  async populateOverallBalanceChart(): Promise<void> {
    try {
      const response = await lastValueFrom(
        this.expensesService.getAllExpensesAndIncomes()
      );

      this.totalExpensesAndIncomes = response.payload;
      this.createOverallBalanceChart(this.totalExpensesAndIncomes);
    } catch (error) {
      this.notificationService.showError('Something went wrong!');
    }

    this.overallBalanceStats = {
      totalIncomes: `${
        this.getCurrency() === 'RON'
          ? '+' +
            parseFloat(this.totalExpensesAndIncomes.totalIncomes).toFixed(2) +
            ' ' +
            this.getCurrency()
          : this.getCurrency() +
            '+' +
            parseFloat(this.totalExpensesAndIncomes.totalIncomes).toFixed(2)
      }`,

      totalExpenses: `${
        this.getCurrency() === 'RON'
          ? '-' +
            parseFloat(this.totalExpensesAndIncomes.totalExpenses).toFixed(2) +
            ' ' +
            this.getCurrency()
          : this.getCurrency() +
            '-' +
            parseFloat(this.totalExpensesAndIncomes.totalExpenses).toFixed(2)
      }`,
    };
  }

  createOverallBalanceChart(data: any): void {
    let total = parseFloat(data.totalExpenses) + parseFloat(data.totalIncomes);
    this.overallBalanceData = {
      // values on X-Axis
      labels: ['Expenses', 'Incomes'],
      datasets: [
        {
          label: 'Overall Balance',
          data: [data.totalExpenses, data.totalIncomes],
          backgroundColor: ['red', 'darkgreen'],
          hoverOffset: 4,
        },
      ],
    };

    this.overallBalanceOptions = {
      maintainAspectRatio: false,
      responsive: true,
      plugins: {
        legend: {
          display: true,
          position: 'bottom',
          align: 'center',
          labels: {
            textAlign: 'center',
          },
        },
        tooltip: {
          callbacks: {
            label: function (context: any) {
              let percentageValue = ((context.parsed / total) * 100).toFixed(2);
              return percentageValue + '%';
            },
          },
        },
      },
    };
  }

  async populateLoansDebtsChart(): Promise<void> {
    try {
      const response = await lastValueFrom(
        this.debtsService.getAllLoansAndDebts()
      );

      this.totalLoansAndDebts = response.payload;
      this.createLoansDebtsChart(this.totalLoansAndDebts);
    } catch (error) {
      this.notificationService.showError('Something went wrong!');
    }

    this.loansDebtsStats = {
      totalLoans: `${
        this.getCurrency() === 'RON'
          ? parseFloat(this.totalLoansAndDebts.totalLoans).toFixed(2) +
            ' ' +
            this.getCurrency()
          : this.getCurrency() +
            parseFloat(this.totalLoansAndDebts.totalLoans).toFixed(2)
      }`,

      totalDebts: `${
        this.getCurrency() === 'RON'
          ? '-' +
            parseFloat(this.totalLoansAndDebts.totalDebts).toFixed(2) +
            ' ' +
            this.getCurrency()
          : this.getCurrency() +
            '-' +
            parseFloat(this.totalLoansAndDebts.totalDebts).toFixed(2)
      }`,
    };
  }

  createLoansDebtsChart(data: any): void {
    let total = parseFloat(data.totalLoans) + parseFloat(data.totalDebts);
    this.loansDebtsData = {
      // values on X-Axis
      labels: ['Loans', 'Debts'],
      datasets: [
        {
          label: 'Loans & Debts',
          data: [data.totalLoans, data.totalDebts],
          backgroundColor: ['#DC143C', '#3b444b'],
          hoverOffset: 4,
        },
      ],
    };

    this.loansDebtsOptions = {
      maintainAspectRatio: false,
      responsive: true,
      plugins: {
        legend: {
          display: true,
          position: 'bottom',
          align: 'center',
          labels: {
            textAlign: 'center',
          },
        },
        tooltip: {
          callbacks: {
            label: function (context: any) {
              let percentageValue = ((context.parsed / total) * 100).toFixed(2);
              return percentageValue + '%';
            },
          },
        },
      },
    };
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
