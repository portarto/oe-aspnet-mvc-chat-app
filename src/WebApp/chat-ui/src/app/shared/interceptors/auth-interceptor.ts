import { HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenProvider } from '../providers/token-provider';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private readonly tokenProvider: TokenProvider) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    if (this.tokenProvider.tokenModel) {
      const authReq = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${this.tokenProvider.tokenModel.token}`)
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
