import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsService } from '../../services/debts.service';
import { MatDialog } from '@angular/material/dialog';
import { Debt } from 'src/app/entities/debt';
import { ViewDebtDialog } from 'src/app/dialogs/view-debt-dialog/view-debt.dialog';
import { NotificationService } from '../../services/notification.service';
import { DeleteConfirmationDialog } from '../../dialogs/delete-confirmation-dialog/delete-confirmation.dialog';
import { PaypalService } from '../../services/paypal.service';
import { AddOrEditDebtDialog } from '../../dialogs/add-or-edit-debt-dialog/add-or-edit-debt.dialog';
import { PaymentDialog } from '../../dialogs/payment-dialog/payment.dialog';

@Component({
  selector: 'app-debts',
  templateUrl: './debts.component.html',
  styleUrls: ['./debts.component.css'],
  providers: [PaypalService],
})
export class DebtsComponent implements OnInit {
  displayedReceivingColumns: string[] = [
    'name',
    'amount',
    'deadline',
    'reason',
    'status',
    'action',
  ];

  displayedUserColumns: string[] = [
    'amount',
    'deadline',
    'reason',
    'status',
    'action',
  ];
  dataReceivingDebtsSource = new MatTableDataSource();
  dataUserDebtsSource = new MatTableDataSource();

  isReceivingDebtsLoaded: boolean = false;
  isUserDebtsLoaded: boolean = false;

  constructor(
    private liveAnnouncer: LiveAnnouncer,
    private debtsService: DebtsService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
  ) {}
  ngOnInit(): void {
    this.debtsService.getAllReceivingDebts().subscribe((response) => {
      this.dataReceivingDebtsSource.data = response.payload.sort((a, b) => {
        return new Date(b.deadline).getTime() - new Date(a.deadline).getTime();
      });
      this.isReceivingDebtsLoaded = true;
    });

    this.debtsService.getAllUserDebts().subscribe(
      (response) => {
        this.dataUserDebtsSource.data = response.payload;
        this.isUserDebtsLoaded = true;
      },
      () => {
        this.notificationService.showError('Something went wrong');
      }
    );
  }

  @ViewChild('debtReceivingTbSort') set debtReceivingTbSort(sort: MatSort) {
    this.dataReceivingDebtsSource.sort = sort;
  }
  @ViewChild('debtUserTbSort') set debtUserTbSort(sort: MatSort) {
    this.dataUserDebtsSource.sort = sort;
  }

  announceReceivingSortChange(sortState: Sort) {
    console.log(sortState.direction);
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction} ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }

  announceUserSortChange(sortState: Sort) {
    if (sortState.direction) {
      this.liveAnnouncer.announce(`Sorted ${sortState.direction} ending`);
    } else {
      this.liveAnnouncer.announce('Sorting cleared');
    }
  }

  areDebtsToReceive(): boolean {
    return this.dataReceivingDebtsSource.data.length > 0;
  }

  areDebts(): boolean {
    return this.dataUserDebtsSource.data.length > 0;
  }

  addDebt(): void {
    const dialogRef = this.dialog
      .open(AddOrEditDebtDialog, {
        data: {
          debts: this.dataReceivingDebtsSource.data,
        },
      })
      .afterClosed()
      .subscribe((response) => {
        if (response) {
          this.dataReceivingDebtsSource.data = response;
        }
      });
  }

  onLoanClick(debt: Debt) {
    const dialogRef = this.dialog.open(ViewDebtDialog, {
      width: '600px',
      data: { debt: debt, isUserDebt: false },
    });
  }

  onDebtClick(debt: Debt) {
    const dialogRef = this.dialog.open(ViewDebtDialog, {
      width: '600px',
      data: { debt: debt, isUserDebt: true },
    });
  }

  openEditDialog(event: Event, debt: Debt): void {
    event.stopPropagation();

    const dialogRef = this.dialog
      .open(AddOrEditDebtDialog, {
        data: {
          debts: this.dataReceivingDebtsSource.data,
          selectedDebt: debt,
        },
      })
      .afterClosed()
      .subscribe((response) => {
        if (response) {
          this.dataReceivingDebtsSource.data = response;
        }
      });
  }

  deleteDebt(event: Event, debt: Debt): void {
    event.stopPropagation();

    const dialogRef = this.dialog
      .open(DeleteConfirmationDialog, {
        width: '600px',
        data: { debt: debt, debtsList: this.dataReceivingDebtsSource.data },
      })
      .afterClosed()
      .subscribe((response) => {
        if (response) {
          this.dataReceivingDebtsSource.data = response;
        }
      });
  }

  approveDebt(event: Event, debt: Debt): void {
    event.stopPropagation();

    this.debtsService.approveDebt(debt.guid).subscribe((response) => {
      switch (response.statusCode) {
        case 200:
          debt.status = 'Accepted';
          break;

        case 404:
          this.notificationService.showError('Debt to approve not found');
          break;

        case 403:
          this.notificationService.showError('Unauthorized');
          break;

        default:
          this.notificationService.showError('Something went wrong');
          break;
      }
    });
  }

  rejectDebt(event: Event, debt: Debt): void {
    event.stopPropagation();

    this.debtsService.rejectDebt(debt.guid).subscribe((response) => {
      switch (response.statusCode) {
        case 200:
          debt.status = 'Rejected';
          break;

        case 404:
          this.notificationService.showError('Debt to reject not found');
          break;

        case 403:
          this.notificationService.showError('Unauthorized');
          break;

        default:
          this.notificationService.showError('Something went wrong');
          break;
      }
    });
  }

  onPayClick(event: Event, debt: Debt) {
    event.stopPropagation();

    const dialogRef = this.dialog
    .open(PaymentDialog, {
      width: '600px',
      data: { debt: debt},
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
}