import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';

export class PostService {
    private static postsAxios = axios.create();

    static async getPost(link_to_post: string): Promise<IPost> {
        let url = 'http://82.146.37.127/social/Posts/' + link_to_post;
        return (await this.postsAxios.get<IPost>(url)).data;
    }
}
