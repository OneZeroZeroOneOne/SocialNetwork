import { Guid } from "@/utilities/guid";

export interface IComment {
    id: number
    date: string
    userId: Guid
    text: string
    postId: number
}

export interface IComments {
    comments: IComment[]
}