import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from './Models/LoginRequest';
import { LoginService } from './login.service';
import { ActivatedRoute, Router } from '@angular/router';
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

  isLoginFaild: boolean = false;
  loginMessage: string = "";

  constructor(private loginService: LoginService, private route: ActivatedRoute, private router: Router) {
    this.loginRequest.returnurl = this.route.snapshot.queryParamMap.get("returnUrl");
  }

  OnSubmit(form: NgForm) {
    this.loginMessage = "";
    this.loginService.Authenticate(this.loginRequest)
      .subscribe(
        response => {
          AuthService.JwtToken = response.jwtToken;
          if (response.returnUrl != undefined) {
            window.location.href = response.returnUrl + "?jwtToke=" + response.jwtToken;
          } else {
            this.router.navigateByUrl("user?id=" + response.userId);
          }
          this.isLoginFaild = false;
          this.loginMessage = "Successed";
        },
        error => {
          this.isLoginFaild = true;
          this.loginMessage = "Failed";
        }
    );
  }
}
