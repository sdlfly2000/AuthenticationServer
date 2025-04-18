import { Component } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { StatusMessageService } from '../../services/statusmessage.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  displayName: string | null;

  isLoginStatus: boolean = false;

  constructor(
    private navMenuService: NavMenuService,
    private authService: AuthService,
    private statusMessageService: StatusMessageService,
    private router: Router) {

    this.authService.OnUserDisplayName.subscribe(name => this.displayName = name);
    this.displayName = this.authService.UserDisplayName

    this.authService.OnLoginSuccess.subscribe(() => {
      this.isLoginStatus = true;
    });

    this.authService.OnLoginFailure.subscribe(() => {
      this.isLoginStatus = false;
    })

    this.authService.CheckLoginStatus();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  Logout() {
    this.navMenuService.logout().subscribe(() =>
    {
      this.authService.CleanLocalCache();
      this.authService.LoginStatus = false;
      this.statusMessageService.StatusMessage = "";
      this.router.navigate(["/"]);
    });
  }
}
