<template>
  <div class="post">
    <div class="post-body">
      <div class="post-header">
        <div class="post-header-link"
          @click.self="openEditor()">
        >>{{postObj.id}}
        </div>
        <div class="post-header-title">
          {{postObj.title}}
        </div>
        <div class="post-header-time">
          {{postObj.date | formatDate}}
        </div>
      </div>
      <div class="post-content" :style="stylesContent()">
        <div class=post-content-header v-if="postObj.attachmentPost.length > 0" :style="stylesContentHeader()">
          <attachment-component :attachmentObjs="postObj.attachmentPost"/>
        </div>
        <div class=post-content-body v-html="postObj.text">
        </div>
      </div>
      <div class="post-footer">
        <div v-on:click="goToPost" class="post-footer-enter button noselect" v-if="showEnterButton === true">
          To thread
        </div>
        <div v-on:click="openEditor" class="post-footer-reply button noselect">
          Reply
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";

import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IComment } from '../models/responses/CommentViewModel';
import { IAttachment } from '../models/responses/Attachment';
import { IPost } from "@/models/responses/PostViewModel";

import AttachmentComponent from '../components/AttachmentComponent.vue';
import CommentComponent from "./CommentComponent.vue";

import Nprogress from "nprogress"
import _ from 'lodash'

@Component({
  components: {
    AttachmentComponent,
  }
})
export default class PostComponent extends Vue {
  @Prop() public postObj!: IPost; 
  @Prop() public postNum!: number;
  @Prop() public showEnterButton!: boolean;
  
  constructor() {
    super();
  }

  openEditor(): void {
    this.$root.$emit('show-editor-modal-from-post', this.postObj)
  }

  stylesContent(): string {
    if (this.postObj.attachmentPost.length === 1)
      return "display: inline-flex;padding-right: 10px;"
    
    return ""
  }

  stylesContentHeader(): string {
    if (this.postObj.attachmentPost.length === 1)
      return "padding-right: 10px;align-items: center;"
    
    return ""
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  goToPost(): void {
    this.$router.push({name: 'post', params: { board: this.boardName(), postid: this.postObj.id.toString()}})
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

  .post-content-body {
    margin-top: 10px;
    min-height: 50px;
    word-break: break-all;
  }

  .post-header {
    min-height: $post-header-height;
    color: $header-text-color;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $post-header-border-color;
    padding-left: 10px;
    padding-top: 5px;
    .post-header-time {
      color: $text-color;
      float: right;
      margin-right: 10px;
    }
    .post-header-link {
      color: $text-color;
      float: left;
      //margin-right: 10px;
      color: orange;
      &:hover {
        cursor: pointer;
        color: orangered;
      }
    }
    .post-header-title {
      float: left;
      padding-left: 10px;
    }
  }
  .post-content {
    max-width: calc(100% - 10px);
    min-height: $post-content-height;
    padding-left: 10px;
    padding-top: 5px;
    padding-bottom: 5px;
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
