import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ViewDebtsComponent} from './components/view-debts/view-debts.component'
import {LoginComponent} from './components/login/login.component'
import { authGuard } from './guards/auth.guard';
import { unauthGuard } from './guards/unauth.guard';
import { RegisterComponent } from './components/register/register.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';


const routes: Routes = [
  { path: 'dashboard', component: ViewDebtsComponent, canActivate: [authGuard]},
  { path: '', component: LoginComponent, canActivate:[unauthGuard] },
  {path: 'register', component: RegisterComponent, canActivate: [unauthGuard]},
  { path: 'emailconfirmation', component: EmailConfirmationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
