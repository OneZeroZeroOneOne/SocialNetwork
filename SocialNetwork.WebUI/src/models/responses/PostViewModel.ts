import { Guid } from "@/utilities/guid";

export interface IPost {
    id:	Guid
    text: string
    nullable: boolean
    date: string
    userId: string
}

export interface IPosts {
    Posts: IPost[]
}