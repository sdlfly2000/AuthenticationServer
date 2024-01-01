import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoginRequest } from './Models/LoginRequest';
import { LoginService } from './login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { StatusMessageService } from '../statusmessage.service';

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

  loginMessage: string = "";

  isLoading: boolean = false;

  constructor(
    private loginService: LoginService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private statusMessageService: StatusMessageService) {
    this.loginRequest.returnurl = this.route.snapshot.queryParamMap.get("returnUrl");
    if (this.authService.UserId != null) {
      this.router.navigateByUrl("user?userid=" + this.authService.UserId);
    }
  }

  OnSubmit(form: NgForm) {
    if (this.password == "" || this.password == null) {
      return;
    }

    this.isLoading = true;
    this.statusMessageService.StatusMessage = "In Progress";

    this.loginRequest.password = btoa(this.password + "|" + Date.now());
    this.loginService.Authenticate(this.loginRequest)
      .subscribe({
        next: response => {
          this.authService.JwtToken = response.jwtToken;
          this.authService.UserId = response.userId;
          this.authService.UserDisplayName = response.userDisplayName;
          this.authService.LoginStatus = true;

          if (response.returnUrl != undefined) {
            window.location.href = response.returnUrl + "?jwtToken=" + response.jwtToken + "&userid=" + response.userId + "&userDisplayName=" + response.userDisplayName;
          } else {
            this.router.navigateByUrl("user?userid=" + response.userId);
          }
          this.isLoading = false;
          this.statusMessageService.StatusMessage = "Successed";
        }, 
        error: err => {
          this.authService.LoginStatus = false;
          this.isLoading = false;
          this.statusMessageService.StatusMessage = "Failed";
        }
    });
  }
}
