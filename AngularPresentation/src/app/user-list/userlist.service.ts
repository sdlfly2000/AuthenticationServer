import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class UserService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject("BASE_URL") private BaseUrl: string) {

  }

  GetUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.BaseUrl + "api/ClaimManager/GetClaimByUserId?id=" + UserID);
  }
}
