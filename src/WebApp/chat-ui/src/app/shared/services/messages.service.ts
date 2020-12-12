import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatRoomClient, ChatRoomModel, MessageClient, MessageModel } from 'src/clients/client.generated';

@Injectable()
export class MessageService {
  
  constructor(
    private readonly messageClient: MessageClient,
    private readonly chatRoomClient: ChatRoomClient
  ) { }

  public sendMessage(message: MessageModel): void {
    
  }
}
