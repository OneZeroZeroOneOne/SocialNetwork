import { IBoardService } from "../Abstractions/IBoardService";
import { BoardService } from './BoardService';
import { IPost } from '@/models/responses/PostViewModel';
import { Guid } from '@/utilities/guid';
import IDBService from './IDBService';
import { IPagedResult } from '@/models/responses/PagedResult';
import { ResponseModel } from '@/models/responses/ResponseModel';
import { ResponseState } from '@/models/enum/ResponseState';
import { IComment } from '@/models/responses/CommentViewModel';
import { ICommentService } from '../Abstractions/ICommentService';
import { CommentService } from './CommentService';
import { IPostService } from '../Abstractions/IPostService';
import { PostService } from './PostService';
import { IBoard } from '@/models/responses/Board';
import { IIDBService } from '../Abstractions/IIDBService';

class GlobalStorage {
    private boardService: IBoardService;
    private commentService: ICommentService;
    private postService: IPostService;

    public currentBoard: IBoard;

    private db: IIDBService;

    constructor() {
        this.boardService = new BoardService();
        this.commentService = new CommentService();
        this.postService = new PostService();

        IDBService.Connect().then()

        this.db = IDBService;
    }

    public async getBoardByName(name: string): Promise<ResponseModel<IBoard>> {
        let respModel = new ResponseModel<IBoard>();

        if (this.currentBoard !== undefined && this.currentBoard !== null)
            if (this.currentBoard.name === name)
            {
                respModel.state = ResponseState.success;
                respModel.value = this.currentBoard;
            }

        await this.boardService.getBoardByName(name)
            .then(response => {
                if (response.status == 200)
                {
                    respModel.state = ResponseState.success;
                    respModel.value = response.data;
                    this.currentBoard = response.data;
                } else {
                    respModel.state = ResponseState.success;
                }
            })
            .catch(error => {
                respModel.state = ResponseState.fail;
            });

        return respModel;
    }

    public async getComment(comment_id: string): Promise<ResponseModel<IComment>> {
        let respModel = new ResponseModel<IComment>();

        let comment = await this.db.getComment(Number(comment_id));

        if (comment !== undefined)
        {
            console.log(`from storage comment ${comment_id}`)
            respModel.state = ResponseState.success;
            respModel.value = comment;
            return respModel;
        }

        await this.commentService.getCommentById(comment_id.toString())
            .then(resp => {
                if (resp.status === 200)
                {
                    comment = resp.data;

                    this.db.addComments([comment])

                    respModel.state = ResponseState.success;
                    respModel.value = comment;
                } else {
                    respModel.state = ResponseState.success;
                }
            }).catch(err => {
                respModel.state = ResponseState.fail;
            })
        
        return respModel;
    }

    public async getPost(board_id: Guid, post_id: string): Promise<ResponseModel<IPost>> {
        let respModel = new ResponseModel<IPost>();

        let post = await this.db.getPost(Number(post_id));

        if (post !== undefined)
        {
            console.log(`from storage post ${post_id}`)
            respModel.state = ResponseState.success;
            respModel.value = post;
            return respModel;
        }

        await this.postService.getPost(board_id, post_id.toString())
            .then(response => {
                if (response.status === 200)
                {
                    post = response.data;
                    post.boardId = board_id;
                    
                    this.db.addPosts([post])

                    respModel.state = ResponseState.success;
                    respModel.value = post;
                } else {
                    respModel.state = ResponseState.success;
                }
            })
            .catch(error => {
                respModel.state = ResponseState.fail;
            });   
        
        return respModel;
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

        await this.commentService.getCommentForPost(link_to_post, page, quantity)
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