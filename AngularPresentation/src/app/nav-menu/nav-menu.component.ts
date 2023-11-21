import { Component } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { AuthService } from '../auth.service';
import { StatusMessageService } from '../statusmessage.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  displayName: string | null;

  constructor(private navMenuService: NavMenuService, private authService: AuthService, private statusMessageService: StatusMessageService) {
    this.authService.OnUserDisplayName.subscribe(name => this.displayName = name);
    this.displayName = this.authService.UserDisplayName
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  Logout() {
    this.navMenuService.logout().subscribe(res =>
    {
      this.authService.CleanLocalCache();
      this.statusMessageService.StatusMessage = ""; 
    });
  }
}
