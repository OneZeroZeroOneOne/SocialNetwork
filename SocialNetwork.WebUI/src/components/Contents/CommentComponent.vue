<template>
<div :class="classNames" :id="obj.id" @mouseover="hovered = true" @mouseleave="hovered = false" v-bind:style="modalStylesCalc()">
  <div class="comment-body">
    <div class="comment-header">
      <a class="comment-header-link"
          :href="'/'+boardName()+'/'+obj.postId+'#'+obj.id"
          target="_blank"
          rel="noopener noreferrer"
          @click.self="openEditor">
        #{{obj.id}}
      </a>
      <div class="comment-header-time">
        {{obj.date | formatDate}}
      </div>
      <div class="comment-header-number">
        {{obj.seqId + 2}}
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
    <div class="comment-footer" v-if="obj.mention.length > 0">
      <div class="comment-footer-mentions">
        Ответы: 
        <a v-for="(mention, index) in obj.mention" v-bind:key="index"
          target="_blank"
          class="link-to"
          rel="noopener noreferrer"
          :href="'/'+boardName()+'/'+obj.id"
          :data-thread="obj.postId"
          :data-comment="mention.isComment ? mention.mentionerId : undefined"
          :data-post="!mention.isComment ? mention.mentionerId : undefined">>>{{mention.mentionerId}} </a>
      </div>
    </div>
  </div>
</div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";

import { IPost } from "@/models/responses/PostViewModel";
import { IComment } from '@/models/responses/CommentViewModel';

import AttachmentComponent from '@/components/Other/AttachmentComponent.vue';

import eventBus from "@/utilities/EventBus";
import GlobalStorage from '../../services/Implementations/GlobalStorage';

import animateCSS from "@/utilities/AnimateCSS";

@Component({
  components: {
    AttachmentComponent,
  }
})
export default class CommentComponent extends Vue {
  @Prop() public obj!: IComment;
  @Prop() public fatherPost!: IPost;

  @Prop() public isModal!: boolean;
  
  @Prop() public keyId!: number;

  @Prop() public event!: MouseEvent;

  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any;
  public corner: string;
  public side: string;

  public classNames: any = {
    "comment": true,
  }

  public modalStyles: string = "";

  constructor() {
    super();
  }

  boardName(): string {
    let board = GlobalStorage.getBoardById(this.fatherPost.boardId);
    if (board !== undefined)
      return board.name;
    
    return '';
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
    animateCSS(this.$el, `close_${this.corner}${this.side}`, () => {
      eventBus.emit('hide-link-component', this, this.keyId)
    }, "animation__", "close")
  }

  modalStylesCalc() {
    if (this.isModal !== undefined && this.isModal !== false) {
      return this.modalStyles;
    }
    return {};
  }

  mounted() {
    if (this.isModal !== undefined && this.isModal !== false) {
      this.calculatePosition()
        
      this.timer = setTimeout(this.end, 3 * 1000);
      clearTimeout(this.timer);
      this.hovered = false;
    }else{
      //this is not modal so...
      //animateCSS(this.$el, 'fadeInLeft')
    }
  }

  calculatePosition() {
    if (this.event.target === null)
        return;
      
    //@ts-ignore
    let coordsF: DOMRect = this.event.target.getBoundingClientRect();

    let scrW = document.body.clientWidth || document.documentElement.clientWidth;
    let scrH = window.innerHeight || document.documentElement.clientHeight;

    let coords = this.$el.getBoundingClientRect();
    //@ts-ignore
    //console.log(coords,coordsF , scrW, scrH)

    let corner = "b",
        side = "l";
    
    let scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
        scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    //console.log(scrollLeft, scrollTop);
    //console.log(coordsF.x, coordsF.y);
    
    let linkElementX = coordsF.x// + scrollLeft;
    let linkElementY = coordsF.y// + scrollTop;

    //console.log(linkElementX, linkElementY);

    let relY = linkElementY + coords.height + (this.obj.attachmentComment.length > 0 ? 250 : 20 );
    let relX = linkElementX + coords.width  + (this.obj.attachmentComment.length > 0 ? 200 : 20 );

    //console.log(relY, scrH);

    //@ts-ignore
    let xOffset = coordsF.x + this.event.target.offsetWidth / 2;

    if (relY > scrH)
    {
      corner = "b";
      this.modalStyles += `bottom: ${scrH - linkElementY - scrollTop}px;`;
      //console.log("bigger")
    }else{
      corner = "t";
      this.modalStyles += `top: ${linkElementY + scrollTop + coordsF.height}px;`;
      //console.log("smaller")
    }

    if (relX > scrW)
    {
      side = "r";
      this.modalStyles += `right: ${scrW - xOffset}px;`
      //console.log('right')
    }else{
      side = 'l';
      this.modalStyles += `left: ${xOffset}px;`
      //console.log('left')
    }

    this.side = side;
    this.corner = corner;

    animateCSS(this.$el, `open_${corner}${side}`, () => {}, "animation__", "open")
  }

  openEditor(event: MouseEvent): void {
    event.preventDefault();
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
  border: 2px solid;
  border-left-width: 4px;
  border-color: $comment-body-border-color;
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  max-width: 70vw;
  min-width: 25vw;

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
    display: flow-root;
    max-width: 100%;
    padding-top: 2px;
    padding-bottom: 2px;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $comment-header-border-color;
    .comment-header-number {
      padding-left: 5px;
      float: left;
      color: $comment-number-header-color;
    }
    .comment-header-time {
      color: $text-color;
      float: right;
      margin-right: 5px;
    }
    .comment-header-link {
      padding-right: 5px;
      color: $text-color;
      float: right;
      text-decoration: none;
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
    padding-left: 5px;
    padding-bottom: 10px;
  }
  .comment-footer {
    padding-left: 5px;
    border-top-style: solid;
    border-top-width: 2px;
    border-top-color: $comment-footer-border-color;
    
    &-mentions {
      padding-top: 3px;
      padding-bottom: 3px;
      font-size: small;
    }

    /*.comment-footer-reply {
      float: right;
      margin: 6px;
      margin-right: 10px;
    }*/
  }

}
</style>
