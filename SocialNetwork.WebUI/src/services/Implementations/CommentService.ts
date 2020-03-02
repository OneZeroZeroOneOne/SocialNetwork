import { AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { ICommentService } from '../Abstractions/ICommentService';
import apiClient from '@/services/Implementations/ApiClient';

export class CommentService implements ICommentService {
    private commentAxios = apiClient;
    
    async sendComment(comment: any, attachmentList: number[]): Promise<AxiosResponse<IComment>> {
        return this.commentAxios.post<IComment>(`social/Comments`, {
            text: comment.text,
            postId: comment.postId,
            attachmentList: attachmentList,
        }, {
            headers: {}
        })
    }

    async getCommentForPost(link_to_post: string, page: number = 1, quantity: number = 5): Promise<AxiosResponse<IPagedResult<IComment>>> {
        return this.commentAxios.get<IPagedResult<IComment>>(`social/Comments/Page/${link_to_post.toString()}`, {
            params: {
                page: page.toString(),
                quantity: quantity.toString(),
            }
        });
    }
}

