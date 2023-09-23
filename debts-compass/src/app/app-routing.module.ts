import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ViewDebtsComponent} from './view-debts/view-debts.component'
import {HeaderComponent} from './header/header.component'
import {LoginComponent} from './login/login.component'


const routes: Routes = [
  { path: '', component: ViewDebtsComponent },
  { path: 'login', component: LoginComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
