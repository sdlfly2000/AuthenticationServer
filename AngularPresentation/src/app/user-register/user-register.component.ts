import { Component } from "@angular/core";
import { RegisterUserRequest } from "./models/RegisterUserRequest";
import { UserRegisterService } from "./user-register.service";
import { Router } from "@angular/router";
import { StatusMessageService } from "../../services/statusmessage.service";
import { HttpErrorResponse } from "@angular/common/http";


@Component({
  selector: 'app-root',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  title = "User Register";

  registerUserRequest: RegisterUserRequest = {
    UserName: "",
    PasswordEncrypto: "",
    DisplayName: ""
  };

  isLoading: boolean = false;

  constructor(
    private userRegisterService: UserRegisterService,
    private router: Router,
    private statusMessageService: StatusMessageService) {

  }

  OnSubmit() {
    this.isLoading = true;
    this.userRegisterService.Register(this.registerUserRequest).subscribe({
      next: () => this.router.navigate(["/"]),
      error: errReponse => {
        if (errReponse instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = errReponse.message;
        }
        this.isLoading = false;
      }
    });
  }
}
