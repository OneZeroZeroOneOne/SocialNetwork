<template>
  <div class="board-view">
    <board-name-header-component :boardObj="boardObj" v-if="requestBoardStatus === 1"/>
    <div id="posts" v-if="requestPostsStatus === 1">
      <div v-for="(postO, indexPost) in postObjs" v-bind:key="postO.id">
        <post-component :obj="postO" :postNum="indexPost+1" :showEnterButton="true"/>
        <div id="comments">
          <div class="comment-wrapper" v-for="(commentO, indexComment) in getPreloadedCommentForPost.filter(x => x.postId === postO.id)" v-bind:key="commentO.id">
            <comment-component :obj="commentO" :commentNum="indexComment+1" :fatherPost="postO"/>
          </div>
        </div>
      </div>
    </div>
    <footer-component/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

import { IBoard } from '@/models/responses/Board';
import { IComment } from '@/models/responses/CommentViewModel';
import { IPost } from '@/models/responses/PostViewModel';

import { Guid } from '@/utilities/guid';
import { ResponseState } from "@/models/enum/ResponseState";

import PostComponent from '@/components/PostComponent.vue'
import FooterComponent from "@/components/FooterComponent.vue";
import CommentComponent from '@/components/CommentComponent.vue'
import BoardNameHeaderComponent from '@/components/BoardNameHeaderComponent.vue';

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';

import Nprogress from "nprogress"
import _ from 'lodash'
import { CommentService } from '../services/Implementations/CommentService';
import { BoardService } from '../services/Implementations/BoardService';

@Component({
  components: { 
    PostComponent,
    FooterComponent,
    CommentComponent,
    BoardNameHeaderComponent
  }
})
export default class BoardView extends Vue {
  private requestBoardStatus: ResponseState = ResponseState.loading;
  private requestPostsStatus: ResponseState = ResponseState.loading;
  private currentBoardName: string = "";
  private boardObj!: IBoard;

  private postObjs: IPost[] = [];
  private postIds: number[] = [];
  private currentPage: number = 1;

  private preloadedComments: IComment[] = [];

  private _commentService!: ICommentService
  private _boardService!: IBoardService

  constructor() {
    super();

    this.currentBoardName = this.boardName();
    this.loadBoardByName(this.currentBoardName)
      .then(x => {
        this.loadPagePosts()
        this.$root.$on('footerInView', this.throttleLoadPosts)
      })
  }

  beforeCreate() {
    this._commentService =  new CommentService();
    this._boardService = new BoardService();
  }

  beforeDestroy() {
    this.$root.$off('footerInView', this.throttleLoadPosts)
  }

  throttleLoadPosts = _.throttle(() => this.loadPagePosts(), 2000);

  get getPreloadedCommentForPost(): IComment[] {
    //let comments = this.preloadedComments.get(postId)
    return this.preloadedComments === undefined ? [] : this.preloadedComments;
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  async loadBoardByName(name: string): Promise<void> {
    return this._boardService.getBoardByName(name)
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

  async loadPagePosts(): Promise<void> {
    Nprogress.start()
    await this._boardService.getPostsOnBoard(this.boardObj.id, this.currentPage, 10)
      .then(response => {
        if (response.status == 200)
        {
          if (response.data.currentPage < response.data.pageCount)
          {
            this.currentPage += 1;
          }

          let newPostCount: number = 0;

          let newPostIds: number[] = []
          response.data.results.forEach(x => {
            if (this.postIds.indexOf(x.id) === -1)
            {
              x.boardId = this.boardObj.id;
              this.postIds.push(x.id);
              this.postObjs.push(x);
              newPostIds.push(x.id);
              newPostCount += 1;
            }
          })

          /*
          preload comments
          */
          newPostIds.forEach(x => {
            let resp = this._commentService.getCommentForPost(x.toString(), 1, 5)
            .then(resp => {
              this.preloadedComments.push(...resp.data.results)
            })
          })
          /* 
          */
          this.$awn.info(
            newPostCount === 0 ? 
            'No new posts' : 
            'Loaded ' + newPostCount.toString() + " posts", {
              durations: {
                info: 1500
              }
            })

          this.requestPostsStatus = ResponseState.success
        } else {
          this.requestPostsStatus = ResponseState.fail;
          this.$router.push({name: "notfound"})
        }
      })
      .catch(error => {
        this.requestPostsStatus = ResponseState.fail;
        this.$router.push({name: "notfound"})
      });
    Nprogress.done();
  }
}
</script>

<style lang="scss" scoped>
.foo {
	position: absolute;
	left: 0;
	bottom: 0;
	width: 100%;
	height: 80px;
}

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