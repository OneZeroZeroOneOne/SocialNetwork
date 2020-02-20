import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';

export class PostService {
    private static postsAxios = axios.create();

    static async getPost(board_id: Guid, post_id: string): Promise<IPost> {
        let url = 'http://16ch.tk/social/Posts/' + board_id.toString() + "/" + post_id;
        return (await this.postsAxios.get<IPost>(url)).data;
    }
    static async getNewPosts(user_id: string, page: string, quantity: string): Promise<IPagedResult<IPost>> {
        let url = 'http://16ch.tk/social/Post/Page/' + "?page=" + page + "?page=" + quantity;
        return (await this.postsAxios.get<IPagedResult<IPost>>(url)).data;
    }
}
