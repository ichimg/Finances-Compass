import { UsersService } from 'src/app/services/users.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
import { UserModel } from 'src/app/entities/user-friend.model';
import { NotificationService } from 'src/app/services/notification.service';
import { DebtsService } from 'src/app/services/debts.service';
import { Debt } from 'src/app/entities/debt';
import { PaginationService } from 'src/app/services/pagination.service';

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
    PaginationService,
  ],
})
export class AddDebtDialog implements OnInit {
  @ViewChild('endDatePicker') endDatePicker: any;
  minDeadlineDate!: Date | null;

  userDebtOptions = [{ name: 'A friend' }, { name: 'Non user' }];

  isVisible: number = -1;
  userFriends!: UserModel[];
  userFriendsTotalCount!: number;
  debts!: Debt[];
  selectedUser!: string;

  isDebtEdited: boolean = false;

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.maxLength(320),
  ]);
  maxDate = new Date();

  constructor(
    public dialogRef: MatDialogRef<AddDebtDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private debtsService: DebtsService,
    private usersService: UsersService,
    private paginationService: PaginationService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.debts = this.data.debts;

    this.usersService
      .getAllFriends(
        this.paginationService.pageNumber,
        this.paginationService.pageSize
      )
      .subscribe((response) => {
        this.userFriends = response.body.payload;
        this.userFriendsTotalCount = JSON.parse(
          response.headers.get('X-Pagination')!
        ).TotalCount;

        if (this.data.selectedDebt !== undefined) {
          this.fillEditModalForm();
        }
      });
  }

  debtForm = new FormGroup({
    amount: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    reason: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
    ]),
    borrowingDate: new FormControl('', Validators.required),
    deadlineDate: new FormControl('', Validators.required),
    email: this.emailFormControl,
    firstName: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    lastName: new FormControl('', [
      Validators.required,
      Validators.maxLength(20),
    ]),
    userSelect: new FormControl('', Validators.required),
    userType: new FormControl(''),
  });

  closeDialog(): void {
    this.dialogRef.close(this.debts);
  }

  fillEditModalForm() {
    this.debtForm.controls['amount'].setValue(this.data.selectedDebt.amount);
    this.debtForm.controls['reason'].setValue(this.data.selectedDebt.reason);
    this.debtForm.controls['borrowingDate'].setValue(
      this.data.selectedDebt.borrowingDate
    );
    this.debtForm.controls['deadlineDate'].setValue(
      this.data.selectedDebt.deadline
    );

    if (this.data.selectedDebt.isUserAccount) {
      this.debtForm.controls['userType'].setValue(
        this.userDebtOptions[0]['name']
      );
      this.debtForm.controls['userSelect'].setValue(
        this.data.selectedDebt.email
      );
      this.isVisible = 0;
    } else {
      this.debtForm.controls['userType'].setValue(
        this.userDebtOptions[1]['name']
      );
      this.debtForm.controls['email'].setValue(this.data.selectedDebt.email);
      this.debtForm.controls['firstName'].setValue(
        this.data.selectedDebt.firstName
      );
      this.debtForm.controls['lastName'].setValue(
        this.data.selectedDebt.lastName
      );
      this.isVisible = 1;
    }

    this.onItemChange(this.isVisible);
    this.listenOnFormChanges();
  }

  listenOnFormChanges() {
    this.debtForm.controls['amount'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
    this.debtForm.controls['reason'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
    this.debtForm.controls['borrowingDate'].valueChanges.subscribe(
      (newValue) => {
        this.isDebtEdited = true;
      }
    );
    this.debtForm.controls['deadlineDate'].valueChanges.subscribe(
      (newValue) => {
        this.isDebtEdited = true;
      }
    );
    this.debtForm.controls['email'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
    this.debtForm.controls['firstName'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
    this.debtForm.controls['lastName'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
    this.debtForm.controls['userSelect'].valueChanges.subscribe((newValue) => {
      this.isDebtEdited = true;
    });
  }

  onItemChange(itemIndex: number) {
    this.isVisible = itemIndex;
    if (this.isVisible !== 1) {
      this.debtForm.get('userSelect')!.setValidators(Validators.required);
      this.debtForm.get('userSelect')!.updateValueAndValidity();

      this.debtForm.get('email')!.clearValidators();
      this.debtForm.get('email')!.updateValueAndValidity();
      this.debtForm.get('firstName')!.clearValidators();
      this.debtForm.get('firstName')!.updateValueAndValidity();
      this.debtForm.get('lastName')!.clearValidators();
      this.debtForm.get('lastName')!.updateValueAndValidity();
    } else {
      this.debtForm.get('userSelect')!.clearValidators();
      this.debtForm.get('userSelect')!.updateValueAndValidity();

      this.debtForm
        .get('email')!
        .setValidators([Validators.required, Validators.email]);
      this.debtForm.get('email')!.updateValueAndValidity();
      this.debtForm
        .get('firstName')!
        .setValidators([Validators.required, Validators.maxLength(20)]);
      this.debtForm.get('firstName')!.updateValueAndValidity();
      this.debtForm
        .get('lastName')!
        .setValidators([Validators.required, Validators.maxLength(20)]);
      this.debtForm.get('lastName')!.updateValueAndValidity();
    }
  }

  onBorrowingDateSelected(selectedDate: Date): void {
    this.minDeadlineDate = selectedDate;
  }

  editDebt(): void {
    if (this.isVisible == 0) {
      // Has account
      let selectedUserFriend = this.userFriends.find(u => u.email === this.selectedUser)!;

      const editUserDebtRequest: Debt = Object.assign({
        guid: this.data.selectedDebt.guid,
        firstName: selectedUserFriend.firstName,
        lastName: selectedUserFriend.lastName,
        email: selectedUserFriend.email,
        amount: this.debtForm.value.amount,
        borrowingDate: this.debtForm.value.borrowingDate,
        deadline: this.debtForm.value.deadlineDate,
        reason: this.debtForm.value.reason,
        isUserAccount: true,
      });

      this.debtsService
        .updateDebt(editUserDebtRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 200:
              editUserDebtRequest.status = this.data.selectedDebt.status;
              editUserDebtRequest.isPaid = this.data.selectedDebt.isPaid;

              const index = this.data.debts.indexOf(this.data.selectedDebt);
              this.data.debts.splice(index, 1);
              this.debts.push(editUserDebtRequest);
              this.notificationService.showSuccess(
                'Debt updated successfully!'
              );
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    } else if (this.isVisible == 1) {
      // Non user
      const editNonUserDebtRequest: Debt = Object.assign({
        guid: this.data.selectedDebt.guid,
        firstName: this.debtForm.value.firstName,
        lastName: this.debtForm.value.lastName,
        email: this.debtForm.value.email,
        amount: this.debtForm.value.amount,
        borrowingDate: this.debtForm.value.borrowingDate,
        deadline: this.debtForm.value.deadlineDate,
        reason: this.debtForm.value.reason,
        isUserAccount: false,
      });

      this.debtsService
        .updateDebt(editNonUserDebtRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 200:
              editNonUserDebtRequest.status = this.data.selectedDebt.status;
              editNonUserDebtRequest.isPaid = this.data.selectedDebt.isPaid;

              const index = this.data.debts.indexOf(this.data.selectedDebt);
              this.data.debts.splice(index, 1);
              this.debts.push(editNonUserDebtRequest);
              this.notificationService.showSuccess(
                'Debt updated successfully!'
              );
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    }
  }

  createDebt(): void {
    if (this.isVisible == 0) {
      // Has account
      let selectedUserFriend = this.userFriends.find(u => u.email === this.selectedUser)!;

      const createUserDebtRequest: Debt = Object.assign({
        firstName: selectedUserFriend.firstName,
        lastName: selectedUserFriend.lastName,
        email: selectedUserFriend.email,
        amount: this.debtForm.value.amount,
        borrowingDate: this.debtForm.value.borrowingDate,
        deadline: this.debtForm.value.deadlineDate,
        reason: this.debtForm.value.reason,
        status: 'Pending',
        isPaid: false,
        isUserAccount: true,
      });
      this.debtsService
        .createDebt(createUserDebtRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 201:
              createUserDebtRequest.guid = response.payload;
              this.debts.push(createUserDebtRequest);
              this.notificationService.showSuccess('Debt added successfully!');
              this.closeDialog();
              break;

            default:
              this.notificationService.showError('Something went wrong');
              break;
          }
        });
    } else if (this.isVisible == 1) {
      // Non user
      const createNonUserDebtRequest: Debt = Object.assign({
        firstName: this.debtForm.value.firstName,
        lastName: this.debtForm.value.lastName,
        email: this.debtForm.value.email,
        amount: this.debtForm.value.amount,
        borrowingDate: this.debtForm.value.borrowingDate,
        deadline: this.debtForm.value.deadlineDate,
        reason: this.debtForm.value.reason,
        status: 'Pending',
        isPaid: false,
        isUserAccount: false,
      });

      this.debtsService
        .createDebt(createNonUserDebtRequest)
        .subscribe((response) => {
          switch (response.statusCode) {
            case 201:
              createNonUserDebtRequest.guid = response.payload;
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

  onSubmit(): void {
    if (this.data.selectedDebt !== undefined) {
      this.editDebt();
    } else {
      console.log('face asta');
      this.createDebt();
    }
  }

  loadMoreFriends(): void {
    if (this.userFriends.length < this.paginationService.totalCount) {
      this.paginationService.change(this.userFriendsTotalCount);

      this.usersService
        .getAllFriends(
          this.paginationService.pageNumber,
          this.paginationService.pageSize
        )
        .subscribe((response) => {
          this.userFriends = this.userFriends.concat(response.body.payload);
          this.userFriendsTotalCount = JSON.parse(
            response.headers.get('X-Pagination')!
          ).TotalCount;
        });
    }
  }
}
