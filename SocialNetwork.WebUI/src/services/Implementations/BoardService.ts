import axios, { AxiosResponse } from 'axios';
import { IBoard } from '@/models/responses/Board';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';
import { IPost } from '@/models/responses/PostViewModel';
import { IBoardService } from '@/services/Abstractions/IBoardService';

export class BoardService implements IBoardService {
    private boardAxios = axios.create();

    async getBoards(): Promise<AxiosResponse<IBoard[]>> {
        let url = 'http://16ch.tk/api/board/Board/';
        return (await this.boardAxios.get<IBoard[]>(url));
    }

    async getBoardByName(name: string): Promise<AxiosResponse<IBoard>> {
        let url = 'http://16ch.tk/api/board/Board/' + encodeURIComponent(name);
        return await this.boardAxios.get<IBoard>(url);
    }

    async getPostsOnBoard(boardId: Guid, page: number, quantity: number): Promise<AxiosResponse<IPagedResult<IPost>>> {
        let url = 'http://16ch.tk/api/social/Posts/' + boardId.toString() + "/Page?page=" + page.toString() + "&quantity=" + quantity.toString();
        return await this.boardAxios.get<IPagedResult<IPost>>(url);
    }
}
