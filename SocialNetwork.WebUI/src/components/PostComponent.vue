<template>
  
  <div class="post">
    <div class="post-body">
    <b-field label="From Backend" v-if="requestPostStatus === 1">
      <label>Post Text: {{postObj.text}}</label>
    </b-field>
    <b-field label="From Backend" v-if="requestPostStatus === 0">
      <label>Post Text: LOADING</label>
    </b-field>
    <b-field label="From Backend" v-if="requestPostStatus === 2">
      <label>Post Text: error</label>
    </b-field>
    </div>
    <ul id="comments" v-if="requestCommentsStatus === 1">
    <li class="comment" v-for="item in commentObjs.results" v-bind:key="item.id">
      {{ item.text }}
    </li>
  </ul>
  </div>

</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";
import { IPost } from "@/models/responses/PostViewModel";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';

@Component({})
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
  .post-body {
    padding-left: 10px;
    padding-top: 5px;
    background-color: rgb(185, 182, 6);
    width: 80%;
    min-height: 150px;
    height: auto;
    border: 4px ridge;
    border-color: rgb(165, 162, 9);
    border-width: 25;

    float: left;
    margin: 20px;
  }

  #comments {
    width: 80%;
    float: left;
    margin-left: 40px;
  }
  .comment {
    padding-left: 10px;
    padding-top: 5px;
    margin: 0 0 5px 0;
    background-color: rgb(185, 182, 6);
    width: 80%;
    min-height: 100px;
    height: auto;
    border: 4px ridge;
    border-color: rgb(165, 162, 9);
    border-width: 25;
  }
</style>
