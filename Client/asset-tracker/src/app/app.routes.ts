import { Routes } from '@angular/router';
import { Home } from './component/home/home';
import { Login } from './component/login/login';
import { Plaid } from './component/plaid/plaid';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'Login', component: Login },
  { path: 'Plaid', component: Plaid }
];
