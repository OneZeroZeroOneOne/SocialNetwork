<template>
  <div class="post">
    <div class="post-body" v-if="requestPostStatus === 1">
      <div class="post-header">
        Header
      </div>
      <div class="post-content" >
        Post Text: {{postObj.text}}
      </div>
      <div class="post-footer">
        Footer
      </div>
    </div>
    <div class="post-body" v-if="requestPostStatus === 0">
      <div class="post-header">
        loading
      </div>
      <div class="post-content" >
        loading
      </div>
      <div class="post-footer">
        loading
      </div>
    </div>
    <div class="post-body" v-if="requestPostStatus === 2">
      <div class="post-header">
        ERROR
      </div>
      <div class="post-content" >
        ERROR
      </div>
      <div class="post-footer">
        ERROR
      </div>
    </div>
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
import CommentComponent from "./CommentComponent.vue";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";
import { IPost } from "@/models/responses/PostViewModel";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';
import Nprogress from "nprogress"
import 'nprogress/nprogress.css';

@Component({
  components: { 
    CommentComponent
  }
})
export default class PostComponent extends Vue {
  @Prop() public postId!: string;
  private postObj!: IPost; 

  private commentObjs: IComment[] = [];
  private currentPage: number = 1;

  private requestPostStatus: ResponseState = ResponseState.loading;
  private requestCommentsStatus: ResponseState = ResponseState.loading;

  constructor() {
    super();
    Nprogress.start()
    this.loadPost();
    Nprogress.set(0.5)
    this.loadComments();
    Nprogress.done();
  }
  

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;

    PostService.getPost(this.postId)
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

    CommentService.getCommentForPost(this.postId, this.currentPage, 3)
      .then(response => {
        console.log(response.results)
        console.log(this.commentObjs)
        this.commentObjs = this.commentObjs.concat(response.results);
        this.requestCommentsStatus = ResponseState.success;
        this.currentPage += 1;
        console.log(this.commentObjs)
      })
      .catch(error => {
        this.requestCommentsStatus = ResponseState.fail;
      })
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$post-header-height: 25px;
$post-content-height: 100px;
$post-footer-height: 45px;

$post-body-color: rgb(21, 32, 43);
$post-body-border-color: rgb(56, 68, 77);
$post-header-border-color: cornflowerblue;

$header-text-color: #6995c5;
$text-color: #ccc;

.post-body {
  position: relative;
  color: $text-color;
  background-color: $post-body-color;
  width: 80%;
  min-height: 150px;
  height: auto;
  border: 2px solid;
  border-color: $post-body-border-color;
  border-bottom-width: 4px; 
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  float: left;
  margin: 20px;

  .post-header {
    max-width: calc(100% - 50px);
    min-height: $post-header-height;
    color: $header-text-color;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $post-header-border-color;
    padding-left: 10px;
    padding-top: 5px;
  }
  .post-content {
    max-width: calc(100% - 10px);
    min-height: $post-content-height;
    padding-left: 10px;
    padding-top: 5px;
  }
  .post-footer {
    padding-left: 10px;
    min-height: $post-footer-height;
    border-top-style: solid;
    border-top-width: 2px;
    border-top-color: cornflowerblue;
  }

}

#comments {
  width: 80%;
  float: left;
  margin-left: 40px;
}
</style>
