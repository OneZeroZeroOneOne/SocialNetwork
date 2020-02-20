import { Guid } from "@/utilities/guid";

export interface IPost {
    id:	number
    text: string
    nullable: boolean
    date: string
    userId: string
}

export interface IPosts {
    Posts: IPost[]
}