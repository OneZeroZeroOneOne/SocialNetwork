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
      <ul id="comments" v-if="requestCommentsStatus === 1">
        <li v-for="item in commentObjs.results" v-bind:key="item.id">
          <CommentComponent :commentObj="item"/>
        </li>
      </ul>
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


@Component({
  components: { 
    CommentComponent
  }
})
export default class PostComponent extends Vue {
  @Prop() public postId!: string;
  private postObj!: IPost; 

  private commentObjs!: IPagedResult<IComment>;

  private requestPostStatus: ResponseState = ResponseState.loading;
  private requestCommentsStatus: ResponseState = ResponseState.loading;

  constructor() {
    super();
    this.loadPost();
  }

  async loadPost(): Promise<void> {
    this.requestPostStatus = ResponseState.loading;
    this.requestCommentsStatus = ResponseState.loading;

    PostService.getPost(this.postId)
      .then(response => {
        this.postObj = response;
        this.requestPostStatus = ResponseState.success;
      })
      .catch(error => {
        this.requestPostStatus = ResponseState.fail;
      });
    

    CommentService.getCommentForPost(this.postId)
      .then(response => {
        this.commentObjs = response;
        this.requestCommentsStatus = ResponseState.success;
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

.post-body {
  position: relative;
  color: #eff8f8;
  background-color: #706c61;
  width: 80%;
  min-height: 150px;
  height: auto;
  border: 2px solid;
  border-color: #333333;
  border-bottom-width: 4px; 
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  float: left;
  margin: 20px;

  .post-header {
    max-width: calc(100% - 10px);
    min-height: $post-header-height;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: cornflowerblue;
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
