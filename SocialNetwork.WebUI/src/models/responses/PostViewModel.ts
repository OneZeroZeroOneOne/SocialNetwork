import { Guid } from "@/utilities/guid";
import { IAttachment } from './Attachment';
import { IMention } from './MentionViewModel';

export interface IPost {
    id:	number
    text: string
    title: string
    nullable: boolean
    date: string
    userId: string
    mention: IMention[]
    attachmentPost: IAttachment[]
    boardId: Guid
}

export interface IPosts {
    Posts: IPost[]
}