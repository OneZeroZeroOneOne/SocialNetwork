<template>
  <div class="post" :id="obj.id" @mouseover="hovered = true" @mouseleave="hovered = false" v-bind:style="modalStylesCalc()">
    <div class="post-body">
      <div class="post-header">
        <div class="post-header-number">
          OP
        </div>
        <div class="post-header-title">
          {{obj.title}}
        </div>
        <a class="post-header-link"
          :href="'/'+boardName()+'/'+obj.id"
          target="_blank"
          rel="noopener noreferrer"
          @click.self="openEditor">
        #{{obj.id}}
        </a>
        <div class="post-header-time">
          {{obj.date | formatDate}}
        </div>
      </div>
      <div class="post-content" :style="stylesContent()">
        <div class=post-content-header v-if="obj.attachmentPost.length > 0" :style="stylesContentHeader()">
          <attachment-component :attachmentObjs="obj.attachmentPost"/>
        </div>
        <div v-bar>
          <article class=post-content-body v-html="obj.text">
          </article>
        </div>
      </div>
      <div class="post-footer" v-if="obj.mention.length > 0">
        <div class="post-footer-mentions">
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

import AttachmentComponent from '@/components/Other/AttachmentComponent.vue';

import eventBus from "@/utilities/EventBus";
import GlobalStorage from '../../services/Implementations/GlobalStorage';
import animateCSS from '@/utilities/AnimateCSS';

@Component({
  components: {
    AttachmentComponent,
  }
})
export default class PostComponent extends Vue {
  @Prop() public obj!: IPost; 
  @Prop() public postNum!: number;
  @Prop() public showEnterButton!: boolean;
  
  @Prop() public isModal!: boolean;

  @Prop() public keyId!: number;

  public modalStyles: string = "";
  @Prop() public event!: MouseEvent;

  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any; 
  public corner: string;
  public side: string;

  constructor() {
    super();
  }

  @Watch('hovered', {immediate: true})
  change(value) {
    if (this.isModal)
    {
      if (value === true)
      {
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
      this.calculatePosition();

      this.timer = setTimeout(this.end, 3 * 1000);
      clearTimeout(this.timer);
      this.hovered = false;
    }else{
      //this is not modal so...
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

    let relY = linkElementY + coords.height + (this.obj.attachmentPost.length > 0 ? 250 : 20 );
    let relX = linkElementX + coords.width  + (this.obj.attachmentPost.length > 0 ? 200 : 20 );

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

  boardName(): any {
    let b = GlobalStorage.getBoardById(this.obj.boardId)
    if (b !== undefined)
      return b.name
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

$post-body-color: var(--post-body-background-color);
$post-body-border-color: var(--post-body-border-color);
$post-header-border-color: var(--post-header-border-color);

$header-text-color: var(--post-header-border-text-color);
$text-color: var(--post-text-color);

.post {
  display: flow-root;
}

.post-body {
  position: relative;
  color: $text-color;
  background-color: $post-body-color;
  min-width: 25vw;
  width: 90%;
  height: auto;
  border: 2px solid;
  border-color: $post-body-border-color;
  border-bottom-width: 4px; 
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  float: left;

  .post-content-body {
    //margin-top: 10px;
    max-height: 50vh;
    padding-left: 5px;
    min-height: 50px;
    word-break: break-all;
  }

  .post-header {
    display: flow-root;
    color: $header-text-color;
    border-bottom-style: solid;
    border-bottom-width: 2px;
    border-bottom-color: $post-header-border-color;
    padding-top: 2px;
    padding-bottom: 2px;
    font-size: var(--comment-header-font-size);
    .post-header-number {
      padding-left: 5px;
      float: left;
      color: var(--post-header-number-color);
    }

    .post-header-time {
      color: $text-color;
      float: right;
    }
    .post-header-link {
      color: $text-color;
      float: right;
      margin-right: 5px;
      margin-left: 5px;
      color: var(--post-header-text-link);
      text-decoration: none;
      &:hover {
        cursor: pointer;
        color: var(--post-header-text-hover-link);
      }
    }
    .post-header-title {
      float: left;
      padding-left: 5px;
    }
  }
  .post-content {
    max-width: calc(100% - 10px);
    padding-left: 5px;
    padding-top: 5px;
    padding-bottom: 5px;
  }
  .post-footer {
    padding-left: 10px;
    border-top-style: solid;
    border-top-width: 2px;
    border-top-color: var(--post-footer-border-color);

    &-mentions {
      padding-top: 3px;
      padding-bottom: 3px;
      font-size: small;

      .link-to {
        padding-left: 3px;
      }
    }
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
