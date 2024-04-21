import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserClaim } from "./Models/UserClaim";

@Injectable({
  providedIn: "root"
})
export class UserService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject("BASE_URL") private BaseUrl: string) {

  }

  GetUserClaims(UserID: string): Observable<UserClaim[]> {
    return this.httpClient.get<UserClaim[]>(this.BaseUrl + "api/ClaimManager/GetClaimByUserId?id=" + UserID);
  }

  UpdateUserClaim(UserId: string, UserClaim: UserClaim): Observable<string> {
    return this.httpClient.post<string>(this.BaseUrl + "api/ClaimManager/UpdateUserClaim?id=" + UserId, JSON.stringify(UserClaim), { headers: this.httpHeaders });
  }

  AddUserClaim(UserId: string, UserClaim: UserClaim): Observable<string> {
    return this.httpClient.post<string>(this.BaseUrl + "api/ClaimManager/AddUserClaim?id=" + UserId, JSON.stringify(UserClaim), { headers: this.httpHeaders });
  }
}
