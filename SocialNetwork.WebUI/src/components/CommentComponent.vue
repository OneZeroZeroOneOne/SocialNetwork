<template>
<div class="comment" @mouseover="hovered = true" @mouseleave="hovered = false">
  <countdown :time="4 * 1000" ref="countdown" :auto-start="false" @end="end"/>
  <div class="comment-body">
    <div class="comment-header">
      <div class="comment-header-link"
          @click.self="openEditor()">
        >>{{commentObj.id}}
      </div>
      <div class="comment-header-time">
        {{commentObj.date | formatDate}}
      </div>
    </div>
    <div class="comment-content" :style="stylesContent()">
      <div class=comment-content-header v-if="commentObj.attachmentComment.length > 0" :style="stylesContentHeader()">
       <attachment-component :attachmentObjs="commentObj.attachmentComment"/>
      </div>
      <div class="comment-content-body" >
        <span v-for="block in mockMarkdown().child" :key="block.node_id">
          <component :is="getEntityDependOnTag(block.tag)" :key="block.position" :block_data="block" :all_blocks="flattenedData"/>
        </span>
      </div>
    </div>
    <div class="comment-footer">
      <div v-on:click="openEditor" class="comment-footer-reply button noselect">
        Reply
      </div>
    </div>
  </div>
</div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";

import { IPost } from "@/models/responses/PostViewModel";
import { IComment } from '@/models/responses/CommentViewModel';
import { IAttachment } from '@/models/responses/Attachment';
import { IMarkdownNode } from '@/models/responses/MarkdownNode';

import { ResponseState } from "@/models/enum/ResponseState";

import PreviewModal from '@/components/PreviewModal.vue';
import AttachmentComponent from '@/components/AttachmentComponent.vue';
import VRuntimeTemplate from "v-runtime-template";
import LinkToComponent from '@/components/MarkdownComponents/LinkToComponent.vue';
import GreenComponent from '@/components/MarkdownComponents/GreenComponent.vue';
import TextComponent from '@/components/MarkdownComponents/TextComponent.vue';
import SpoilerComponent from '@/components/MarkdownComponents/SpoilerComponent.vue';

import tagToEntity from '@/utilities/MarkdownUtilities';

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
export default class CommentComponent extends Vue {
  @Prop() public commentObj!: IComment;
  @Prop() public fatherPost!: IPost;
  @Prop() public commentNum!: number;

  @Prop() public isModal!: boolean;
  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any; 
  //<component :is="LinkToComponent" v-bind="{comment: '1', post: '0'}">1</component>
  public LinkToComponent: string = "LinkToComponent";

  public testString: string = 'hello im test';

  public testData = [
    {
      entityType: "TextComponent",
      position: 0,
      post: 0,
      comment: 0,
      text: "qwer\n",
    },
    {
      entityType: "SpoilerComponent",
      position: 2,
      post: 0,
      comment: 0,
      text: "spoiler",
    },
    {
      entityType: "LinkToComponent",
      position: 5,
      post: 12,
      comment: 0,
      text: "12",
    },
    {
      entityType: "GreenComponent",
      position: 7,
      post: 0,
      comment: 0,
      text: "qwerqwer",
    },
  ]

  public newData = '{ "node_id": 1, "parent_id": 0, "node": "Element", "tag": "p", "child": [ { "node_id": 2, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 3, "parent_id": 2, "node": "Text", "text": "there is bold" } ] }, { "node_id": 5, "parent_id": 1, "node": "Element", "tag": "ins", "child": [ { "node_id": 6, "parent_id": 5, "node": "Text", "text": "bol " }, { "node_id": 7, "parent_id": 5, "node": "Element", "tag": "ins", "child": [ { "node_id": 8, "parent_id": 7, "node": "Text", "text": "in  " }, { "node_id": 9, "parent_id": 7, "node": "Element", "tag": "linktocomponent", "attr": { "id": "999" } }, { "node_id": 10, "parent_id": 7, "node": "Element", "tag": "del", "child": [ { "node_id": 11, "parent_id": 10, "node": "Text", "text": "qwe" } ] }, { "node_id": 12, "parent_id": 7, "node": "Text", "text": " side" } ] }, { "node_id": 13, "parent_id": 5, "node": "Text", "text": " d" } ] }, { "node_id": 14, "parent_id": 1, "node": "Text", "text": " inside " }, { "node_id": 15, "parent_id": 1, "node": "Element", "tag": "linktocomponent", "attr": { "id": "34" } }, { "node_id": 16, "parent_id": 1, "node": "Text", "text": "qwe " }, { "node_id": 17, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 18, "parent_id": 17, "node": "Text", "text": "there is bold" } ] } ] }'

  public flattenedData: IMarkdownNode[] = []

  constructor() {
    super();
  }

