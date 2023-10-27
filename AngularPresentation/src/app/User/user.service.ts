import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserClaim } from "./Models/UserClaim";

@Injectable({
  providedIn: "root"
})
export class UserService {
  constructor(private httpClient: HttpClient, @Inject("BASE_URL") private BaseUrl: string) {

  }

  GetUserClaims(UserID: string): Observable<UserClaim[]> {
    return this.httpClient.get<UserClaim[]>(this.BaseUrl + "api/ClaimManager/GetClaimByUserId?id=" + UserID);
  }
}
