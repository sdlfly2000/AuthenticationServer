import { HttpClient, HttpResponse } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class NavMenuService {

  constructor(private httpClient: HttpClient, @Inject("BASE_URL") private baseUrl: string) {

  }

  public logout(): Observable<string> {
    return this.httpClient.get<string>(this.baseUrl + "api/Authentication/Logout")
  }
}
