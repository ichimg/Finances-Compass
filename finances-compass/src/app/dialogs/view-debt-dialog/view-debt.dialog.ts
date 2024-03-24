import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import { Debt } from 'src/app/entities/debt';
import { DebtsService } from 'src/app/services/debts.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-view-debt',
  templateUrl: './view-debt.dialog.html',
  styleUrls: ['./view-debt.dialog.css'],
})
export class ViewDebtDialog implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<ViewDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public debtsService: DebtsService,
    public notificationService: NotificationService
  ) {}

  ngOnInit(): void {}
  closeDialog(): void {
    this.dialogRef.close();
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

  async markDebtPaid(): Promise<void> {
    const response = await lastValueFrom(
      this.debtsService.markDebtPaid(this.data.debt.guid)
    );

    switch (response.statusCode) {
      case 200:
        this.data.debt.isPaid = true;
        break;

      case 404:
        this.notificationService.showError('Debt to mark not found');
        break;

      default:
        this.notificationService.showError('Something went wrong');
        break;
    }
  }

  isDeadlinePassed(debt: Debt): boolean {
    return !debt.isPaid && new Date(debt.deadline) < new Date();
  }
}
