import { Inject, Injectable } from '@angular/core';
import { API_BASE_URL, ChatRoomModel } from 'src/clients/client.generated';
import { BaseSignalRService } from './base-signalr.service';

@Injectable({providedIn: 'root'})
export class ChatRoomsSignalRService extends BaseSignalRService<ChatRoomModel> {
  constructor(
    @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(`${baseUrl}/hub/rooms`);
  }
}
