import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChatRoomService } from 'src/app/shared/services/chat-room.service';
import { take } from 'rxjs/operators';
import { ChatRoomModel } from 'src/clients/client.generated';
import { ChatRoomsSignalRService } from 'src/app/shared/services/signalr/chat-rooms.sr.services';
import { Subject } from 'rxjs';
import { ChatRoomListItem } from './models/chat-room-list-item.model';
import { IBaseMenuModel } from 'src/app/shared/base-components/base-menu/models/base-menu.model';
import { ModalService } from 'src/app/shared/services/modal.service';
import { AddChatRoomParticipantsModal } from '../modals/add-chat-room-participants/add-chat-room-participants.component';

@Component({
  selector: 'app-chat-rooms-list',
  templateUrl: './chat-rooms-list.component.html',
  styleUrls: ['./chat-rooms-list.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class ChatRoomsListComponent implements OnInit{
  @Input()
  public set currentRoomId(id: string) {
    if (!this.selectedRoomId) {
      this.onNewSelectedRoom.next(id);
    }
  }

  @Output()
  public roomChanged = new EventEmitter<ChatRoomModel>();

  public rooms: ChatRoomListItem[] = [];
  public baseMenuItems = [{ 
      text: 'Rename Room',
      icon: 'edit',
      action: (item) => this.renameRoom(item)
    }, {
      text: 'Add Participants',
      icon: 'add',
      action: (item) => this.openParticipantsModal(item)
    }
  ];

  private selectedRoomId: string;
  private readonly onNewSelectedRoom = new Subject<string>();

  constructor(
    public dialog: MatDialog,
    public chatRoomService: ChatRoomService,
    public signalrService: ChatRoomsSignalRService,
    private readonly modalService: ModalService
  ) { 
    this.onNewSelectedRoom.subscribe(this.handleNewSelectedRoom);
  }

  public ngOnInit() {
    this.getRooms();
  }

  public onRoomChange(room: ChatRoomListItem): void {
    this.onNewSelectedRoom.next(room.id);
    if (this.roomChanged && this.roomChanged.observers && this.roomChanged.observers.length > 0) {
      this.roomChanged.emit(room);
    }
  }

  private handleNewSelectedRoom = (id: string): void => {
    if (id) {
      this.selectedRoomId = id;
    }
    if (this.selectedRoomId) {
      this.setSelectedRoomById(this.selectedRoomId);
    }
  }

  private setSelectedRoomById(id: string): void {
    if (this.rooms && this.rooms.length > 0) {
      this.rooms.forEach(
        room => {
          room.isSelected = room.id === id;
        }
      );
    }
  }

  private getRooms(): void {
    this.chatRoomService
      .getChatRoomsForCurrentUser()
      .pipe(take(1))
      .subscribe(
        result => {
          this.rooms = result;
          this.onNewSelectedRoom.next();
          this.registerSignalR();
        }
      )
    ;
  }

  private registerSignalR(): void {
    this.signalrService
      .register(
        'newRoom',
        room => this.rooms.push(room)
      )
    ;
  }

  private openParticipantsModal(room: ChatRoomModel): void {
    this.modalService
      .openCustomDialog<string[], AddChatRoomParticipantsModal>({
          title: 'Add chat room participants',
          label: 'Chat room participants',
          cancelText: 'Cancel',
          okText: 'Ok',
          data: room
        },
        AddChatRoomParticipantsModal,
        '400px'
      )
      .then(results => this.addParticipants(results))
      .catch(error => console.error(error))
    ;
  }

  private addParticipants(userIds: string[]) {
    if (userIds && userIds.length > 0) {
      this
        .chatRoomService
        .addParticipantsToRoom(this.selectedRoomId, userIds)
        .subscribe(() => console.log('added'))
      ;
    }
  }

  private renameRoom(room: ChatRoomModel): void {
    this
      .modalService
      .openDialog<string>('Add new room', 'New room', room.name)
      .then(result => this.updateRoom(room.id, result))
      .catch(error => console.log('error', error))
    ;
  }

  private updateRoom(id: string, name: string): void {
    if (id && name) {
      this.chatRoomService
        .updateRoom(id, name)
        .subscribe(
          result => {
            this.rooms.find(room => room.id === result.id).name = result.name;
          }
        )
      ;
    }
  }
}
