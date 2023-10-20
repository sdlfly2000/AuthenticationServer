import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from '../Models/LoginRequest';

@Component({
  selector: 'app-root',
  templateUrl: './login.component.html'
})
export class loginComponent {
  title = 'Lgoin';

  loginRequest: LoginRequest = {
    UserName: "",
    Password: ""
  };

  constructor() {

  }

  OnSubmit(form: NgForm) {

  }
}
