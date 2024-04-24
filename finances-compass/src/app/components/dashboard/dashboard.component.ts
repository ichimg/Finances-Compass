import { Component, OnInit } from '@angular/core';
import Chart, { TooltipItem } from 'chart.js/auto';
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
  username!: string | null;

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

  // Annual Expenses
  public annualExpensesChart: any;
  annualExpensesOptions: any;
  annualExpensesData: any;
  months = [
    'Jan',
    'Feb',
    'Mar',
    'Apr',
    'May',
    'Jun',
    'Jul',
    'Aug',
    'Sep',
    'Oct',
    'Nov',
    'Dec',
  ];

  constructor(
    private expensesService: ExpensesService,
    private debtsService: DebtsService,
    private notificationService: NotificationService
  ) {}

  async ngOnInit(): Promise<void> {
    await this.populateOverallBalanceChart();
    await this.populateLoansDebtsChart();
    const tooltipPlugin = Chart.registry.getPlugin('tooltip') as any;

    tooltipPlugin.positioners.verticallyCenter = (
      elements: any,
      position: any
    ) => {
      if (!elements.length) {
        return tooltipPlugin.positioners.average(elements);
      }
      const { x, y, base, width } = elements[0].element;
      const height = (base - y) / 2;
      const offset = x + width / 2;
      return {
        x: offset,
        y: y + height,
      };
    };

    await this.populateAnnualExpensesChart();
    this.username = localStorage.getItem('userFirstName');
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
          backgroundColor: ['#3b444b', '#DC143C'],
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

  async populateAnnualExpensesChart(): Promise<void> {
    try {
      const response = await lastValueFrom(
        this.expensesService.getAnnualExpenses()
      );

      this.createAnnualExpensesChart(response.payload);
    } catch (error) {
      console.log(error);
      this.notificationService.showError('Something went wrong!');
    }
  }

  createAnnualExpensesChart(data: any) {
    let dataForFood = new Array(12).fill(0);
    let dataForClothes = new Array(12).fill(0);
    let dataForInvoices = new Array(12).fill(0);
    let dataForRent = new Array(12).fill(0);
    let dataForCar = new Array(12).fill(0);
    let dataForDebts = new Array(12).fill(0);

    data.forEach((d: any) => {
      let index = d.month - 1;
      switch (d.category) {
        case 'Food': {
          dataForFood[index] = d.totalAmount;
          break;
        }
        case 'Clothes': {
          dataForClothes[index] = d.totalAmount;
          break;
        }
        case 'Invoices': {
          dataForInvoices[index] = d.totalAmount;
          break;
        }
        case 'Rent': {
          dataForRent[index] = d.totalAmount;
          break;
        }
        case 'Car': {
          dataForCar[index] = d.totalAmount;
          break;
        }
        case 'Debts': {
          dataForDebts[index] = d.totalAmount;
          break;
        }
        default:
          break;
      }
    });

    this.annualExpensesData = {
      labels: this.months,
      datasets: [
        {
          maxBarThickness: 40,
          label: 'Food',
          data: dataForFood,
          backgroundColor: '#AC2C24',
        },
        {
          maxBarThickness: 40,
          label: 'Clothes',
          data: dataForClothes,
          backgroundColor: '#F9D615',
        },
        {
          maxBarThickness: 40,
          label: 'Invoices',
          data: dataForInvoices,
          backgroundColor: '#484848',
        },
        {
          maxBarThickness: 40,
          label: 'Rent',
          data: dataForRent,
          backgroundColor: '#62A704',
        },
        {
          maxBarThickness: 40,
          label: 'Car',
          data: dataForRent,
          backgroundColor: '#FFF5C1',
        },
        {
          maxBarThickness: 40,
          label: 'Debts',
          data: dataForDebts,
          backgroundColor: '#6A4C93',
        },
      ],
    };
    this.annualExpensesData.datasets = this.annualExpensesData.datasets.filter(
      (dataset: any) => !dataset.data.every((value: any) => value === 0)
    );

    this.annualExpensesOptions = {
      aspectRatio: 2,
      layout: {
        padding: {
          top: 0,
        },
      },
      responsive: true,
      maintainAspectRatio: true,
      scales: {
        y: {
          title: {
            display: false,
          },
          axis: 'y',
          grid: {
            display: false,
            drawTicks: false,
            tickLength: 0,
          },
          ticks: {
            major: {
              enabled: false,
            },
            padding: 17,
            stepSize: 25,
            callback: (value: any) => {
              return ` ${
                this.getCurrency() === 'RON'
                  ? parseFloat(value) + ' ' + this.getCurrency()
                  : this.getCurrency() + parseFloat(value)
              } `;
            },
          },
        },
        x: {
          title: {
            display: false,
          },
          axis: 'x',
          grid: { drawTicks: false },
          ticks: {
            padding: 17,
          },
        },
      },
      plugins: {
        tooltip: {
          position: 'verticallyCenter' as 'average',
          animation: { duration: 0 },
          callbacks: {
            title: (context: TooltipItem<'bar'>[]) => {
              return context[0].label;
            },
          },
        },
        legend: {
          display: true,
          position: 'bottom',
        },
        title: {
          display: false,
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

  private allZero(array: any) {
    return array.every((value: any) => value === 0);
  }
}
