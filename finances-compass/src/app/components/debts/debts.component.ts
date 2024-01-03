import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DebtsService } from '../../services/debts.service';
import { MatDialog } from '@angular/material/dialog';
import { AddDebtDialog } from 'src/app/dialogs/add-debt-dialog/add-debt.dialog';
import { Debt } from 'src/app/entities/debt';
import { ViewDebtDialog } from 'src/app/dialogs/view-debt-dialog/view-debt.dialog';
import { NotificationService } from '../../services/notification.service';
import { DeleteConfirmationDialog } from '../../dialogs/delete-confirmation-dialog/delete-confirmation.dialog';

@Component({
  selector: 'app-debts',
  templateUrl: './debts.component.html',
  styleUrls: ['./debts.component.css'],
})
export class DebtsComponent implements OnInit, AfterViewInit {
  displayedReceivingColumns: string[] = [
    'name',
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
    private notificationService: NotificationService
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

  ngAfterViewInit() {}

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
      .open(AddDebtDialog, {
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

  onRowClick(debt: Debt) {
    const dialogRef = this.dialog.open(ViewDebtDialog, {
      width: '600px',
      data: { debt },
    });
  }

  openEditDialog(event: Event, debt: Debt): void {
    event.stopPropagation();
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
}
