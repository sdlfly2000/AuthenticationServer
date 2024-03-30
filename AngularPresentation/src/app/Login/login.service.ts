import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { LoginRequest } from "./Models/LoginRequest";
import { LoginResponse } from "./Models/LoginResponse";
import { AuthService } from "../auth.service";

@Injectable({
  providedIn: "root"
})
export class LoginService{

  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json" , "Access-Control-Allow-Origin":"*"});

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string, private authService: AuthService) {

  }

  private ErrorHandler(errorResponse: HttpErrorResponse) {
    this.authService.JwtToken = "";
    return throwError(() => new Error(errorResponse.error));
  }

  public Authenticate(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(this.baseUrl + "api/Authentication/Authenticate", JSON.stringify(request), { headers: this.httpHeaders })
      .pipe(
        catchError(this.ErrorHandler)
      );
  }
}
