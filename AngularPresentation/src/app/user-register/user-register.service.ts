import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable} from "@angular/core";
import { Observable } from "rxjs";
import { RegisterUserRequest } from "./models/RegisterUserRequest";

@Injectable({
  providedIn: 'root'
})
export class UserRegisterService {
  private httpHeaders: HttpHeaders = new HttpHeaders({ "Content-Type": "application/json", "Access-Control-Allow-Origin": "*" })

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public Register(request: RegisterUserRequest): Observable<string> {
    return this.httpClient.post<string>(this.baseUrl + "api/UserManager/Create", JSON.stringify(request), { headers: this.httpHeaders })
  }
}
