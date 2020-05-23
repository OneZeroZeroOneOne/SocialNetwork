import { AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { ICommentService } from '../Abstractions/ICommentService';
import apiClient from '@/services/Implementations/ApiClient';
import { IApiClient } from '../Abstractions/IApiClient';

export class CommentService implements ICommentService {
    private commentAxios: IApiClient = apiClient;
    
    async sendComment(comment: any, attachmentList: number[]): Promise<AxiosResponse<IComment>> {
        return this.commentAxios.post<IComment>(`social/Comments`, {
            title: comment.textTitle,
            mentionList: comment.mentionList,
            text: comment.text,
            postId: comment.postId,
            AttachmentIdList: attachmentList,
        }, {
            headers: {}
        })
    }

    async getCommentById(commentid: string | number): Promise<AxiosResponse<IComment>> {
        return this.commentAxios.get<IComment>(`social/Comments/${commentid.toString()}`, {});
    }

    async getCommentForPost(link_to_post: string, page: number = 1, quantity: number = 5, sortOrder: boolean = false): Promise<AxiosResponse<IPagedResult<IComment>>> {
        return this.commentAxios.get<IPagedResult<IComment>>(`social/Comments/Page/${link_to_post.toString()}`, {
            params: {
                page: page.toString(),
                quantity: quantity.toString(),
                sortOrder: sortOrder
            }
        });
    }
}

