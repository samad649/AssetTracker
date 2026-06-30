import { Routes } from '@angular/router';
import { Home } from './component/home/home';
import { Plaid } from './component/plaid/plaid';
import { Login } from './component/login/login';
import { Register } from './component/register/register';
import { AccountCard } from './component/account-card/account-card';
import { TransactionCard } from './component/transaction-card/transaction-card';
import { AuthGuard } from './guards/authGuard';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'plaid', component: Plaid, canActivate: [AuthGuard]},
  { path: 'accounts', component: AccountCard, canActivate: [AuthGuard] },
  { path: 'transactions', component: TransactionCard, canActivate: [AuthGuard] },
  { path: 'login', component: Login },
  { path: 'register', component: Register }
];
