import { Routes } from '@angular/router';
import { Home } from './component/home/home';
import { Plaid } from './component/plaid/plaid';
import { Profile } from './component/profile/profile';
import { AccountCard } from './component/account-card/account-card';
import { TransactionCard } from './component/transaction-card/transaction-card';
import { AuthGuard } from './guards/authGuard';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'plaid', component: Plaid },
  { path: 'accounts', component: AccountCard, canActivate: [AuthGuard] },
  { path: 'transactions', component: TransactionCard, canActivate: [AuthGuard] },
  { path: 'profile', component: Profile },
];
