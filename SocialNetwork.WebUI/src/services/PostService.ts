import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';

export class PostService {
    private static postsAxios = axios.create();

    static async getPost(link_to_post: string): Promise<IPost> {
        let url = 'http://82.146.37.127/social/Posts/' + link_to_post;
        return (await this.postsAxios.get<IPost>(url)).data;
    }
    static async getNewPosts(user_id: string, page: string, quantity: string): Promise<IPagedResult<IPost>> {
        let url = 'http://82.146.37.127/social/PostPage/'+ user_id+ "?page=" + page + "?page=" + quantity;
        return (await this.postsAxios.get<IPagedResult<IPost>>(url)).data;
    }
}
