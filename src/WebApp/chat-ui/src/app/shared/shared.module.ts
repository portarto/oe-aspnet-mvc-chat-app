import { NgModule } from '@angular/core';
import { API_BASE_URL } from 'src/clients/client.generated';
import { environment } from 'src/environments/environment';
import { BaseModule } from './base-components/base.module';
import { ClientsModule } from './clients/clients.module';
import { ChatRoomService } from './services/chat-room.service';
import { IdentityService } from './services/identity-service';
import { MessageService } from './services/messages.service';
import { ModalService } from './services/modal.service';

@NgModule({
  imports: [
    ClientsModule
  ],
  exports: [
    BaseModule
  ],
  providers: [
    { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
    ChatRoomService,
    IdentityService,
    ModalService,
    MessageService
  ]
})
export class SharedModule {}
