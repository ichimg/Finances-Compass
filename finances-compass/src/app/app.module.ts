import { DeleteConfirmationDialog } from './dialogs/delete-confirmation-dialog/delete-confirmation.dialog';
import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutComponent } from './components/layout/layout.component';
import { LoginComponent } from './components/login/login.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { HeaderComponent } from './components/header/header.component';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { MatInputModule } from '@angular/material/input';
import {
  MAT_FORM_FIELD_DEFAULT_OPTIONS,
  MatFormFieldModule,
} from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ToastrModule } from 'ngx-toastr';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';
import { JwtInterceptor } from './interceptor/interceptor';
import { RegisterComponent } from './components/register/register.component';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { NgFor, AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { EmailConfirmationDialog } from './dialogs/email-confirmation-dialog/email-confirmation.dialog';
import { AddOrEditDebtDialog } from './dialogs/add-or-edit-debt-dialog/add-or-edit-debt.dialog';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { DebtsComponent } from './components/debts/debts.component';
import { UsersService } from './services/users.service';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { StatusTransformPipe } from './pipes/status-transform.pipe';
import { ViewDebtDialog } from './dialogs/view-debt-dialog/view-debt.dialog';
import { SearchUsersDialog } from './dialogs/search-users/search-users.dialog';
import { InfiniteScrollDirective } from './directives/infinite-scroll.directive';
import { FooterComponent } from './components/footer/footer.component';
import { NgxPayPalModule } from 'ngx-paypal';
import { PaymentDialog } from './dialogs/payment-dialog/payment.dialog';
import { ExpensesComponent } from './components/expenses/expenses.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { AddExpenseOrIncomeDialog } from './dialogs/add-expense-or-income-dialog/add-expense-or-income-dialog';
import { TextFieldModule } from '@angular/cdk/text-field';
import { ViewOrEditExpenseDialog } from './dialogs/view-or-edit-expense-dialog/view-or-edit-expense-dialog';
import { NgChartsModule } from 'ng2-charts';
import { FriendsDialog } from './dialogs/friends-dialog/friends-dialog.dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { ScrollingModule } from '@angular/cdk/scrolling';

const OPTIONS = {
  appereance: 'outline',
  floatLabel: 'always',
  hideRequiredMarker: false,
};

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LayoutComponent,
    LoginComponent,
    RegisterComponent,
    EmailConfirmationComponent,
    EmailConfirmationDialog,
    AddOrEditDebtDialog,
    DebtsComponent,
    StatusTransformPipe,
    ViewDebtDialog,
    SearchUsersDialog,
    InfiniteScrollDirective,
    FooterComponent,
    DeleteConfirmationDialog,
    PaymentDialog,
    ExpensesComponent,
    AddExpenseOrIncomeDialog,
    ViewOrEditExpenseDialog,
    FriendsDialog
  ],
  imports: [
    BrowserModule,
    FormsModule,
    MatCardModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatButtonModule,
    HttpClientModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    RouterModule,
    MatSnackBarModule,
    ToastrModule.forRoot(),
    MatTableModule,
    MatSortModule,
    MatSelectModule,
    NgFor,
    AsyncPipe,
    MatAutocompleteModule,
    MatIconModule,
    MatTooltipModule,
    MatDialogModule,
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatMenuModule,
    MatSidenavModule,
    MatListModule,
    LoadingBarModule,
    LoadingBarHttpClientModule, 
    LoadingBarRouterModule,
    NgxPayPalModule,
    FullCalendarModule,
    TextFieldModule,
    NgChartsModule,
    MatTabsModule,
    ScrollingModule
  ],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { OPTIONS },
    },
    UsersService,
    Title
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
