<template>
  <div class="post">
    <b-field label="From Backend" v-if="requestStatus === 1">
      <label>Post Text: {{commentObj.text}}</label>
    </b-field>
    <b-field label="From Backend" v-if="requestStatus === 0">
      <label>Post Text: LOADING</label>
    </b-field>
    <b-field label="From Backend" v-if="requestStatus === 2">
      <label>Post Text: error</label>
    </b-field>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";
import { IPost } from "@/models/responses/PostViewModel";
import { ResponseState } from "@/models/enum/ResponseState";
import { IComment } from '../models/responses/CommentViewModel';

@Component({})
export default class PostComponent extends Vue {
  @Prop() public commentObj!: IComment;
  private requestStatus: ResponseState = ResponseState.loading;

  constructor() {
    super();
    this.loadPost();
    this.requestStatus = ResponseState.success;
  }

  async loadPost(): Promise<void> {

    /*PostService.getPost(this.postId)
      .then(response => {
        this.postObj = response;
        this.requestStatus = ResponseState.success;
      })
      .catch(error => {
        this.requestStatus = ResponseState.fail;
      });
    
    CommentService.getCommentForPost(this.postId)
      .then(response => {
        console.log(response)
      })
      .catch(error => {
        console.log(error)
      })
    console.log(this.postObj);*/
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
  .post {
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
</style>
