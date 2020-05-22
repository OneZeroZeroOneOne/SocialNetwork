import { IComment } from '@/models/responses/CommentViewModel';
import { IPost } from '@/models/responses/PostViewModel';

export interface IIDBService {
    Connect(): Promise<void>;
    getComment(comment_id: number): Promise<IComment|undefined>;
    getPost(post_id: number): Promise<IPost|undefined>;
    addPosts(posts: IPost[]): Promise<void>;
    addComments(comments: IComment[]): Promise<void>;
}