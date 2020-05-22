import { DBSchema } from 'idb';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPost } from '@/models/responses/PostViewModel';

export interface MyDB extends DBSchema {
  comments: {
    key: number;
    value: IComment;
  };
  posts: {
    key: number;
    value: IPost;
  }
}
