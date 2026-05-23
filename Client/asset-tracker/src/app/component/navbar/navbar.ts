import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { authService } from '../../services/authService';
@Component({
  standalone: true,
  selector: 'app-navbar',
  imports: [CommonModule, RouterModule, NzIconModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar {

    constructor(public authService: authService) {
    }


}
