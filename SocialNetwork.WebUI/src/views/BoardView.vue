<template>
  <div class="board-view">
    <div class="board-name-description" v-if="requestBoardStatus == 1">
      <div class="board-name">/{{boardObj.name}}/</div> - <div class="board-description">{{boardObj.description}}</div>
    </div>
    <ul id="posts" v-if="requestPostsStatus === 1">
      <li v-for="(postO, indexPost) in postObjs" v-bind:key="postO.id">
        <PostComponent :postObj="postO" :postNum="indexPost+1" :showEnterButton="true"/>
        <ul id="comments">
          <li v-for="(commentO, indexComment) in getPreloadedCommentForPost.filter(x => x.postId === postO.id)" v-bind:key="commentO.id">
            <CommentComponent :commentObj="commentO" :commentNum="indexComment+1"/>
          </li>
        </ul>
      </li>
    </ul>
    <li v-if="postObjs.length > 0">
      <FooterComponent/>
    </li>
    <li v-else>
      <FooterComponent class="foo"/>
    </li>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Getter } from 'vuex'
import { IBoard } from '../models/responses/Board';
import { Guid } from '../utilities/guid';
import { BoardService } from '../services/BoardService';
import { ResponseState } from "@/models/enum/ResponseState";
import { IPost } from '../models/responses/PostViewModel';
import PostComponent from '../components/PostComponent.vue'
import FooterComponent from "@/components/FooterComponent.vue";
import Nprogress from "nprogress"
import _ from 'lodash'
import { Dictionary } from 'vue-router/types/router';
import { IComment } from '../models/responses/CommentViewModel';
import { CommentService } from '../services/CommentService';
import CommentComponent from '../components/CommentComponent.vue'

@Component({
  components: { 
    PostComponent,
    FooterComponent,
    CommentComponent,
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

  constructor() {
    super();
    this.currentBoardName = this.boardName();
    this.loadBoardByName(this.currentBoardName)
      .then(x => {
        this.loadPagePosts()
        /*.then(xx => {
          this.preloadComments()
        })*/
      })
    
    this.$root.$on('footerInView', () => {
      this.throttleLoadPosts();
    })
  }

  throttleLoadPosts = _.throttle(() => this.loadPagePosts(), 2000);

  thisPostId(postId: number) {

  }

  get getPreloadedCommentForPost(): IComment[] {
    //let comments = this.preloadedComments.get(postId)
    console.log(this.preloadedComments)
    return this.preloadedComments === undefined ? [] : this.preloadedComments;
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  async preloadComments(): Promise<void> {
    this.postIds.forEach(x => {
      let resp = CommentService.getCommentForPost(x.toString(), 1, 5);
    })
  }

  async loadBoardByName(name: string): Promise<void> {
    await BoardService.getBoardByName(name)
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
    await BoardService.getPostsOnBoard(this.boardObj.id, this.currentPage, 10)
      .then(response => {
        if (response.status == 200)
        {
          if (response.data.currentPage < response.data.pageCount)
          {
            this.currentPage += 1;
          }

          let newPostCount: number = 0;
          console.log(response.data)

          let newPostIds: number[] = []
          response.data.results.forEach(x => {
            if (this.postIds.indexOf(x.id) === -1)
            {
              this.postIds.push(x.id);
              this.postObjs.push(x);
              newPostIds.push(x.id);
              newPostCount += 1;
            }
          })

          /*
          preload comments
          */
          console.log(newPostIds)
          newPostIds.forEach(x => {
            let resp = CommentService.getCommentForPost(x.toString(), 1, 5).then(resp => {
              this.preloadedComments.push(...resp.results)//.set(x, resp.results)
            })
          })
          console.log(this.preloadedComments)
          /* 
          */
          this.$notify({
            group: 'foo',
            title: 'Loaded posts',
            text: newPostCount === 0 ? 
              'No new posts' : 
              'Loaded ' + newPostCount.toString() + " posts",
          });

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

.board-name-description {
  margin-top: 10px;
  color: white;
  text-align: center;
  font-size: 4em;  
  .board-description {
    display: inline;
    color: orange;
  }
  .board-name {
    display: inline;
    color: orangered;
  }

}
#comments {
  width: 80%;
  float: left;
  margin-left: 40px;
  padding-bottom: 2.5rem;
}

</style>