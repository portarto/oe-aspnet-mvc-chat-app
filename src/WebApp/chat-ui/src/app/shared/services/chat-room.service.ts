import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatRoomClient, ChatRoomModel, ChatUserModel, MessageModel } from 'src/clients/client.generated';

@Injectable()
export class ChatRoomService {
  
  constructor(
    private readonly chatRoomClient: ChatRoomClient
  ) { }

  public getAll(): Observable<ChatRoomModel[]> {
    return this
      .chatRoomClient
      .getAll();
  }

  public getChatRoomsForCurrentUser(): Observable<ChatRoomModel[]> {
    return this
      .chatRoomClient
      .getChatRoomsForCurrentUser()
    ;
  }

  public addNewRoom(model: ChatRoomModel): Observable<any> {
    return this
      .chatRoomClient
      .create(model)
    ;
  }

  public updateRoom(roomId: string, name: string): Observable<ChatRoomModel> {
    return this.chatRoomClient.updateRoom(roomId, name);
  }

  public sendMessageToRoom(roomId: string, messageModel: MessageModel): Observable<MessageModel> {
    return this
      .chatRoomClient
      .sendMessageToRoom(roomId, messageModel)
    ;
  }

  public getMessagesForRoom(roomId: string): Observable<MessageModel[]> {
    return this
      .chatRoomClient
      .getMessagesByRoomId(roomId)
    ;
  }
  
  public getNonRoomParticipants(roomId: string): Observable<ChatUserModel[]> {
    return this
      .chatRoomClient
      .getNonParticipants(roomId)
    ;
  }

  public addParticipantsToRoom(roomId: string, userIds: string[]): Observable<void> {
    return this.chatRoomClient.addUsersById(roomId, userIds);
  }
}