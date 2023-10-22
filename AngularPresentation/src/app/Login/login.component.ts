import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from './Models/LoginRequest';
import { LoginService } from './login.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './login.component.html',
  styleUrls: ['./lgoin.component.css']
})
export class loginComponent {
    title = 'Lgoin';

  loginRequest: LoginRequest = {
    username: "",
    password: "",
    returnurl: ""
  };

  constructor(private loginService: LoginService, private router: ActivatedRoute) {
    this.loginRequest.returnurl = this.router.snapshot.queryParamMap.get("returnUrl");
  }

  OnSubmit(form: NgForm) {
    this.loginService.Authenticate(this.loginRequest).subscribe(
      response =>
      {
        AuthService.JwtToken = response.jwtToken;
        if (response.returnurl != undefined) {
          window.location.href = response.returnurl + "?jwtToke=" + response.jwtToken;
        }
      });
  }

  GetUsers() {
    this.loginService.GetUsers().subscribe(result => console.log(result));
  }
}
