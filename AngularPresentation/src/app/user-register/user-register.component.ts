import { Component } from "@angular/core";
import { NgForm } from "@angular/forms";
import { RegisterUserRequest } from "./models/RegisterUserRequest";


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

  constructor() {

  }

  OnSubmit(form: NgForm) {

  }

}
