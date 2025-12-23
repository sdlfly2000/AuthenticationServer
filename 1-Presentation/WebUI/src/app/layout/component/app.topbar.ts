import { Component } from '@angular/core';
import { RouterModule, Router} from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '../service/layout.service';
import { AppTopBarService } from '../service/app.topbar.service';
import { AuthService } from '../../../services/auth.service';
import { StatusMessageService } from '../../../services/statusmessage.service';

@Component({
    selector: 'app-topbar',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule],
    template: `<div class="layout-topbar">
        <div class="layout-topbar-logo-container">
            <button class="layout-menu-button layout-topbar-action" (click)="layoutService.onMenuToggle()">
                <i class="pi pi-bars"></i>
            </button>
            <a class="layout-topbar-logo" routerLink="/">
                <span>Auth Service</span>
            </a>
        </div>

        <div class="layout-topbar-actions">
           
            <button class="layout-topbar-menu-button layout-topbar-action" pStyleClass="@next" enterFromClass="hidden" enterActiveClass="animate-scalein" leaveToClass="hidden" leaveActiveClass="animate-fadeout" [hideOnOutsideClick]="true">
                <i class="pi pi-ellipsis-v"></i>
            </button>

            <div class="layout-topbar-menu hidden lg:block">
                <div class="layout-topbar-menu-content">
                    <button type="button" class="layout-topbar-action">
                        <i class="pi pi-calendar"></i>
                        <span>Calendar</span>
                    </button>
                    <button type="button" class="layout-topbar-action">
                        <i class="pi pi-inbox"></i>
                        <span>Messages</span>
                    </button>
                    <button type="button" class="layout-topbar-action">
                        <i class="pi pi-user"></i>
                        <span>Profile</span>
                    </button>
                </div>
            </div>
        </div>
    </div>`
})
export class AppTopbar {
    displayName: string | null = null;

    isLoginStatus: boolean = false;
    
    constructor(
        public layoutService: LayoutService, 
        private appTopBarService: AppTopBarService,
        private authService: AuthService,
        private statusMessageService: StatusMessageService,
        private router: Router
    ) {
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

    toggleDarkMode() {
        this.layoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }
    
    Logout() {
        this.appTopBarService.logout().subscribe(() =>
        {
            this.authService.CleanLocalCache();
            this.authService.LoginStatus = false;
            this.statusMessageService.StatusMessage = "";
            this.router.navigate(["/"]);
        });
    }
}
