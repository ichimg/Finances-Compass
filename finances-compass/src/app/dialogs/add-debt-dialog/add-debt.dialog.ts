import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_DATE_FORMAT } from 'src/app/configurations/my-date-format';
import { UserFriend } from 'src/app/entities/user-friend.model';
import { NotificationService } from 'src/app/services/notification.service';
import { DebtsService } from 'src/app/services/debts.service';
import { Debt } from 'src/app/entities/debt';


@Component({
  selector: 'app-add-debt',
  templateUrl: './add-debt.dialog.html',
  styleUrls: ['./add-debt.dialog.css'],
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
export class AddDebtDialog implements OnInit {
  @ViewChild('endDatePicker') endDatePicker: any;
  minDeadlineDate!: Date | null;

  userDebtOptions = [{ name: 'A friend' }, { name: 'Non user' }];

  isVisible!: number;
  selectedUserEmail!: string;
  userFriends!: UserFriend[];
  debts!: Debt[];

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  maxDate = new Date();

  constructor(
    public dialogRef: MatDialogRef<AddDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private debtsService: DebtsService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.userFriends = this.data.userFriends;
    this.debts = this.data.debts;
  }

  addDebtForm = new FormGroup({
    amount: new FormControl('', Validators.required),
    reason: new FormControl('', Validators.required),
    borrowingDate: new FormControl('', Validators.required),
    deadlineDate: new FormControl('', Validators.required),
    email: this.emailFormControl,
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    userSelect: new FormControl('', Validators.required),
  });

  closeDialog(): void {
    this.dialogRef.close(this.debts);
  }

  onItemChange(itemIndex: number) {
    this.isVisible = itemIndex;
    if (this.isVisible != 1) {
      this.addDebtForm.get('userSelect')!.setValidators(Validators.required);
      this.addDebtForm.get('userSelect')!.updateValueAndValidity();

      this.addDebtForm.get('email')!.clearValidators();
      this.addDebtForm.get('email')!.updateValueAndValidity();
      this.addDebtForm.get('firstName')!.clearValidators();
      this.addDebtForm.get('firstName')!.updateValueAndValidity();
      this.addDebtForm.get('lastName')!.clearValidators();
      this.addDebtForm.get('lastName')!.updateValueAndValidity();
    } else {
      this.addDebtForm.get('userSelect')!.clearValidators();
      this.addDebtForm.get('userSelect')!.updateValueAndValidity();

      this.addDebtForm
        .get('email')!
        .setValidators([Validators.required, Validators.email]);
      this.addDebtForm.get('email')!.updateValueAndValidity();
      this.addDebtForm.get('firstName')!.setValidators(Validators.required);
      this.addDebtForm.get('firstName')!.updateValueAndValidity();
      this.addDebtForm.get('lastName')!.setValidators(Validators.required);
      this.addDebtForm.get('lastName')!.updateValueAndValidity();
    }
  }

  onBorrowingDateSelected(selectedDate: Date): void {
    this.minDeadlineDate = selectedDate;
  }

  onSubmit(): void {
    if (this.isVisible == 0) {
      // Has account
      const createUserDebtRequest: Debt = Object.assign({
        firstName: '',
        lastName: '',
        email: this.addDebtForm.value.userSelect,
        amount: this.addDebtForm.value.amount,
        borrowingDate:  this.addDebtForm.value.borrowingDate,
        deadline:  this.addDebtForm.value.deadlineDate,
        reason: this.addDebtForm.value.reason,
        status: 'Pending',
        isPaid: false,
        isUserAccount: true,
      });
      console.log(`emailu: ${this.addDebtForm.value.userSelect}`);
      console.log(createUserDebtRequest);
      this.debtsService
        .createDebt(createUserDebtRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 201:
              this.debts.push(createUserDebtRequest);
              this.notificationService.showSuccess('Debt added successfully!');
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    } else if (this.isVisible == 1){
      // Non user
      const createNonUserDebtRequest: Debt = Object.assign({
        firstName: this.addDebtForm.value.firstName,
        lastName: this.addDebtForm.value.lastName,
        email: this.addDebtForm.value.email,
        amount: this.addDebtForm.value.amount,
        borrowingDate: this.addDebtForm.value.borrowingDate,
        deadline: this.addDebtForm.value.deadlineDate,
        reason: this.addDebtForm.value.reason,
        status: 'Pending',
        isPaid: false,
        isUserAccount: false,
      });

      this.debtsService
      .createDebt(createNonUserDebtRequest)
      .subscribe((response) => {
        switch (response.statusCode) {
          case 201:
            this.debts.push(createNonUserDebtRequest);
            this.notificationService.showSuccess('Debt added successfully!');
            this.closeDialog();
            break;

          default:
            this.notificationService.showError('Something went wrong');
            break;
        }
      });
    }
  }
}
