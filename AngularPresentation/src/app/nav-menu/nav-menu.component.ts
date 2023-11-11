import { Component } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private navMenuService: NavMenuService, private authService: AuthService) {

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  Logout() {
    this.authService.RemoveLocalJwt();
    this.navMenuService.logout();
  }
}
