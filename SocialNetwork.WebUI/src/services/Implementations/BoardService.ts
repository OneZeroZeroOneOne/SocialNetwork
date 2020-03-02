import axios, { AxiosResponse } from 'axios';
import { IBoard } from '@/models/responses/Board';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';
import { IPost } from '@/models/responses/PostViewModel';
import { IBoardService } from '@/services/Abstractions/IBoardService';
import apiClient from '@/services/Implementations/ApiClient';


export class BoardService implements IBoardService {
    private boardAxios = apiClient;

    async getBoards(): Promise<AxiosResponse<IBoard[]>> {
        return this.boardAxios.get<IBoard[]>('board/Board')
    }

    async getBoardByName(name: string): Promise<AxiosResponse<IBoard>> {
        return this.boardAxios.get<IBoard>(`board/Board/${encodeURIComponent(name)}`);
    }

    async getPostsOnBoard(boardId: Guid, page: number, quantity: number): Promise<AxiosResponse<IPagedResult<IPost>>> {
        return this.boardAxios.get<IPagedResult<IPost>>(`social/Posts/${boardId.toString()}/Page`, {
            params: {
                page: page.toString(),
                quantity: quantity.toString(),
            }
        });
    }
}
