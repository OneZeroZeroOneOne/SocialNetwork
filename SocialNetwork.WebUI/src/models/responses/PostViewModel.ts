import { Guid } from "@/utilities/guid";
import { IAttachment } from './Attachment';

export interface IPost {
    id:	number
    text: string
    title: string
    nullable: boolean
    date: string
    userId: string
    attachmentPost: IAttachment[]
    boardId: Guid
}

export interface IPosts {
    Posts: IPost[]
}