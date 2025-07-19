import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';
import { unauthGuard } from './guards/unauth.guard';
import { RegisterComponent } from './components/register/register.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { DebtsComponent } from './components/debts/debts.component';
import { ExpensesComponent } from './components/expenses/expenses.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'debts', component: DebtsComponent },
      { path: 'expenses', component: ExpensesComponent },
      { path: 'dashboard', component: DashboardComponent }],
  },
  { path: 'login', component: LoginComponent, canActivate: [unauthGuard] },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [unauthGuard],
  },
  { path: 'emailconfirmation', component: EmailConfirmationComponent },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
