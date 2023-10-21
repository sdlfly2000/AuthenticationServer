import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { loginComponent } from './Login/login.component';
import { LoginModule } from './Login/login.module';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    LoginModule,
    RouterModule.forRoot([
      { path: '', component: loginComponent, pathMatch: 'full' }
    ]),
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
