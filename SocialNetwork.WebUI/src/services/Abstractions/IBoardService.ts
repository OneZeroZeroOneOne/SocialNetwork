import { AxiosResponse } from 'axios';
import { IBoard } from '@/models/responses/Board';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';
import { IPost } from '@/models/responses/PostViewModel';

export interface IBoardService {
    getBoards(): Promise<AxiosResponse<IBoard[]>>;
    getBoardByName(name: string): Promise<AxiosResponse<IBoard>>;
    getPostsOnBoard(boardId: Guid, page: number, quantity: number): Promise<AxiosResponse<IPagedResult<IPost>>>;
}
