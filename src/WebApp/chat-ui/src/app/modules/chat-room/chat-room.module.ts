import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { AngularMaterialModule } from 'src/app/shared/angular-material.module';
import { BaseModule } from 'src/app/shared/base-components/base.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { ChatRoomsListComponent } from './components/chat-rooms-list/chat-rooms-list.component';
import { ChatRoomsComponent } from './components/chat-rooms/chat-rooms.component';
import { MessagesListComponent } from './components/messages-list/messages-list.component';
import { AddChatRoomParticipantsModal } from './components/modals/add-chat-room-participants/add-chat-room-participants.component';

@NgModule({
  declarations: [
    ChatRoomsListComponent,
    MessagesListComponent,
    ChatRoomsComponent,
    AddChatRoomParticipantsModal
  ],
  imports: [
    SharedModule,
    AngularMaterialModule
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA ]
})
export class ChatRoomModule {}
