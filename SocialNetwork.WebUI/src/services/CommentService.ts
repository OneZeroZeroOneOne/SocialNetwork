import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';

export class CommentService {
    private static commentAxios = axios.create();

    static async getCommentForPost(link_to_post: string, page: number = 1, quantity: number = 5): Promise<IPagedResult<IComment>> {
        let url = 'http://82.146.37.127/social/Comments/Page/' + link_to_post + "?page=" + page.toString() + "&quantity=" + quantity.toString();
        return (await this.commentAxios.get<IPagedResult<IComment>>(url)).data;
    }
}
