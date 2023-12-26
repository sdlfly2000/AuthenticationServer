import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { RegisterUserRequest } from "./models/RegisterUserRequest";
import { UserRegisterService } from "./user-register.service";
import { Router } from "@angular/router";
import { StatusMessageService } from "../statusmessage.service";
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
    Password: "",
    DisplayName: ""
  };

  isLoading: boolean = false;

  constructor(
    private userRegisterService: UserRegisterService,
    private router: Router,
    private statusMessageService: StatusMessageService) {

  }

  OnSubmit(form: NgForm) {
    this.isLoading = true;
    this.userRegisterService.Register(this.registerUserRequest).subscribe(
      res => this.router.navigate(["/"]),
      error => {
        if (error instanceof HttpErrorResponse) {
          this.statusMessageService.StatusMessage = error.message;
        }
        this.isLoading = false;
      }
    );
  }
}
