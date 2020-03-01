import axios, { AxiosStatic, AxiosResponse } from 'axios';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPagedResult } from '@/models/responses/PagedResult';

export class CommentService {
    private static commentAxios = axios.create();
    
    static async sendComment(comment: any, attachmentList: number[]): Promise<AxiosResponse<IComment>> {
        let url = 'http://16ch.tk/api/social/Comments';
        console.log(comment, attachmentList)
        return (await this.commentAxios.post<IComment>(url, {
            text: comment.text,
            postId: comment.postId,
            attachmentList: attachmentList,
        }))
    }

    static async getCommentForPost(link_to_post: string, page: number = 1, quantity: number = 5): Promise<IPagedResult<IComment>> {
        let url = 'http://16ch.tk/api/social/Comments/Page/' + link_to_post + "?page=" + page.toString() + "&quantity=" + quantity.toString();
        return (await this.commentAxios.get<IPagedResult<IComment>>(url)).data;
    }
}
