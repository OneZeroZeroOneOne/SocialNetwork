<template>
  <div class="home">
    <PostComponent :postId="postId()"/>
    <ul id="comments">
      <li v-for="item in commentObjs" v-bind:key="item.id">
        <CommentComponent :commentObj="item"/>
      </li>
    </ul>
    <div class="footer-end">

    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import PostComponent from '@/components/PostComponent.vue'
import CommentComponent from "@/components/CommentComponent.vue";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';
import { Guid } from "@/utilities/guid";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";
import Nprogress from "nprogress"
import _ from 'lodash'

@Component({
  components: { 
    CommentComponent,
    PostComponent
  }
})
export default class PostView extends Vue {
  private requestCommentsStatus: ResponseState = ResponseState.loading;

  private commentObjs: IComment[] = [];
  private commentIds: Guid[] = [];
  private currentPage: number = 1;

  private scrolledToBottom: boolean = false;

  constructor() {
    super();
    window.onscroll = () => {
      let bottomOfWindow = Math.max(window.pageYOffset, document.documentElement.scrollTop, document.body.scrollTop) + window.innerHeight >= document.documentElement.offsetHeight - 100
      if (bottomOfWindow) {
        this.scrolledToBottom = true // replace it with your code
        console.log("scrolled to bottom")
        this.throttleLoadComments();
      }
    }

    setInterval(() => this.loadComments(), 30000);

    this.loadComments()
  }

  throttleLoadComments = _.throttle(() => this.loadComments(), 2000);

  postId(): string {
    console.log(this.$route)
    if (this.$route.query.hasOwnProperty('id'))
      return this.$route.query.id.toString()
    return 'error'
  }

  async loadComments()
  {
    console.log("loading comments")
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
        console.log(this.commentIds);
        console.log(this.commentObjs)
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
}
</style>