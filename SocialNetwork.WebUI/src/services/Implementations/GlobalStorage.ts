import { IBoardService } from "../Abstractions/IBoardService";
import { BoardService } from './BoardService';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';
import IDBSrvice from './IDBService';
import { IPagedResult } from '@/models/responses/PagedResult';
import { ResponseModel } from '@/models/responses/ResponseModel';
import { ResponseState } from '@/models/enum/ResponseState';
import { IComment } from '@/models/responses/CommentViewModel';
import { ICommentService } from '../Abstractions/ICommentService';
import { CommentService } from './CommentService';

class GlobalStorage {
    private boardService: IBoardService;
    private commentService: ICommentService;
    private db: IDBSrvice;

    constructor() {
        this.boardService = new BoardService();
        this.commentService = new CommentService();

        IDBSrvice.GetDb().then(x => {
            this.db = x;
        })
    }

    public async getTopPostsOnBoard(boardId: Guid, page: number, quantity: number): Promise<ResponseModel<IPagedResult<IPost>>> {
        let respModel = new ResponseModel<IPagedResult<IPost>>();

        await this.boardService.getPostsOnBoard(boardId, page, quantity)
            .then(async response => {
                if (response.status == 200)
                {
                    await this.db.addPosts(response.data.results);
                    
                    respModel.state = ResponseState.success;
                    respModel.value = response.data;
                } else {
                    respModel.state = ResponseState.fail;
                }
            })
            .catch(error => {
                respModel.state = ResponseState.fail;
            });

        return respModel;
    }

    public async getCommentsForPost(link_to_post: string, page: number, quantity: number): Promise<ResponseModel<IPagedResult<IComment>>> {
        let respModel = new ResponseModel<IPagedResult<IComment>>();

        await this.commentService.getCommentForPost(link_to_post, 1, 5)
            .then(async resp => {
                if (resp.status == 200)
                {
                    await this.db.addComments(resp.data.results);

                    respModel.state = ResponseState.success;
                    respModel.value = resp.data;
                } else {
                    respModel.state = ResponseState.fail;
                }
            }).catch(err => {
                respModel.state = ResponseState.fail;
            })

        return respModel;
    }
}

export default new GlobalStorage();