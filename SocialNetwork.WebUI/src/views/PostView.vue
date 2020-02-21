<template>
  <div class="post-view">
    <PostComponent :postObj="postObj" v-if="requestPostStatus === 1"/>
    <ul id="comments">
      <li v-for="(item, index) in commentObjs" v-bind:key="item.id">
        <CommentComponent :commentObj="item" :commentNum="index+1"/>
      </li>
    </ul>
    <FooterComponent/>
  </div>

</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import PostComponent from '@/components/PostComponent.vue'
import CommentComponent from "@/components/CommentComponent.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';
import { IPost } from "@/models/responses/PostViewModel";
import { Guid } from "@/utilities/guid";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";
import { IBoard } from "@/models/responses/Board";
import { BoardService } from "@/services/BoardService";
import Nprogress from "nprogress"
import _ from 'lodash'

@Component({
  components: { 
    CommentComponent,
    PostComponent,
    FooterComponent
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

  constructor() {
    super();

    //this.refreshInterval = setInterval(() => this.loadComments(), 1000 * 30); //every 30 sec update

    this.loadBoardByName(this.boardName())
    .then(x => {
      this.loadPost()
      this.loadComments()
    });
    /*this.$root.$on('footerInView', () => {
      this.throttleLoadComments();
    })*/
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
    console.log(this.$route)
    return this.$route.params.boardname;
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

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;

    await PostService.getPost(this.boardObj.id, this.postId())
      .then(response => {
        this.postObj = response.data;
        this.requestPostStatus = ResponseState.success;
        console.log(response)
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

    await CommentService.getCommentForPost(this.postId(), this.currentPage, 1000)
      .then(response => {
        if (response.currentPage < response.pageCount)
        {
          this.currentPage += 1;
        }
        this.requestCommentsStatus = ResponseState.success;
        let newComCount: number = 0;
        response.results.forEach(x => {
          if (this.commentIds.indexOf(x.id) === -1)
          {
            this.commentIds.push(x.id);
            this.commentObjs.push(x);
            newComCount += 1;
          }
        })
        Nprogress.done();
        this.$notify({
          group: 'foo',
          title: 'Loaded comments',
          text: newComCount === 0 ? 
            'No new comments' : 
            'Loaded ' + newComCount.toString() + " comments",
        });
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
  width: 80%;
  float: left;
  margin-left: 40px;
  padding-bottom: 2.5rem;
}
</style>