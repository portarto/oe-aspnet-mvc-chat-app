import { NgModule } from '@angular/core';
import { AngularMaterialModule } from 'src/app/shared/angular-material.module';
import { BaseMenuComponent } from './base-menu/base-menu.component';
import { BaseModalComponent } from './base-modal/base-modal.component';

@NgModule({
  imports: [
    AngularMaterialModule
  ],
  declarations: [
    BaseMenuComponent,
    BaseModalComponent
  ],
  exports: [
    BaseMenuComponent,
    BaseModalComponent
  ]
})
export class BaseModule {}
