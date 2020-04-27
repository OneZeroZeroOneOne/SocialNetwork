<template>
  <div class="post" v-bind:style="modalStylesCalc()">
    <div class="post-body">
      <div class="post-header">
        <div class="post-header-title">
          {{obj.title}}
        </div>
        <div class="post-header-time">
          {{obj.date | formatDate}}
        </div>
        <div class="post-header-link"
          @click.self="openEditor()">
        #{{obj.id}}
        </div>
        <div class="post-header-number">
          1
        </div>
      </div>
      <div class="post-content" :style="stylesContent()">
        <div class=post-content-header v-if="obj.attachmentPost.length > 0" :style="stylesContentHeader()">
          <attachment-component :attachmentObjs="obj.attachmentPost"/>
        </div>
        <div class=post-content-body>
          <span v-for="block in parsedData.child" :key="block.node_id">
            <component :is="getEntityDependOnTag(block.tag)" :key="block.position" :block_data="block" :all_blocks="flattenedData"/>
          </span>
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
import VRuntimeTemplate from "v-runtime-template";
import LinkToComponent from '@/components/MarkdownComponents/LinkToComponent.vue';
import GreenComponent from '@/components/MarkdownComponents/GreenComponent.vue';
import TextComponent from '@/components/MarkdownComponents/TextComponent.vue';
import SpoilerComponent from '@/components/MarkdownComponents/SpoilerComponent.vue';

import tagToEntity from '@/utilities/MarkdownUtilities';

import Nprogress from "nprogress"
import _ from 'lodash'
import { IMarkdownNode } from '../models/responses/MarkdownNode';

import eventBus from "@/utilities/EventBus";

@Component({
  components: {
    AttachmentComponent,
    VRuntimeTemplate,
    LinkToComponent,
    TextComponent, 
    GreenComponent,
    SpoilerComponent,
  }
})
export default class PostComponent extends Vue {
  @Prop() public obj!: IPost; 
  @Prop() public postNum!: number;
  @Prop() public showEnterButton!: boolean;
  
  @Prop() public isModal!: boolean;
  @Prop() public linkToFather!: Vue;
  @Prop() public modalId!: number;
  @Prop() public position!: any;

  public modalStyles = {
    'position': 'absolute',
    'left':'0px',
    'top':'0px',
    'width':'80%',
  }

  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any; 

  public flattenedData: IMarkdownNode[] = []
  // @ts-ignore
  public parsedData: IMarkdownNode = {child: []};
  
  constructor() {
    super();
  }

  end() {
    /*console.log(data.seconds);
    if (data.seconds === 1)
    {*/
      // @ts-ignore
      this.linkToFather.showing = true;
      eventBus.emit('hide-link-component', this)
    //}
  }

  modalStylesCalc() {
    if (this.isModal !== undefined && this.isModal !== false) {
      return this.modalStyles;
    }
    return {};
  }

  mounted() {
    //this.parseMarkdownToTree()

    console.log(this.obj.text)
    this.parsedData = JSON.parse(this.obj.text);
    this.flattenedData = this.flatten(this.parsedData)
    console.log(this)
    console.log(this.parsedData)
    console.log(this.flattenedData)

    if (this.isModal !== undefined && this.isModal !== false) {
      /*console.log(this.isModal)
      this.countdown = this.$refs.countdown;
      this.countdown.start();
      this.countdown.pause();*/
      this.modalStyles.left = this.position.x + 'px';
      this.modalStyles.top = this.position.y + 'px';
      this.counter = 6 * 10;
      this.timer = setInterval(() => {
        this.counter = this.counter - 1;
        //console.log(this.counter)
        if(this.counter === 0) 
        {
          clearInterval(this.timer)
          this.end()
        }
      }, 100);
    }else{
      //this is not modal so...
    }
  }

  openEditor(): void {
    this.$root.$emit('show-editor-modal-from-post', this.obj)
  }

  stylesContent(): string {
    if (this.obj.attachmentPost.length === 1)
      return "display: inline-flex;padding-right: 10px;"
    
    return ""
  }

  stylesContentHeader(): string {
    if (this.obj.attachmentPost.length === 1)
      return "padding-right: 10px;align-items: center;"
    
    return ""
  }

  flatten(input: IMarkdownNode) {
    const stack = [...input.child];
    const res: IMarkdownNode[] = [];
    res.push(input);
    while(stack.length) {
      const next: IMarkdownNode|undefined = stack.pop();
        if (next !== undefined)
        {
          stack.push(...next.child);
          res.push(next);
        }
    }

    return res;
  }

  parseMarkdownToTree() {
    var d: IMarkdownNode = JSON.parse(this.obj.text);
    this.flattenedData = this.flatten(d)
    console.log(this.flattenedData)

    return d
  }

  getEntityDependOnTag(tag: string): string {
    if (tag === undefined)
      return "TextComponent"
    return tagToEntity[tag];
  }

  boardName(): string {
    return this.$route.params.boardname;
  }

  goToPost(): void {
    this.$router.push({name: 'post', params: { board: this.boardName(), postid: this.obj.id.toString()}})
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
  margin-top: 20px;
  margin-right: 20px;
  margin-bottom: 20px;
  margin-left: 0px;

  .post-content-body {
    margin-top: 10px;
    padding-left: 5px;
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
    .post-header-number {
      padding-right: 10px;
      float: right;
      color: #789922;
    }

    .post-header-time {
      color: $text-color;
      float: left;
      margin-left: 10px;
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
    }
  }
  .post-content {
    max-width: calc(100% - 10px);
    min-height: $post-content-height;
    padding-left: 5px;
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
