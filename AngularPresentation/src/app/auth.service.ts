import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AuthService {

  private displayNameSubject: Subject<string>;

  constructor() {
    this.displayNameSubject = new Subject<string>();
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

  RemoveLocalUserDisplayName() {
    localStorage.removeItem("UserDisplayName");
    this.displayNameSubject.next("");
  }

  CleanLocalCache() {
    this.RemoveLocalJwt();
    this.RemoveLocalUserDisplayName();
    this.RemoveLocalUserId();
  }
}
