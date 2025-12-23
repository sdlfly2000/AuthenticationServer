import { BASE_URL } from "@/app.config";
import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AppTopBarService {
  private baseUrl: string;

  constructor(private httpClient: HttpClient) {
    this.baseUrl = Inject(BASE_URL);
  }

  public logout(): Observable<string> {
    return this.httpClient.get<string>(this.baseUrl + "api/Authentication/Logout")
  }
}
