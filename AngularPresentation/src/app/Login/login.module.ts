import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { loginComponent } from './login.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { LoginService } from './login.service';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';

@NgModule({
  declarations: [
    loginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    InputTextModule,
    ButtonModule
  ],
  providers:
    [
      { provide: "BASE_URL", useValue: document.getElementsByTagName('base')[0].href },
      { provide: LoginService },
    ],
  bootstrap: [loginComponent]
})
export class LoginModule { }
