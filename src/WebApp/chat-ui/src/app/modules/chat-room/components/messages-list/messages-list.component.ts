import { ThrowStmt } from '@angular/compiler';
import { AfterViewChecked, AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ChatRoomService } from 'src/app/shared/services/chat-room.service';
import { MessageService } from 'src/app/shared/services/messages.service';
import { MessageSignalRService } from 'src/app/shared/services/signalr/messages.sr.service';
import { MessageModel } from 'src/clients/client.generated';

@Component({
  selector: 'app-messages-list',
  templateUrl: './messages-list.component.html',
  styleUrls: ['./messages-list.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class MessagesListComponent implements OnInit {
  @Input()
  public set currentRoomId(roomId: string) {
    this._currentRoomId = roomId;
    this.onChatRoomIdChange.next(roomId);
  }

  @Output()
  public newMessage = new EventEmitter<void>();

  public messages: MessageModel[] = [];
  
  private readonly onChatRoomIdChange = new EventEmitter<string>();
  private isRegistered: boolean;
  private _currentRoomId: string;

  constructor(
    private readonly messageService: MessageService,
    private readonly chatRoomService: ChatRoomService,
    private readonly messageSignalrService: MessageSignalRService
  ) {
    this.onChatRoomIdChange.subscribe(id => this.getMessages(id));
  }
  
  ngOnInit(): void {
  }

  private getMessages(roomId: string): void {
    if (roomId) {
      this.chatRoomService
        .getMessagesForRoom(roomId)
        .subscribe(
          messages => {
            this.messages = messages;
            this.registerMessagesSignalr();
          }
        )
      ;
    }
  }

  private registerMessagesSignalr(): void {
    if (!this.isRegistered) {
      this.isRegistered = true;

      this.messageSignalrService
        .register(
          'newMessage',
          message => message.sentToChatRoomId === this._currentRoomId && this.messages.push(message)
        )
      ;
    }
  }

}
