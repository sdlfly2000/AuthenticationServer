import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { LoginRequest } from "../Models/LoginRequest";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class LoginService{

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public Login(request: LoginRequest): Observable<any> {
    return this.httpClient.post("","");
  }
}
