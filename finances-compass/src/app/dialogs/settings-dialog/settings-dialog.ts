import { lastValueFrom } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../services/notification.service';
import { UsersService } from '../../services/users.service';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-settings-dialog',
  templateUrl: './settings-dialog.html',
  styleUrls: ['./settings-dialog.css'],
})
export class SettingsDialog implements OnInit {
  selectedCurrency!: string;
  isCurrencyChanged: boolean = false;
  currencies: string[] = ['RON', 'EUR', 'USD'];

  constructor(
    public dialogRef: MatDialogRef<SettingsDialog>,
    private notificationService: NotificationService,
    private usersService: UsersService,
    private authService: AuthenticationService
  ) {}

  async ngOnInit(): Promise<void> {
    try {
      let email: string = this.authService.getEmailFromToken();
      const response = await lastValueFrom(
        this.usersService.getCurrencyPreference(email)
      );

      if (response.statusCode === 200) {
        this.selectedCurrency = response.payload;
      } else {
        this.notificationService.showError('Something went wrong!');
      }
    } catch (error) {
      this.notificationService.showError('Something went wrong!');
    }
  }

  onCurrencyChange(newValue: string) {
    this.isCurrencyChanged = true;
  }

  async onSaveClick(): Promise<void> {
    if (!this.isCurrencyChanged) {
      this.closeDialog();
      return;
    }

    let email: string = this.authService.getEmailFromToken();

    try {
      const response = await this.usersService.changeCurrencyPreference(
        email,
        this.selectedCurrency
      );

      if (response.statusCode === 200) {
        localStorage.setItem('currencyPreference', this.selectedCurrency);

        this.notificationService.showSuccess('Settings saved');
        this.closeDialog();
        window.location.reload();
      } else {
        this.notificationService.showError('Something went wrong!');
      }
    } catch (error) {
      this.notificationService.showError('Something went wrong!');
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
