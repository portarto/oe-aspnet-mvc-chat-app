import { ChatRoomModel } from 'src/clients/client.generated';

export interface ChatRoomListItem extends ChatRoomModel {
  isSelected?: boolean;
}