<template>
  <div class="post-view" id="thread-view">
    <top-bottom-buttons/>
    <board-name-header-component :boardObj="boardObj" v-if="requestBoardStatus === 1"/>
    <post-component :obj="postObj" v-if="requestPostStatus === 1"/>
    <div id="comments">
      <div class="comment-wrapper" v-for="item in commentObjs" v-bind:key="item.id" style="">
        <comment-component :obj="item" :fatherPost="postObj"/>
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

import BoardNameHeaderComponent from '@/components/Navigations/BoardNameHeaderComponent.vue';
import PostComponent from '@/components/Contents/PostComponent.vue'
import CommentComponent from "@/components/Contents/CommentComponent.vue";
import FooterComponent from "@/components/Navigations/FooterComponent.vue";
import TopBottomButtons from "@/components/Navigations/TopBottomButtons.vue";

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { IPostService } from '../services/Abstractions/IPostService';

import { BoardService } from '../services/Implementations/BoardService';
import { CommentService } from '../services/Implementations/CommentService';
import { PostService } from '../services/Implementations/PostService';

import eventBus from '@/utilities/EventBus';

import Nprogress from "nprogress"
import _ from 'lodash'

import globalStorage from '@/services/Implementations/GlobalStorage';

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

    Nprogress.set(0.2)
    this.loadBoardByName(this.boardName())
    .then(x => {
      Nprogress.set(0.5)
      this.loadPost()
      Nprogress.set(0.7)
      this.loadComments()
      Nprogress.done()
    });
    /*this.$root.$on('footerInView', () => {
      this.throttleLoadComments();
    })*/
  }

  mounted() {
    /*this.$nextTick(() => {
      let scrollTo = document.getElementById('345');
      console.log(scrollTo)
      if (scrollTo !== null)
        scrollTo.scrollIntoView(true);
    })*/
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

    let board = await globalStorage.getBoardByName(name);

    this.requestBoardStatus = board.state;

    if (board.state === ResponseState.fail)
    {
      this.$router.push({name: "notfound"})
      return;
    }

    this.boardObj = board.value;
  }

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;

    let resp = await globalStorage.getPost(this.boardObj.id, this.postId())

    if (resp.state === ResponseState.fail)
    {
      this.requestPostStatus = ResponseState.fail;
      this.$router.push({name: "notfound", params: {fromBoard: this.boardObj.name} })
      return;
    }

    this.postObj = resp.value;
    this.requestPostStatus = ResponseState.success;
  }

  async loadComments()
  {
    this.requestCommentsStatus = ResponseState.loading;

    let comments = await globalStorage.getCommentsForPost(this.postId(), this.currentPage, 1000, false);

    if (comments.state === ResponseState.fail)
    {
      this.requestCommentsStatus = ResponseState.fail;
      return;
    }
    
    this.requestCommentsStatus = ResponseState.success;

    if (comments.value.currentPage < comments.value.pageCount)
    {
      this.currentPage += 1;
    }

    let newComCount: number = 0;
    comments.value.results.forEach(x => {
      if (this.commentIds.indexOf(x.id) === -1)
      {
        this.commentIds.push(x.id);
        this.commentObjs.push(x);
        newComCount += 1;
      }
    })

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
}
</script>


<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
#comments {
  display: inline-flex;
  flex-direction: column;
  margin-top: 20px;
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