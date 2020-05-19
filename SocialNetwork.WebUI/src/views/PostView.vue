<template>
  <div class="post-view" id="thread-view">
    <top-bottom-buttons/>
    <board-name-header-component :boardObj="boardObj" v-if="requestBoardStatus === 1"/>
    
    <post-component :obj="postObj" v-if="requestPostStatus === 1"/>
    <div id="comments">
      <div class="comment-wrapper" v-for="(item, index) in commentObjs" v-bind:key="item.id" style="">
        <comment-component :obj="item" :commentNum="index+1" :fatherPost="postObj"/>
      </div>
    </div>
    <footer-component/>
  </div>

</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { ResponseState } from "@/models/enum/ResponseState";
import { AxiosResponse } from 'axios';

import { Guid } from "@/utilities/guid";
import { IBoard } from "@/models/responses/Board";
import { IPagedResult } from '@/models/responses/PagedResult';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPost } from "@/models/responses/PostViewModel";

import BoardNameHeaderComponent from '@/components/BoardNameHeaderComponent.vue';
import PostComponent from '@/components/PostComponent.vue'
import CommentComponent from "@/components/CommentComponent.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import TopBottomButtons from "@/components/TopBottomButtons.vue";

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { IPostService } from '../services/Abstractions/IPostService';

import { BoardService } from '../services/Implementations/BoardService';
import { CommentService } from '../services/Implementations/CommentService';
import { PostService } from '../services/Implementations/PostService';

import eventBus from '@/utilities/EventBus';

import Nprogress from "nprogress"
import _ from 'lodash'


@Component({
  components: { 
    CommentComponent,
    PostComponent,
    FooterComponent,
    BoardNameHeaderComponent,
    TopBottomButtons,
  }
})
export default class PostView extends Vue {
  private requestCommentsStatus: ResponseState = ResponseState.loading;
  private requestPostStatus: ResponseState = ResponseState.loading;
  private requestBoardStatus: ResponseState = ResponseState.loading;

  private commentObjs: IComment[] = [];
  private commentIds: number[] = [];
  private currentPage: number = 1;
  private postObj!: IPost; 

  private boardObj!: IBoard;

  private refreshInterval!: number;
  
  private _commentService!: ICommentService;
  private _postService!: IPostService;
  private _boardService!: IBoardService;

  constructor() {
    super();

    //this.refreshInterval = setInterval(() => this.loadComments(), 1000 * 30); //every 30 sec update
    this.$root.$on('comment-sent-success', this.commentSentSuccess);

    this.loadBoardByName(this.boardName())
    .then(x => {
      this.loadPost()
      this.loadComments()
    });
    /*this.$root.$on('footerInView', () => {
      this.throttleLoadComments();
    })*/
  }

  mounted() {
    
  }

  beforeDestroy() {
    this.$root.$off('comment-sent-success', this.commentSentSuccess)
  }

  async commentSentSuccess(comment: AxiosResponse<IComment>): Promise<void> {
    if (this.commentIds.indexOf(comment.data.id) === -1) {
      this.commentIds.push(comment.data.id);
      this.commentObjs.push(comment.data);
    }
  }

  beforeCreate() {
    this._boardService = new BoardService();
    this._commentService = new CommentService();
    this._postService = new PostService();
  }

  @Watch('$route',{ immediate: true}) 
  onRouteChange(obj): void {
    //console.log(obj)
    //clearInterval(this.refreshInterval)
  }

  throttleLoadComments = _.throttle(() => this.loadComments(), 2000);

  postId(): string {
    return this.$route.params.postid
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  async loadBoardByName(name: string): Promise<void> {
    this.requestBoardStatus = ResponseState.loading;
    await this._boardService.getBoardByName(name)
      .then(response => {
          if (response.status == 200)
          {
            this.boardObj = response.data;
            this.requestBoardStatus = ResponseState.success;
          } else {
            this.requestBoardStatus = ResponseState.fail;
            this.$router.push({name: "notfound"})
          }
      })
      .catch(error => {
        this.requestBoardStatus = ResponseState.fail;
        this.$router.push({name: "notfound"})
      });
  }

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;

    await this._postService.getPost(this.boardObj.id, this.postId())
      .then(response => {
        this.postObj = response.data;
        this.postObj.boardId = this.boardObj.id;
        this.requestPostStatus = ResponseState.success;
      })
      .catch(error => {
        this.requestPostStatus = ResponseState.fail;
        if (error.response.status === 400)
        {
          this.requestPostStatus = ResponseState.fail;
          this.$router.push({name: "notfound", params: {fromBoard: this.boardObj.name} })
        }
      });
  }

  async loadComments()
  {
    this.requestCommentsStatus = ResponseState.loading;
    Nprogress.start()

    await this._commentService.getCommentForPost(this.postId(), this.currentPage, 1000)
      .then(response => {
        if (response.status === 200)
        {

          if (response.data.currentPage < response.data.pageCount)
          {
            this.currentPage += 1;
          }
          this.requestCommentsStatus = ResponseState.success;
          let newComCount: number = 0;
          response.data.results.forEach(x => {
            if (this.commentIds.indexOf(x.id) === -1)
            {
              this.commentIds.push(x.id);
              this.commentObjs.push(x);
              newComCount += 1;
            }
          })
          Nprogress.done();
          this.$awn.info(
            newComCount === 0 ? 
            'No new comments' : 
            'Loaded ' + newComCount.toString() + " comments",
            {
              durations: {
                info: 1500
              }
            }
          )
        }
      })
      .catch(error => {
        this.requestCommentsStatus = ResponseState.fail;
        Nprogress.done();
      })
  }
}
</script>


<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
#comments {
  display: inline-flex;
  flex-direction: column;
  //width: 80%;
  float: left;
  //margin-left: 40px;
  padding-bottom: 2.5rem;
}

.comment-wrapper {
  display: inline-flex;
  width: fit-content; 
  min-width: 30vw;
}
</style>