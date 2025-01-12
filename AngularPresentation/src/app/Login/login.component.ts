import { Component, OnInit } from '@angular/core';
import { LoginRequest } from './Models/LoginRequest';
import { LoginService } from './login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { StatusMessageService } from '../../services/statusmessage.service';
import { QueryStringService } from '../../services/shared.QueryString.service';
import { AuthService } from '../../services/auth.service';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './login.component.html',
  styleUrls: ['./lgoin.component.css']
})
export class loginComponent implements OnInit {
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
    private statusMessageService: StatusMessageService,
    private queryStringService: QueryStringService,
    private confirmationService: ConfirmationService
  ) {

  }

  ngOnInit(): void {

    const returnUrl = this.queryStringService.Get("returnUrl");
    this.loginRequest.returnurl = returnUrl;
    this.authService.SetReturnUrl(returnUrl);

    if (this.authService.UserId != null && this.authService.IsOutSideRequest == false) {
      this.router.navigateByUrl("user?userid=" + this.authService.UserId);
    }

    if (this.authService.IsValidLogin == true && this.authService.IsOutSideRequest == true) {
      this.CheckForwardCurrentLoginPopupDialog(this.authService.UserDisplayName!);
    }
  }


  CheckForwardCurrentLoginPopupDialog(userName: string) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed with current login user: ' + userName + ' ?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => window.location.href = this.authService.ReturnUrl + "?jwtToken=" + this.authService.JwtToken + "&userid=" + this.authService.UserId + "&userDisplayName=" + this.authService.UserDisplayName,
      reject: () => this.authService.CleanLocalCache()
    })
  }

  OnSubmit() {
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
        error: () => {
          this.authService.LoginStatus = false;
          this.isLoading = false;
          this.statusMessageService.StatusMessage = "Failed";
        }
    });
  }
}
