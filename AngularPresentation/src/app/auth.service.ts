import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class AuthService {

  get JwtToken() : string | null {
    return localStorage.getItem("AuthJwt");
  }

  set JwtToken(value: string) {
    localStorage.setItem("AuthJwt", value);
  }
}
