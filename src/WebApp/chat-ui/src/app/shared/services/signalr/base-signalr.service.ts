import { Predicate } from '@angular/core';
import * as signalR from '@microsoft/signalr';

export class BaseSignalRService<TModel> {
  private hubConnection: signalR.HubConnection;

  constructor(uri: string) {
    this.hubConnection = 
      new signalR.HubConnectionBuilder()
        .withUrl(uri)
        .build()
    ;

    this.start();
  }

  public register(methodName: string, method: (item: TModel) => void): void {
    this.hubConnection.on(methodName, method);
  }

  public start(): Promise<void> {
    return this.hubConnection.start();
  }

  public stop(): Promise<void> {
    return this.hubConnection.stop();
  }

  public async restart(): Promise<void> {
    if (this.hubConnection) {
      await this.stop();
      return this.start();
    }
  }
}
