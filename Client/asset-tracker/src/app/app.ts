import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Navbar } from './component/navbar/navbar';
import { Footer } from './component/footer/footer';
import { Router, NavigationEnd } from '@angular/router';
import { SharedService } from './services/sharedService';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, Navbar, Footer],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  isLandingPage = true;



   constructor(private router: Router, private sharedService: SharedService) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLandingPage = event.url === '/' || event.url === '/home';
      }
    });
  }
  ngOnInit(): void {
  const storedUser = localStorage.getItem('user');
  if (storedUser) {
    console.log("Stored user found:", storedUser);
    this.sharedService.setSelectedUser(JSON.parse(storedUser));
  }
}
}
