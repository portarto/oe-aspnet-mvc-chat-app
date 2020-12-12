import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ChatUserModel, IdentityClient, LoginModel, RegisterModel, TokenModel } from 'src/clients/client.generated';
import { TokenProvider } from '../providers/token-provider';

@Injectable()
export class IdentityService {
  constructor(
    private readonly identityClient: IdentityClient,
    private readonly tokenProvider: TokenProvider
  ) {}

  public get currentUser(): Observable<ChatUserModel> {
    return this.identityClient.getCurrentUser();
  }

  public updateUser(userModel: ChatUserModel): Observable<ChatUserModel> {
    return this.identityClient.updateUser(userModel);
  }

  public registerUser(registerModel: RegisterModel): Observable<ChatUserModel> {
    return this
      .identityClient
      .registerUser(registerModel)
    ;
  }
  
  public loginUser(loginrModel: LoginModel): Observable<boolean> {
    return this
      .identityClient
      .login(loginrModel)
      .pipe(
        map(
          tokenModel => {
            if (tokenModel) {
              this.tokenProvider.tokenModel = tokenModel;
              return true;
            }
            return false;
          }
        )
      )
    ;
  }

  public getUsers(): Observable<ChatUserModel[]> {
    return this
      .identityClient
      .getAllUsers()
    ;
  }
}
