import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IBoard } from '@/models/responses/Board';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';

export class BoardService {
    private static boardAxios = axios.create();

    static async getBoards(): Promise<IBoard[]> {
        let url = 'http://localhost:56075/Board/';
        return (await this.boardAxios.get<IBoard[]>(url)).data;
    }

    static async getBoardByName(name: string): Promise<AxiosResponse<IBoard>> {
        let url = 'http://localhost:56075/Board/' + encodeURIComponent(name);
        return await this.boardAxios.get<IBoard>(url);
    }
}
