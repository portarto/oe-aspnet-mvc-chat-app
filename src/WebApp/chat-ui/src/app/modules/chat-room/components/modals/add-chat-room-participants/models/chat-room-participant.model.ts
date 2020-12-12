import { ChatUserModel } from "src/clients/client.generated";

export interface ChatRoomParticipantModel extends ChatUserModel {
  isSelected?: boolean;
}