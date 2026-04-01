import { Component } from '@angular/core';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [NzDropDownModule, NzMenuModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class Profile {

}
