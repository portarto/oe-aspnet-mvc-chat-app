import { Injectable } from '@angular/core';
import { TokenModel } from 'src/clients/client.generated';

@Injectable({
  providedIn: 'root'
})
export class TokenProvider {
  public get tokenModel(): TokenModel {
    return JSON.parse(localStorage.getItem('tokenModel'));
  }

  public set tokenModel(tokenModel: TokenModel) {
    localStorage.setItem('tokenModel', JSON.stringify(tokenModel));
  }

  public clear(): void {
    localStorage.setItem('tokenModel', undefined);
  }
}