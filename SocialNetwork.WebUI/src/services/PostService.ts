import axios, { AxiosStatic } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';

export class PostService {
    private static filesAxios = axios.create();

    static async getPost(link_to_video: string): Promise<IPost> {
        let url = 'http://82.146.37.127/social/Posts/' + link_to_video
        let response = await this.filesAxios.get<IPost>(url);
        return response.data;
    }
}
