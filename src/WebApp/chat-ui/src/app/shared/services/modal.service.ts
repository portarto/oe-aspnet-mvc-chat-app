import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { take } from 'rxjs/operators';
import { BaseModalComponent } from '../base-components/base-modal/base-modal.component';
import { BaseModalOptions } from '../base-components/base-modal/models/base-modal.model';

@Injectable()
export class ModalService {
  constructor(
    public readonly dialog: MatDialog
  ) {}

  public openCustomDialog<TResult, TModalComponent>(
    modalOptions: BaseModalOptions,
    component: ComponentType<TModalComponent>,
    width: string
  ): Promise<TResult> {
    return this
      .dialog
      .open(component, {
        width: width,
        data: modalOptions
      })
      .afterClosed()
      .pipe(take(1))
      .toPromise()
    ;
  }

  public openDialog<TResult>(
    title: string,
    label: string,
    initialData?: string
  ): Promise<TResult> {
    const modalOptions: BaseModalOptions = {
      title: title,
      label: label,
      cancelText: 'Cancel',
      okText: 'Ok',
      data: initialData
    };
    return this.openBaseDialog<TResult>(
      modalOptions,
      '230px'
    );
  }

  public openBaseDialog<TResult>(
    modalOptions: BaseModalOptions,
    width: string
  ): Promise<TResult> {
    return this
      .dialog
      .open(BaseModalComponent, {
        width: width,
        data: modalOptions
      })
      .afterClosed()
      .pipe(take(1))
      .toPromise()
    ;
  }
}
