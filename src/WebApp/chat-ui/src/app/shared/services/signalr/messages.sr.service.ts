import { Inject, Injectable } from '@angular/core';
import { API_BASE_URL, ChatRoomModel, MessageModel } from 'src/clients/client.generated';
import { BaseSignalRService } from './base-signalr.service';

@Injectable({providedIn: 'root'})
export class MessageSignalRService extends BaseSignalRService<MessageModel> {
  constructor(
    @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(`${baseUrl}/hub/messages`);
  }
}
