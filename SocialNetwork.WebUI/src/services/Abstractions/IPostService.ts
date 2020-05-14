import { AxiosResponse } from 'axios';
import { IPost } from '@/models/responses/PostViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { Guid } from '@/utilities/guid';

export interface IPostService {
    getPost(board_id: Guid, post_id: string): Promise<AxiosResponse<IPost>>;
    getPostGlobal(post_id: string|number): Promise<AxiosResponse<IPost>>;

    sendNewPost(post: any, attachmentList: number[]): Promise<AxiosResponse<IPost>>;
}