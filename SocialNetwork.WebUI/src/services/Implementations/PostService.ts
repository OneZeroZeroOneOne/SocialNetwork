import { AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';
import { IPostService } from '../Abstractions/IPostService';
import apiClient from '@/services/Implementations/ApiClient';
import { IApiClient } from '../Abstractions/IApiClient';

export class PostService implements IPostService {
    
    private postsAxios: IApiClient = apiClient;

    async getPost(board_id: Guid, post_id: string): Promise<AxiosResponse<IPost>> {
        return this.postsAxios.get<IPost>(`social/Posts/${board_id.toString()}/${post_id.toString()}/`)
    }

    async getPostGlobal(post_id: string|number): Promise<AxiosResponse<IPost>> {
        return this.postsAxios.get<IPost>(`social/Posts/${post_id.toString()}/`)
    }

    async sendNewPost(post: any, attachmentList: number[]): Promise<AxiosResponse<IPost>> {
        return this.postsAxios.post<IPost>(`social/Posts`, {
            title: post.title,
            mentionList: post.mentionList,
            text: post.text,
            boardId: post.boardId,
            AttachmentIdList: attachmentList,
        }, {
            headers: {}
        })
    }
}
