<template>
  <div class="post" :id="obj.id" @mouseover="hovered = true" @mouseleave="hovered = false" v-bind:style="modalStylesCalc()">
    <div class="post-body">
      <div class="post-header">
        <div class="post-header-title">
          {{obj.title}}
        </div>
        <div class="post-header-time">
          {{obj.date | formatDate}}
        </div>
        <a class="post-header-link"
          :href="'/'+boardName()+'/'+obj.id"
          target="_blank"
          rel="noopener noreferrer"
          @click.self="openEditor">
        #{{obj.id}}
        </a>
        <div class="post-header-number">
          1
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
          <a v-for="(mention, index) in obj.mention" v-bind:key="index"
            target="_blank" 
            class="link-to" 
            rel="noopener noreferrer"
            :href="'/'+boardName()+'/'+obj.id"
            :data-thread="obj.postId" 
            :data-comment="mention.isComment ? mention.mentionerId : undefined"
            :data-post="!mention.isComment ? mention.mentionerId : undefined">>>{{mention.mentionerId}}</a>
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

  @Prop() public modalStyles!: any;

  public timer: number = -1;
  public counter: number = 5;
  public hovered: boolean = true;
  public countdown!: any; 

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
      /*this.modalStyles.left = this.position.x + 'px';
      this.modalStyles.top = this.position.y + 'px';*/

      this.timer = setTimeout(this.end, 3 * 1000);
      clearTimeout(this.timer);
      this.hovered = false;
    }else{
      //this is not modal so...
    }
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

.post-body {
  position: relative;
  color: $text-color;
  background-color: $post-body-color;
  width: 90%;
  height: auto;
  border: 2px solid;
  border-color: $post-body-border-color;
  border-bottom-width: 4px; 
  border-bottom-right-radius: 4px;
  border-bottom-left-radius: 4px;
  float: left;
  margin-right: 20px;
  margin-left: 0px;

  .post-content-body {
    //margin-top: 10px;
    max-height: 50vh;
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
      color: var(--post-header-number-color);
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
      color: var(--post-header-text-link);
      text-decoration: none;
      &:hover {
        cursor: pointer;
        color: var(--post-header-text-hover-link);
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
