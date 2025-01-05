import { Injectable } from "@angular/core";
import { Observable, Subject, filter } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AuthService {

  private displayNameSubject: Subject<string>;
  private isLogin: Subject<boolean>;

  constructor() {
    this.displayNameSubject = new Subject<string>();
    this.isLogin = new Subject<boolean>();
  }

  get JwtToken() : string | null {
    return localStorage.getItem("AuthJwt");
  }

  set JwtToken(value: string) {
    localStorage.setItem("AuthJwt", value);
  }

  RemoveLocalJwt() {
    localStorage.removeItem("AuthJwt");
  }

  get UserId() : string | null{
    return localStorage.getItem("UserID");
  }

  set UserId(value: string) {
    localStorage.setItem("UserID", value);
  }

  RemoveLocalUserId() {
    localStorage.removeItem("UserID");
  }

  get OnUserDisplayName(): Observable<string> {
    return this.displayNameSubject;
  }

  get UserDisplayName(): string | null {
    return localStorage.getItem("UserDisplayName");
  }

  set UserDisplayName(value: string) {
    localStorage.setItem("UserDisplayName", value);
    this.displayNameSubject.next(value);
  }

  get OnLoginSuccess(): Observable<boolean> {
    return this.isLogin.pipe(
      filter(SuccessLogin => SuccessLogin));
  }

  get OnLoginFailure(): Observable<boolean> {
    return this.isLogin.pipe(
      filter(SuccessLogin => !SuccessLogin));
  }

  set LoginStatus(value: boolean) {
    this.isLogin.next(value);
  }
  
  RemoveLocalUserDisplayName() {
    localStorage.removeItem("UserDisplayName");
    this.displayNameSubject.next("");
  }

  CleanLocalCache() {
    this.RemoveLocalJwt();
    this.RemoveLocalUserDisplayName();
    this.RemoveLocalUserId();
  }

  CheckLoginStatus() {
    if (this.JwtToken != null && this.UserDisplayName != null && this.UserId != null) {
      this.LoginStatus = true;
    }
  }
}
