import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  public authToken : string = "";

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    this.authToken = AuthService.JwtToken;

    if (this.authToken != "" || this.authToken != null) {
      const authReq = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + this.authToken)
      });
      // send cloned request with header to the next handler.
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
