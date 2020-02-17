import { Guid } from "@/utilities/guid";

export interface IBoard {
    id: Guid
    boardTypeId: Guid
    isArchived: boolean
    name: string
    description: string
}

export interface IBoards {
    boards: IBoard[]
}