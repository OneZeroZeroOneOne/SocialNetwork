import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IBoard } from '@/models/responses/Board';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';
import { IPost } from '@/models/responses/PostViewModel';

export class BoardService {
    private static boardAxios = axios.create();

    static async getBoards(): Promise<IBoard[]> {
        let url = 'http://16ch.tk/api/board/Board/';
        return (await this.boardAxios.get<IBoard[]>(url)).data;
    }

    static async getBoardByName(name: string): Promise<AxiosResponse<IBoard>> {
        let url = 'http://16ch.tk/api/board/Board/' + encodeURIComponent(name);
        return await this.boardAxios.get<IBoard>(url);
    }

    static async getPostsOnBoard(boardId: Guid, page: number, quantity: number): Promise<AxiosResponse<IPagedResult<IPost>>> {
        let url = 'http://16ch.tk/api/social/Posts/' + boardId.toString() + "/Page?page=" + page.toString() + "&quantity=" + quantity.toString();
        return await this.boardAxios.get<IPagedResult<IPost>>(url);
    }
}
