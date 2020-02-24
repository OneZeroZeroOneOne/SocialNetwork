import { Guid } from "@/utilities/guid";
import { IAttachment } from './Attachment';

export interface IComment {
    id: number
    date: string
    userId: Guid
    text: string
    postId: number
    attachmentComment: IAttachment[]
}

export interface IComments {
    comments: IComment[]
}