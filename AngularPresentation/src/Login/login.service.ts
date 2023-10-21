import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { LoginResponse } from "../Models/LoginResponse";
import { Observable } from "rxjs";
import { LoginRequest } from "../Models/LoginRequest";

const headers = { 'Content-Type': 'application/json' };

@Injectable({
  providedIn: "root"
})
export class LoginService{

  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json" , "Access-Control-Allow-Origin":"*"});

  constructor(private httpClient: HttpClient, @Inject('BACKEND_BASE_URL') private backendBaseUrl: string) {

  }

  public Authenticate(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this.backendBaseUrl + "Authentication/Authenticate", JSON.stringify(request), { headers: this.httpHeaders });
  }
}
