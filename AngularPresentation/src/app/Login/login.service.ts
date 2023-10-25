import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LoginRequest } from "./Models/LoginRequest";
import { LoginResponse } from "./Models/LoginResponse";

@Injectable({
  providedIn: "root"
})
export class LoginService{

  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json" , "Access-Control-Allow-Origin":"*"});

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public Authenticate(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this.baseUrl + "Authentication/Authenticate", JSON.stringify(request), { headers: this.httpHeaders });
  }
}
