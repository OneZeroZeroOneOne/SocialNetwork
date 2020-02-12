<template>
<div class="comment">
  <div class="comment-body" v-if="requestStatus === 1">
    <div class="comment-header"> 
      HEADER
    </div>
    <div class="comment-content">
      {{commentObj.text}}
    </div>
    <div class="comment-footer">
      Footer
    </div>
  </div>
   <div class="comment-body" v-if="requestStatus === 0">
    <div class="comment-header"> 
      LOADING
    </div>
    <div class="comment-content">
      LOADING
    </div>
    <div class="comment-footer">
      LOADING
    </div>
  </div>
   <div class="comment-body" v-if="requestStatus === 2">
    <div class="comment-header"> 
      ERROR
    </div>
    <div class="comment-content">
      ERROR
    </div>
    <div class="comment-footer">
      ERROR
    </div>
  </div>
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
export default class CommentComponent extends Vue {
  @Prop() public commentObj!: IComment;
  private requestStatus: ResponseState = ResponseState.loading;

  constructor() {
    super();
    this.requestStatus = ResponseState.success;
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$comment-header-height: 25px;
$comment-content-height: 75px;
$comment-footer-height: 25px;

$comment-offset-left: 10px;
$comment-offset-between: 6px;

$comment-body-color: #192734;
$comment-body-border-color: rgb(56, 68, 77);
$comment-header-border-color: cornflowerblue;
$comment-footer-border-color: rgb(76, 96, 116);

$header-text-color: #6995c5;
$text-color: #ccc;

.comment {
  color: $text-color;
  margin: 0 0px $comment-offset-between $comment-offset-left;
  background-color: $comment-body-color;
  width: 80%;
  min-height: 100px;
  border: 2px solid;
  border-left-width: 4px;
  border-color: $comment-body-border-color;
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;

  .comment-header {
    max-width: 100%;
    min-height: $comment-header-height;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $comment-header-border-color;
    padding-left: 10px;
    padding-top: 5px;
  }
  .comment-content {
    max-width: calc(100% - 10px);
    min-height: $comment-content-height;
    padding-left: 10px;
    padding-top: 5px;
  }
  .comment-footer {
    padding-left: 10px;
    min-height: $comment-footer-height;
    border-top-style: solid;
    border-top-width: 2px;
    border-top-color: $comment-footer-border-color;
  }
}
</style>
