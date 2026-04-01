import { Routes } from '@angular/router';
import { Home } from './component/home/home';
import { Login } from './component/login/login';
import { Plaid } from './component/plaid/plaid';
import { Profile } from './component/profile/profile';
import { AccountCard } from './component/account-card/account-card';
import { TransactionCard } from './component/transaction-card/transaction-card';
import { Footer } from './component/footer/footer';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'plaid', component: Plaid },
  { path: 'accounts', component: AccountCard },
  { path: 'transactions', component: TransactionCard },
  { path: 'profile', component: Profile },
];
