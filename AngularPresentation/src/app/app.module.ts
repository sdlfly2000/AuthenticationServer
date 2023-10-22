import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { loginComponent } from './Login/login.component';
import { InputTextModule } from 'primeng/inputtext';
import { LoginService } from './Login/login.service';
import { AuthService } from './auth.service';
import { AuthInterceptor } from './auth.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    loginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    InputTextModule,
    RouterModule.forRoot([
      { path: '', component: loginComponent, pathMatch: 'full' },
      { path: 'login', component: loginComponent, pathMatch: 'full' }
    ]),
  ],
  providers: [
    { provide: "BASE_URL", useValue: document.getElementsByTagName('base')[0].href },
    { provide: LoginService },
    { provide: AuthService },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
