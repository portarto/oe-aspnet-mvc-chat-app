import { NgModule } from '@angular/core';
import { API_BASE_URL, ChatRoomClient, IdentityClient, MessageClient } from 'src/clients/client.generated';
import { environment } from 'src/environments/environment';

@NgModule({
  providers: [
    { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
    ChatRoomClient,
    IdentityClient,
    MessageClient
  ]
})
export class ClientsModule {}
