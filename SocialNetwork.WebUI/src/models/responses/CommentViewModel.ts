import { Guid } from "@/utilities/guid";

export interface IComment {
    id: Guid
    date: string
    userId: Guid
    text: string
    postId: Guid
}

export interface IComments {
    comments: IComment[]
}