  getEntityDependOnTag(tag: string): string {
    if (tag === undefined)
      return "TextComponent"
    return tagToEntity[tag];
  }
  
  @Watch('hovered', {immediate: true})
  change(value) {
    /*if (this.isModal)
    {
      console.log(value)
      if (value === true)
      {
        if (this.countdown !== undefined)
          this.countdown.pause()
      }else {
        if (this.countdown !== undefined)
          this.countdown.continue()
      }
    }*/
  } 

  end() {
    /*console.log(data.seconds);
    if (data.seconds === 1)
    {*/
      console.log('hide')
      eventBus.emit('hide-link-component', this)
    //}
  }

  mockMarkdown(): IMarkdownNode {
    var d: IMarkdownNode = JSON.parse(this.newData)
    console.log(d)
    this.flattenedData = this.flatten(d)
    console.log(this.flattenedData)

    return d
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
    /*let data = this.mockMarkdown()

    data.child.forEach(element => {
      console.log(element)
    });*/
  }

  mounted() {
    this.parseMarkdownToTree()
    if (this.isModal !== undefined && this.isModal !== false) {
      /*console.log(this.isModal)
      this.countdown = this.$refs.countdown;
      this.countdown.start();
      this.countdown.pause();*/
      this.counter = 3 * 10;
      this.timer = setInterval(() => {
        this.counter = this.counter - 1;
        console.log(this.counter)
        if(this.counter === 0) 
        {
          clearInterval(this.timer)
          this.end()
        }
      }, 100);
    }
  }

  wrap(text: string): string {
    return text.replace(/(\r\n|\n|\r)/gm, "<br/>");
  }

  openEditor(): void {
    this.$root.$emit('show-editor-modal-from-comment', this.commentObj, this.fatherPost)
  }

  stylesContent(): string {
    if (this.commentObj.attachmentComment.length === 1)
      return "display: inline-flex;padding-right: 10px;"
    
    return ""
  }

  stylesContentHeader(): string {
    if (this.commentObj.attachmentComment.length === 1)
      return "padding-right: 10px;align-items: center;"
    
    return ""
  }
}
</script>

<style lang="scss" scoped>
/*
.comment {
  transition: border-left 300ms ease-in-out, padding-left 300ms ease-in-out;
  &:hover {
    padding-left: 0.5rem;
    border-left: 0.5rem solid #00ff99;
  }
}*/
</style>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$comment-header-height: 25px;
$comment-content-height: 75px;
$comment-footer-height: 45px;

$comment-offset-left: 10px;
$comment-offset-between: 6px;

$comment-body-color: #192734;
$comment-body-border-color: rgb(56, 68, 77);
$comment-header-border-color: cornflowerblue;
$comment-footer-border-color: rgb(76, 96, 116);
$comment-number-header-color: #cc2c11;

$header-text-color: #6995c5;
$text-color: #ccc;

.comment-attachment-video {
  border-color: darkgray;
  border-style: dashed;
  border-width: 2px;
  box-sizing: border-box;
}

.comment {
  color: $text-color;
  margin: 0 0px $comment-offset-between $comment-offset-left;
  background-color: $comment-body-color;
  width: 100%;
  min-height: 100px;
  border: 2px solid;
  border-left-width: 4px;
  border-color: $comment-body-border-color;
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;

  .comment-content-header {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    align-items: flex-start; //flex end for images to bottom
  }

  .comment-attachment:not(:first-child) {
    margin-left: 5px;
  }

  .comment-attachment {
    margin: 5px 0px;
    grid-template-columns: 200px auto;
    align-items: center;
    cursor: pointer;
  }

  .comment-attachment img {
    border-radius: 5px;
    width: 200px;
    height: auto;
    vertical-align: middle;
  }

  .comment-content-body {
    word-break: break-word;
    margin-top: 10px;
    min-height: 50px;
  }

  .comment-header {
    max-width: 100%;
    min-height: $comment-header-height;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $comment-header-border-color;
    padding-left: 10px;
    padding-top: 5px;
    color: $comment-number-header-color;
    .comment-header-time {
      color: $text-color;
      float: right;
      margin-right: 10px;
    }
    .comment-header-link {
      color: $text-color;
      float: left;
      //margin-right: 10px;
      color: orange;
      &:hover {
        cursor: pointer;
        color: orangered;
      }
    }
  }

  .comment-content {
    max-width: calc(100% - 10px);
    min-height: $comment-content-height;
    padding-left: 10px;
    padding-bottom: 10px;
  }
  .comment-footer {
    padding-left: 10px;
    min-height: $comment-footer-height;
    border-top-style: solid;
    border-top-width: 2px;
    border-top-color: $comment-footer-border-color;
    
    .comment-footer-reply {
      float: right;
      margin: 6px;
      margin-right: 10px;
    }
  }

}
</style>
