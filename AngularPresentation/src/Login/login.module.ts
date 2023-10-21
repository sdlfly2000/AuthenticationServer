import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { loginComponent } from './login.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { LoginService } from './login.service';

@NgModule({
  declarations: [
    loginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers:
    [
      { provide: "BASE_URL", useValue: document.getElementsByTagName('base')[0].href },
      { provide: "BACKEND_BASE_URL", useValue: "https://localhost:7008/" },
      { provide: LoginService },
    ],
  bootstrap: [loginComponent]
})
export class LoginModule { }
