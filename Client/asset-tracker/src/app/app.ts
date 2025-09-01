import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PlaidLinkComponent } from './components/plaid-link/plaid-link.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, PlaidLinkComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('asset-tracker');
}
