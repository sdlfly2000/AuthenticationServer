import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { loginComponent } from './login.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

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
    [{
      provide: "BASE_API_URL", useValue: document.getElementsByTagName('base')[0].href
    }],
  bootstrap: [loginComponent]
})
export class LoginModule { }
