import axios, { AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';
import { ICommentService } from '../Abstractions/ICommentService';

export class CommentService implements ICommentService {
    private commentAxios = axios.create();
    
    async sendComment(comment: any, attachmentList: number[]): Promise<AxiosResponse<IComment>> {
        let url = 'http://16ch.tk/api/social/Comments';
        
        return (await this.commentAxios.post<IComment>(url, {
            text: comment.text,
            postId: comment.postId,
            attachmentList: attachmentList,
        }))
    }

    async getCommentForPost(link_to_post: string, page: number = 1, quantity: number = 5): Promise<AxiosResponse<IPagedResult<IComment>>> {
        let url = 'http://16ch.tk/api/social/Comments/Page/' + link_to_post + "?page=" + page.toString() + "&quantity=" + quantity.toString();
        return (await this.commentAxios.get<IPagedResult<IComment>>(url));
    }
}

