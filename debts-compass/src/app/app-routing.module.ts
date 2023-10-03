import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ViewDebtsComponent} from './view-debts/view-debts.component'
import {HeaderComponent} from './header/header.component'
import {LoginComponent} from './login/login.component'
import { authGuard } from './guards/auth.guard';
import { unauthGuard } from './guards/unauth.guard';


const routes: Routes = [
  { path: '', component: ViewDebtsComponent, canActivate: [authGuard]},
  { path: 'login', component: LoginComponent, canActivate:[unauthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
