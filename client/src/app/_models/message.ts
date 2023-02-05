export interface Message {
    id: number;
    senderId: number;
    senderUserName: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientUsername: string;
    recipientphotoUrl: string;
    content: string;
    dateRead?: Date;
    messageSent: Date;
}