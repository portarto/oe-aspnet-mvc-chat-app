import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalOptions } from './models/base-modal.model';

@Component({
  selector: 'base-modal',
  templateUrl: 'base-modal.component.html',
})
export class BaseModalComponent {
  constructor(
    public dialogRef: MatDialogRef<BaseModalComponent>,
    @Inject(MAT_DIALOG_DATA)
    public readonly data: BaseModalOptions
  ) {
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onOk(): void {
    this.dialogRef.close(this.data.data);
  }
}
