import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from '../Models/LoginRequest';
import { LoginService } from './login.service';

@Component({
  selector: 'app-root',
  templateUrl: './login.component.html'
})
export class loginComponent {
  title = 'Lgoin';

  loginRequest: LoginRequest = {
    username: "",
    password: "",
    redirecturl: ""
  };

  constructor(private loginService: LoginService) {

  }

  OnSubmit(form: NgForm) {
    this.loginService.Authenticate(this.loginRequest).subscribe(response => window.location.href = response.redirecturl);
  }
}
