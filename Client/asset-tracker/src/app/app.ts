import { Component, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Navbar } from './component/navbar/navbar';
import { Footer } from './component/footer/footer';
import { Router, NavigationEnd } from '@angular/router';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, Navbar, Footer],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  isLandingPage = true;

   constructor(private router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLandingPage = event.url === '/' || event.url === '/home';
      }
    });
  }
}
