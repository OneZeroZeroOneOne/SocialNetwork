<template>
  <div class="board-view">
    <board-name-header-component :boardObj="boardObj" v-if="requestBoardStatus === 1"/>
    <div id="posts" v-if="requestPostsStatus === 1">
      <div v-for="(postO, indexPost) in postObjs" v-bind:key="postO.id" class="post-wrapper">
        <post-component :obj="postO" :postNum="indexPost+1" :showEnterButton="true"/>
        <div id="comments">
          <div class="comment-wrapper" v-for="commentO in allComments.filter(x => x.postId === postO.id).reverse()" v-bind:key="commentO.id">
            <comment-component :obj="commentO" :fatherPost="postO"/>
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
import { IPagedResult } from "@/models/responses/PagedResult";


import PostComponent from '@/components/Contents/PostComponent.vue'
import FooterComponent from "@/components/Navigations/FooterComponent.vue";
import CommentComponent from '@/components/Contents/CommentComponent.vue'
import BoardNameHeaderComponent from '@/components/Navigations/BoardNameHeaderComponent.vue';

import Nprogress from "nprogress"
import _ from 'lodash'

import globalStorage from '@/services/Implementations/GlobalStorage';

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

  private perPage: number = 3;

  private allComments: IComment[] = []

  constructor() {
    super();
  }

  async created() {
    this.currentBoardName = this.boardName();
    Nprogress.set(0.3)
    this.loadBoardByName(this.currentBoardName)
      .then(async x => {
        Nprogress.set(0.6)
        await this.loadPagePosts()
        Nprogress.done()
        this.$root.$on('footerInView', this.throttleLoadPosts)
        document.title = `/${this.currentBoardName}/ - ${this.boardObj.description}`
      })
  }

  beforeCreate() {
    //
  }

  beforeDestroy() {
    this.$root.$off('footerInView', this.throttleLoadPosts)
  }

  throttleLoadPosts = _.throttle(() => this.loadPagePosts(), 2000);

  boardName(): string {
    return this.$route.params.boardname;
  }

  async loadBoardByName(name: string): Promise<void> {
    let board = await globalStorage.getBoardByName(name)
    
    this.requestBoardStatus = board.state;
    if (this.requestBoardStatus === ResponseState.fail)
    {
      this.$router.push({name: "notfound"});
      return;
    }
    
    this.boardObj = board.value;
  }

  async loadPagePosts(): Promise<void> {
    let posts = await globalStorage.getTopPostsOnBoard(this.boardObj.id, this.currentPage, 10);
    
    if (posts.state === ResponseState.fail)
    {
      this.requestPostsStatus = ResponseState.fail;
      return;
    } 

    let newPostCount: number = 0;

    let newPostIds: number[] = []
    posts.value.results.forEach(x => {
      if (this.postIds.indexOf(x.id) === -1)
      {
        this.postIds.push(x.id);
        this.postObjs.push(x);
        newPostIds.push(x.id);
        newPostCount += 1;
        
        globalStorage.getCommentsForPost(x.id.toString(), 1, this.perPage, true)
          .then(com => {
            if (com.state != ResponseState.fail)
            {
              this.allComments.push(...com.value.results);
              this.requestPostsStatus = ResponseState.success
            }
          })
      }
    })
  
    this.$awn.info(
      newPostCount === 0 ? 
      'No new posts' : 
      `Loaded ${newPostCount.toString()} posts`, {
        durations: {
          info: 1500
        }
    })

  }
}
</script>


<style lang="scss" scoped>

#posts {
  display: flex;
  flex-direction: column;

  .post-wrapper {
    margin-bottom: 2.0em;
  }
}

#comments {
  display: inline-flex;
  flex-direction: column;
  //width: 80%;
  float: left;
  margin-top: 20px;
  //margin-left: 40px;
}


.comment-wrapper {
  display: inline-flex;
  width: fit-content; 
  min-width: 30vw;
}

</style>