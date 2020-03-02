import axios, { AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';
import { IPostService } from '../Abstractions/IPostService';

export class PostService implements IPostService {
    private postsAxios = axios.create();

    async getPost(board_id: Guid, post_id: string): Promise<AxiosResponse<IPost>> {
        let url = 'http://16ch.tk/api/social/Posts/' + board_id.toString() + "/" + post_id;
        return (await this.postsAxios.get<IPost>(url));
    }
    
    async getNewPosts(user_id: string, page: string, quantity: string): Promise<AxiosResponse<IPagedResult<IPost>>> {
        let url = 'http://16ch.tk/api/social/Post/Page/' + "?page=" + page + "?page=" + quantity;
        return (await this.postsAxios.get<IPagedResult<IPost>>(url));
    }
}
