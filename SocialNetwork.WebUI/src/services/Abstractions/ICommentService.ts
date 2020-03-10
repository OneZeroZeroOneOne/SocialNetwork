import { AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';

export interface ICommentService {
    sendComment(comment: any, attachmentList: number[]): Promise<AxiosResponse<IComment>>;
    getCommentById(commentid: string | number): Promise<AxiosResponse<IComment>>;
    getCommentForPost(link_to_post: string, page: number, quantity: number): Promise<AxiosResponse<IPagedResult<IComment>>>;
}