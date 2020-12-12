import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { take } from 'rxjs/operators';
import { BaseModalOptions } from 'src/app/shared/base-components/base-modal/models/base-modal.model';
import { ChatRoomService } from 'src/app/shared/services/chat-room.service';
import { IdentityService } from 'src/app/shared/services/identity-service';
import { ChatRoomModel, ChatUserModel } from 'src/clients/client.generated';
import { ChatRoomParticipantModel } from './models/chat-room-participant.model';

@Component({
  selector: 'base-modal',
  templateUrl: 'add-chat-room-participants.component.html',
  styleUrls: ['add-chat-room-participants.component.scss']
})
export class AddChatRoomParticipantsModal implements OnInit {
  public readonly columns: string[] = ['isSelected', 'username'];
  public users: ChatRoomParticipantModel[] = [];
  public get selectedUserIds(): string[] {
    return this
      .users
      .filter(user => user.isSelected)
      .map(user => user.id)
    ;
  }

  constructor(
    public dialogRef: MatDialogRef<AddChatRoomParticipantsModal>,
    @Inject(MAT_DIALOG_DATA)
    public readonly data: BaseModalOptions<ChatRoomModel>,
    public readonly chatRoomService: ChatRoomService
  ) {
  }

  public ngOnInit(): void {
    this.loadUsers();
  }

  private loadUsers(): void {
    this.chatRoomService
      .getNonRoomParticipants(this.data.data.id)
      .pipe(take(1))
      .subscribe(
        users => {
          this.users = users.map(user => ({
            id: user.id,
            username: user.username,
            email: user.email,
            isSelected: false
          } as ChatRoomParticipantModel));
        }
      )
    ;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onOk(): void {
    this.dialogRef.close(this.data);
  }
}
