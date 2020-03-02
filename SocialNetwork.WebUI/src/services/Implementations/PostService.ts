import { AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';
import { IPostService } from '../Abstractions/IPostService';
import apiClient from '@/services/Implementations/ApiClient';

export class PostService implements IPostService {
    private postsAxios = apiClient;

    async getPost(board_id: Guid, post_id: string): Promise<AxiosResponse<IPost>> {
        return this.postsAxios.get<IPost>(`social/Posts/${board_id.toString()}/${post_id.toString()}/`)
    }
}
