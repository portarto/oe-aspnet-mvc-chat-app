import { Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ChatRoomService } from 'src/app/shared/services/chat-room.service';
import { ChatRoomModel, ChatUserModel, MessageModel } from 'src/clients/client.generated';
import { IBaseMenuModel } from 'src/app/shared/base-components/base-menu/models/base-menu.model';
import { ModalService } from 'src/app/shared/services/modal.service';
import { take } from 'rxjs/operators';
import { IdentitySettingsModalComponent } from 'src/app/modules/identity/components/identity-settings/identity-settings.component';
import { IdentityService } from 'src/app/shared/services/identity-service';

@Component({
  selector: 'app-chat-rooms',
  templateUrl: './chat-rooms.component.html',
  styleUrls: ['./chat-rooms.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class ChatRoomsComponent implements OnInit{
  @ViewChild('messagesContent', { static: false })
  public set messagesContent(elRef: ElementRef) {
    this.observer = new MutationObserver(
      mutations => {
        mutations.forEach(
          mutation => {
            if (mutation.type === 'childList') {
              this.scrollDown();
            }
          }
        )
      }
    );
    this._messagesContent = elRef;
    var config = { attributes: true, childList: true, characterData: true, subtree: true };
    this.observer.observe(this._messagesContent.nativeElement, config);
  }

  public roomId: string;
  public currentRoom: ChatRoomModel;
  public chatRooms: ChatRoomModel[] = [];
  public message: string;
  public messages: MessageModel[] = [];

  private _messagesContent: ElementRef;
  private observer: MutationObserver;
  private scrollTimeOut: any;

  constructor(
    private readonly httpClient: HttpClient,
    private readonly router: Router,
    public readonly modalService: ModalService,
    private route: ActivatedRoute,
    public dialog: MatDialog,
    public chatRoomService: ChatRoomService,
    private readonly identityService: IdentityService
  ) { }

  public ngOnInit() {
    this.route.params.subscribe(
      params => {
        if (params['id']) {
          this.roomId = params['id'];
        }
      }
    );
  }

  public async scrollDown(): Promise<void> {
    if (this.scrollTimeOut) {
      window.clearTimeout(this.scrollTimeOut);
    }
    this.scrollTimeOut = setTimeout(() => {
      if (this._messagesContent) {
        this._messagesContent.nativeElement.scrollTop = this._messagesContent.nativeElement.scrollHeight;
      }
    }, 300);
  }

  public onRoomChanged(room: ChatRoomModel): void {
    this.currentRoom = room;
    this.router.navigate([`/chat-rooms/${room.id}`]);
  }

  public onOpenNewRoomDialog(): void {
  }

  public openRoom(id: string) {
  }

  public sendMessage() {
    this.chatRoomService
      .sendMessageToRoom(
        this.roomId,
        { text: this.message }
      )
      .subscribe(result => { this.message = ''; })
    ;
  }

  public addNewRoom(): void {
    this
      .modalService
      .openDialog<string>('Add new room', 'New room')
      .then(result => this.createRoom(result))
      .catch(error => console.log('error', error))
    ;
  }

  public openIdentitySettings(): void {
    this.modalService
      .openCustomDialog<ChatUserModel, IdentitySettingsModalComponent>({
          title: 'Add chat room participants',
          label: 'Chat room participants',
          cancelText: 'Cancel',
          okText: 'Ok'
        },
        IdentitySettingsModalComponent,
        '500px'
      )
      .then(result => this.updateUser(result))
      .catch(error => console.error(error))
    ;
  }

  private updateUser(user: ChatUserModel): void {
    this.identityService
      .updateUser(user)
      .subscribe(result => console.log(result))
    ;
  }

  private createRoom(roomName: string): void {
    if (roomName) {
      const model: ChatRoomModel = {
        name: roomName
      };
      this
        .chatRoomService
        .addNewRoom(model)
        .pipe(take(1))
        .subscribe(
          result => {
            console.log('result', result);
          }
        )
      ;
    }
  }
}
