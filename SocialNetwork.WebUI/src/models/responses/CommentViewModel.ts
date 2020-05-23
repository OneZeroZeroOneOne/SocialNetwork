import { Guid } from "@/utilities/guid";
import { IAttachment } from './Attachment';
import { IMention } from './MentionViewModel';

export interface IComment {
    id: number
    date: string
    userId: Guid
    text: string
    postId: number
    seqId: number
    mention: IMention[]
    attachmentComment: IAttachment[]
}

export interface IComments {
    comments: IComment[]
}