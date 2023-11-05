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

  password: string | null = "";

  isLoginFaild: boolean = false;
  loginMessage: string = "";

  constructor(private loginService: LoginService, private route: ActivatedRoute, private router: Router, private authService: AuthService) {
    this.loginRequest.returnurl = this.route.snapshot.queryParamMap.get("returnUrl");
  }

  OnSubmit(form: NgForm) {
    this.loginMessage = "";

    if (this.password == "" || this.password == null) {
      return;
    }

    this.loginRequest.password = btoa(this.password + "|" + Date.now());
    this.loginService.Authenticate(this.loginRequest)
      .subscribe(
        response => {
          this.authService.JwtToken = response.jwtToken;
          if (response.returnUrl != undefined) {
            window.location.href = response.returnUrl + "?jwtToken=" + response.jwtToken;
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
