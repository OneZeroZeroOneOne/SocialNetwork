import { Guid } from "@/utilities/guid";

export interface IBoard {
    id: Guid
    boardTypeId: Guid
    isArchived: boolean
    name: string
    description: string
    settings: IBoardSetting[]
}

export interface IBoards {
    boards: IBoard[]
}

export interface IBoardSetting {
    name: string
    value: string
}