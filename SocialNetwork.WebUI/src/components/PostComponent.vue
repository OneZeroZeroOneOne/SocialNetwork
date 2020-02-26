<template>
  <div class="post">
    <div class="post-body">
      <div class="post-header">
        Header {{postObj.id}}
      </div>
      <div class="post-content" >
        <div class=post-content-header v-if="postObj.attachmentPost.length > 0">
          <div class="post-attachment" v-for="attachment in postObj.attachmentPost" v-bind:key="attachment.id">
            <img v-on:click="imgShow(attachment)" v-bind:src="getAttachmentPath(attachment.path)" />
          </div>
        </div>
        <div class=post-content-body>
          {{postObj.text}}
        </div>
      </div>
      <div class="post-footer">
        <div v-on:click="goToPost" class="post-footer-enter button noselect" v-if="showEnterButton === true">
          To thread
        </div>
        <div class="post-footer-reply button noselect">
          Reply
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";
import CommentComponent from "./CommentComponent.vue";
import { PostService } from "@/services/PostService";
import { IPost } from "@/models/responses/PostViewModel";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';
import {IAttachment} from '../models/responses/Attachment';
import Nprogress from "nprogress"
import _ from 'lodash'

@Component({})
export default class PostComponent extends Vue {
  @Prop() public postObj!: IPost; 
  @Prop() public postNum!: number;
  @Prop() public showEnterButton!: boolean;
  
  constructor() {
    super();
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  goToPost(): void {
    this.$router.push({name: 'post', params: { board: this.boardName(), postid: this.postObj.id.toString()}})
  }

  getAttachmentPath(path: string): string {
    return 'http://16ch.tk/api/attachment/' + path;
  }

  imgShow(attachment: IAttachment): void {
    this.$modal.show('preview-modal', {
      srcPath: this.getAttachmentPath(attachment.path)
    }, {
      draggable: true,
      resizable: true,
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
  width: 90%;
  min-height: 150px;
  height: auto;
  border: 2px solid;
  border-color: $post-body-border-color;
  border-bottom-width: 4px; 
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  float: left;
  margin: 20px;

  .post-attachment:not(:first-child) {
    margin-left: 5px;
  }

  .post-attachment {
    margin: 5px 0px;
    grid-template-columns: 200px auto;
    align-items: center;
    cursor: pointer;
  }

  .post-attachment img {
    border-radius: 5px;
    width: 200px;
    height: auto;
    vertical-align: middle;
  }

  .post-content-body {
    margin-top: 10px;
    min-height: 50px;
  }

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

  .post-footer-enter {
    float: right;
    margin: 6px;
    margin-right: 10px;
  }
  
  .post-footer-reply {
    float: right;
    margin: 6px;
    margin-right: 10px;
  }
}
</style>
