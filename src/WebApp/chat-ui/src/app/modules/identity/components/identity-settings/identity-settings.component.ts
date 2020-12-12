import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalOptions } from 'src/app/shared/base-components/base-modal/models/base-modal.model';
import { IdentityService } from 'src/app/shared/services/identity-service';
import { ChatUserModel } from 'src/clients/client.generated';

@Component({
  selector: 'base-modal',
  templateUrl: 'identity-settings.component.html',
  styleUrls: ['identity-settings.component.scss']
})
export class IdentitySettingsModalComponent implements OnInit {
  public get userModel(): ChatUserModel {
    return this.userForm.getRawValue();
  }

  public userForm = this.fb.group({
    email: [null, [ Validators.required, Validators.email ]],
    username: [null, Validators.required],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    dateOfBirth: [null, Validators.required],
    details: [null],
    phoneNumber: [null],
  });

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<IdentitySettingsModalComponent>,
    @Inject(MAT_DIALOG_DATA)
    public readonly data: BaseModalOptions<ChatUserModel>,
    public readonly identityService: IdentityService
  ) {
    this.identityService.currentUser.subscribe(user => this.mapUserToForm(user));
  }

  public ngOnInit(): void {

  }

  public onCancel(): void {
    this.dialogRef.close();
  }

  public onOk(): void {
    this.dialogRef.close(this.data.data);
  }

  private mapUserToForm(user: ChatUserModel): void {
    this.userForm.setValue({
      email: user.email,
      username: user.username,
      firstName: user.firstName,
      lastName: user.lastName,
      dateOfBirth: user.dateOfBirth,
      details: user.details,
      phoneNumber: user.phoneNumber
    });
  }
  
}
