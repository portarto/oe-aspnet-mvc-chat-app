import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { TokenProvider } from './shared/providers/token-provider';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    private readonly router: Router,
    private readonly tokenProvider: TokenProvider
  ) { }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return route.data.reqAuth
      ? this.canActivateAuthenticated()
      : this.canActivateUnauthenticated();
    }

  private canActivateUnauthenticated(): boolean {
    if (this.tokenProvider.tokenModel) {
      this.router.navigate(['/chat-rooms']);
      return false;
    }
    
    return true;
  }

  private canActivateAuthenticated(): boolean {
    if (this.tokenProvider.tokenModel) {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }
}