<template>
  <div v-if="active" :class="{shakeit: shake, 'editor-modal': true}">
    <vue-draggable-resizable
            :w="width + 10" 
            :h="height" 
            :x="x"
            :y="y"
            @dragging="onDrag" 
            @resizing="onResize"
            @dragstop="dragstop"
            :disable-user-select="false"
            :resizable="false">
        <post-component :postObj="{}" :postNum="1" :showEnterButton="false"/>
    </vue-draggable-resizable>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import VueDraggableResizable from 'vue-draggable-resizable'

import { IAttachment } from '../models/responses/Attachment';
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';

import PostComponent from "@/components/PostComponent.vue";

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { CommentService } from '../services/Implementations/CommentService';
import { ResponseState } from '../models/enum/ResponseState';
import { parseNumber } from './vue-js-modal/src/parser';


@Component({
    components: {
        PostComponent
    }
})
export default class PreviewModal extends Vue {
  private submitProgress: ResponseState = ResponseState.success;

  public x: number = 0;
  public y: number = 0;

  public active: boolean = false;

  public srcPath: string = "";

  public width: number = 450;
  public height: number = 300;

  public keepInBounds: boolean = true;

  public hovered: boolean = false;

  public replyToPost!: IPost;
  public replyToComment!: IComment;

  public attachmentList: IAttachment[] = [];
  public mentionList: string[] = [];

  constructor() {
      super();
      //this.$root.$on('show-editor-modal-from-post', this.showEditorFromPost)
      //this.$root.$on('show-editor-modal-from-comment', this.showEditorFromComment)
  }

  onDrag(x, y) {
    if (x + this.width < window.innerWidth
      && x >= 0)
    {
      localStorage.setItem('modal-editor-x', x)
      this.x = x
    }else{
      this.x = window.innerWidth - this.width;
    }

    if (y + this.height < window.innerHeight
      && y >= 0)
    {
      localStorage.setItem('modal-editor-y', y)
      this.y = y
    }else{
      this.y = window.innerHeight - this.height;
    }

  }

  onResize(x, y, width, height) {
    this.x = x
    this.y = y
    this.width = width
    this.height = height
  }

  getX() {
    return this.x - this.width / 2;
  }

  getY() {
    return this.y - this.height / 2;
  }

  beforeCreate() {
    //this._commentService = new CommentService();
  }

  created(): void {
      
  }

  mounted(): void {
    
  }

}
</script>

<style lang="scss">

</style>