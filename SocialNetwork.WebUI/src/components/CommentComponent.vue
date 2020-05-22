<template>
<div class="comment" @mouseover="hovered = true" @mouseleave="hovered = false" v-bind:style="modalStylesCalc()">
  <div class="comment-body">
    <div class="comment-header">
      <div class="comment-header-time">
        {{obj.date | formatDate}}
      </div>
      <div class="comment-header-link"
          @click.self="openEditor()">
        #{{obj.id}}
      </div>
      <div class="comment-header-number">
        {{commentNum + 1}}
      </div>
    </div>
    <div class="comment-content" :style="stylesContent()">
      <div class=comment-content-header v-if="obj.attachmentComment.length > 0" :style="stylesContentHeader()">
       <attachment-component :attachmentObjs="obj.attachmentComment"/>
      </div>
      <div v-bar>
        <article class="comment-content-body" v-html="obj.text">
        </article>
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

import AttachmentComponent from '@/components/AttachmentComponent.vue';

import eventBus from "@/utilities/EventBus";

@Component({
  components: {
    AttachmentComponent,
  }
})
export default class CommentComponent extends Vue {
  @Prop() public obj!: IComment;
  @Prop() public fatherPost!: IPost;
  @Prop() public commentNum!: number;

  @Prop() public isModal!: boolean;
  @Prop() public linkToFather!: Vue;
  @Prop() public modalId!: number;
  @Prop() public position!: any;
  
  @Prop() public keyId!: number;

  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any; 
  
  public LinkToComponent: string = "LinkToComponent";

  public modalStyles = {
    'position': 'absolute',
    'left':'0px',
    'top':'0px',
  }

  constructor() {
    super();
  }

  @Watch('hovered', {immediate: true})
  change(value) {
    if (this.isModal)
    {
      //console.log('hovered', value)
      if (value === true)
      {
        //console.log('deleting timer', this.timer)
        if (this.timer !== -1)
          clearTimeout(this.timer);
        // @ts-ignore
        this.timer = null;
      } else {
        this.timer = setTimeout(this.end, 3 * 1000);
        //console.log('new timer', this.timer)
      }
    }
  } 

  end() {
    eventBus.emit('hide-link-component', [this, this.keyId])
  }

  modalStylesCalc() {
    if (this.isModal !== undefined && this.isModal !== false) {
      return this.modalStyles;
    }
    return {};
  }

  mounted() {
    if (this.isModal !== undefined && this.isModal !== false) {
      this.modalStyles.left = this.position.x + 'px';
      this.modalStyles.top = this.position.y + 'px';

      this.timer = setTimeout(this.end, 3 * 1000);
      clearTimeout(this.timer);
      this.hovered = false;

    }else{
      //this is not modal so...
    }
  }

  openEditor(): void {
    this.$root.$emit('show-editor-modal-from-comment', this.obj, this.fatherPost)
  }

  stylesContent(): string {
    if (this.obj.attachmentComment.length === 1)
      return "display: inline-flex;padding-right: 10px;"
    
    return ""
  }

  stylesContentHeader(): string {
    if (this.obj.attachmentComment.length === 1)
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

$comment-body-color: var(--comment-body-background-color);
$comment-body-border-color: var(--comment-body-border-color);
$comment-header-border-color: var(--comment-header-border-color);
$comment-footer-border-color: var(--comment-footer-border-color);
$comment-number-header-color: var(--comment-header-number-color);

$header-text-color: var(--comment-header-text-color);
$text-color: var(--comment-text-color);

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
  max-width: 70vw;

  .comment-content-header {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    align-items: flex-start;
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
    white-space: pre-line;
    max-height: 50vh;
    overflow: auto;
  }

  .comment-header {
    max-width: 100%;
    min-height: $comment-header-height;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $comment-header-border-color;
    padding-left: 10px;
    padding-top: 5px;
    .comment-header-number {
      padding-right: 10px;
      float: right;
      color: $comment-number-header-color;
    }
    .comment-header-time {
      color: $text-color;
      float: left;
      margin-right: 10px;
    }
    .comment-header-link {
      color: $text-color;
      float: left;
      //margin-right: 10px;
      color: var(--comment-header-text-link);
      &:hover {
        cursor: pointer;
        color: var(--comment-header-text-hover-link);
      }
    }
  }

  .comment-content {
    max-width: calc(100% - 10px);
    padding-right: 10px;
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
