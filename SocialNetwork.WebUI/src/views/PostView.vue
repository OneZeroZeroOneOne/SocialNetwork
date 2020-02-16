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
import { Component, Prop, Vue } from "vue-property-decorator";
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

  private commentObjs: IComment[] = [];
  private commentIds: Guid[] = [];
  private currentPage: number = 1;
  private postObj!: IPost; 

  constructor() {
    super();
    setInterval(() => this.loadComments(), 1000 * 30); //every 30 sec update

    this.loadComments()
    this.loadPost()

    this.$root.$on('footerInView', () => {
      this.throttleLoadComments();
    })
  }

  throttleLoadComments = _.throttle(() => this.loadComments(), 2000);

  postId(): string {
    if (this.$route.query.hasOwnProperty('id'))
      return this.$route.query.id.toString()
    return 'error'
  }

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;

    PostService.getPost(this.postId())
      .then(response => {
        this.postObj = response;
        this.requestPostStatus = ResponseState.success;
      })
      .catch(error => {
        this.requestPostStatus = ResponseState.fail;
      });
  }

  async loadComments()
  {
    this.requestCommentsStatus = ResponseState.loading;
    Nprogress.start()

    CommentService.getCommentForPost(this.postId(), this.currentPage, 50)
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