import { Guid } from "@/utilities/guid";
import { IAttachment } from './Attachment';

export interface IPost {
    id:	number
    text: string
    nullable: boolean
    date: string
    userId: string
    attachmentPost: IAttachment[]
}

export interface IPosts {
    Posts: IPost[]
}