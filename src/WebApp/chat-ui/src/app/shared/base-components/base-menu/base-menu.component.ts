import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { IBaseMenuModel } from './models/base-menu.model';

@Component({
  selector: 'base-menu',
  templateUrl: './base-menu.component.html',
  styleUrls: ['./base-menu.component.scss'],
  encapsulation: ViewEncapsulation.Emulated
})
export class BaseMenuComponent{
  @Input()
  public menuItems: IBaseMenuModel[];
  @Input()
  public dataItem: any;
}
