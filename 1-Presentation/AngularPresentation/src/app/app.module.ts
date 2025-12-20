import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { loginComponent } from './Login/login.component';
import { InputTextModule } from 'primeng/inputtext';
import { LoginService } from './Login/login.service';
import { AuthService } from '../services/auth.service';
import { AuthInterceptor } from './auth.interceptor';
import { UserComponent } from './User/user.component';
import { UserService } from './User/user.service';
import { AuthFailureInterceptor } from './auth-failure.interceptor';
import { StatusBarComponent } from './status-bar/status-bar.component';
import { StatusMessageService } from '../services/statusmessage.service';
import { NavMenuService } from './nav-menu/nav-menu.service';
import { UserRegisterComponent } from './user-register/user-register.component';
import { UserRegisterService } from './user-register/user-register.service';
import { QueryStringService } from '../services/shared.QueryString.service';

import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

@NgModule({ declarations: [
        AppComponent,
        NavMenuComponent,
        loginComponent,
        UserComponent,
        StatusBarComponent,
        UserRegisterComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        FormsModule,
        InputTextModule,
        ConfirmDialogModule,
        RouterModule.forRoot([
            { path: '', component: loginComponent, pathMatch: 'full' },
            { path: 'login', component: loginComponent, pathMatch: 'full' },
            { path: 'user', component: UserComponent, pathMatch: 'full' },
            { path: 'register', component: UserRegisterComponent, pathMatch: 'full' }
        ])], providers: [
        { provide: "BASE_URL", useValue: document.getElementsByTagName('base')[0].href },
        { provide: LoginService },
        { provide: AuthService },
        { provide: UserService },
        { provide: StatusMessageService },
        { provide: NavMenuService },
        { provide: UserRegisterService },
        { provide: QueryStringService },
        { provide: ConfirmationService },
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: AuthFailureInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule { }
