<template>
  <div v-if="active" :class="{shakeit: shake, 'editor-modal': true}">
    <vue-draggable-resizable
            w="300" 
            h='450'
            :x="x"
            :y="y"
            :parent="true"
            ref="container"
            @dragging="onDrag" 
            @resizing="onResize"
            @dragstop="dragstop"
            :disable-user-select="false"
            :resizable="false"
            :drag-handle="'.header-draggable'"
            :drag-cancel="'.cant-drag'">
        <div :class="'header-draggable '+isLoading()">
          <span class="cant-drag close-button" v-on:click="hide"></span>
        </div>
        <div class="editor-modal-show">
            <div class="editor">
               <editor-text 
                @editor-text-submit="submit"
                :isFileUploading="isFileUploading"
               />
            </div>
        </div>
        <div @mouseover="hovered = true" @mouseleave="hovered = false" 
        class="editor-footer-modal">
          <div class="editor-attachment">
            <attachment-drop-component
            @start-uploading="startUploading"
            @uploaded-succesfully="uploaded"/>
          </div>
        </div>
    </vue-draggable-resizable>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import VueDraggableResizable from 'vue-draggable-resizable'

import { IAttachment } from '@/models/responses/Attachment';
import { IPost } from '@/models/responses/PostViewModel';
import { IComment } from '@/models/responses/CommentViewModel';
import EditorText from '@/components/Other/EditorText.vue';

import AttachmentDropComponent from '@/components/Other/AttachmentDropComponent.vue';

import { ICommentService }from '@/services/Abstractions/ICommentService';
import { CommentService } from '@/services/Implementations/CommentService';
import { ResponseState } from '@/models/enum/ResponseState';
import { parseNumber } from '@/utilities/parser';
import { IPostService } from '@/services/Abstractions/IPostService';
import { PostService } from '@/services/Implementations/PostService';
import { IBoard } from '@/models/responses/Board';
import animateCSS from '../../utilities/AnimateCSS';

import { AxiosResponse } from 'axios';
import EventBus from '../../utilities/EventBus';

@Component({
    components: {
        AttachmentDropComponent,
        EditorText,
    }
})
export default class PreviewModal extends Vue {
  private responseState: number = 0; //1 if response to comment or thread 2 if new thread
  private submitProgress: ResponseState = ResponseState.success;

  public x: number = 0;
  public y: number = 0;

  public active: boolean = false;

  public srcPath: string = "";

  public width: number = 450;
  public height: number = 300;

  public keepInBounds: boolean = true;

  public hovered: boolean = false;

  public replyToPost!: IPost|null;
  public replyToComment!: IComment|null;
  public board!: IBoard|null;

  public shake: boolean = false;

  public attachmentList: IAttachment[] = [];
  public mentionList: string[] = [];

  private _commentService!: ICommentService;
  private _postService!: IPostService;

  private isFileUploading: boolean = false;

  constructor() {
      super();
      this.$root.$on('show-editor-modal-from-post', this.showEditorFromPost)
      this.$root.$on('show-editor-modal-from-comment', this.showEditorFromComment)
      this.$root.$on('show-editor-modal-new-thread', this.showEditorNewThread)
  }

  startUploading() {
    console.log('start')
    this.isFileUploading = true
  }

  toggleShakeAnimation(): void {
    // @ts-ignore
    animateCSS(this.$refs.container.$el, 'headShake')
  }

  showEditorNewThread(board: IBoard): void {
    console.log(board)
    if (this.responseState === 0)
    {
      this.replyToPost = null;
      this.replyToComment = null;
      this.responseState = 2;
      this.board = board;
    }
    if (this.active === false)
    {
      this.setPositionFromStorage();
      this.active = true;
    } else {  //already open
      this.toggleShakeAnimation()
    }
  }

  showEditorFromComment(com: IComment, fatherPost: IPost): void {
    this.addTextToEditor(">>" + com.id)
    if (this.responseState !== 2)
    {
      this.replyToPost = fatherPost;
      this.replyToComment = com;
      this.responseState = 1
    }
    if (this.active === false)
    {
      this.setPositionFromStorage();
      this.active = true;
    } else {  //already open
      this.toggleShakeAnimation()
    }
  }

  showEditorFromPost(post: IPost): void {
    this.addTextToEditor(">>" + post.id)
    if (this.responseState !== 2)
    {
      this.replyToPost = post;
      this.responseState = 1
    }
    if (this.active === false)
    {
      this.setPositionFromStorage();
      this.active = true;
    } else {  //already open
      this.toggleShakeAnimation()
    }
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

  dragstop(left, top) {
    this.setPositionFromStorage()
  }

  setPositionFromStorage(): void {
    this.x = localStorage.getItem('modal-editor-x') === undefined ? window.innerWidth / 2 - this.width / 2 : parseNumber(localStorage.getItem('modal-editor-x')).value
    this.y = localStorage.getItem('modal-editor-y') === undefined ? window.innerHeight / 2 - this.height / 2 : parseNumber(localStorage.getItem('modal-editor-y')).value
    if (this.x + this.width > window.innerWidth
      || this.x < 0)
      this.x = window.innerWidth / 2 - this.width / 2
    
    if (this.y + this.height > window.innerHeight
      || this.y < 0)
      this.y = window.innerHeight / 2 - this.height / 2
  }

  hide(): void {
    this.active = !this.active
    this.responseState = 0;
  }

  addTextToEditor(text: string): void {
    this.$root.$emit('add-text-to-editor', text + '\n');
  }

  beforeCreate() {
    this._commentService = new CommentService();
    this._postService = new PostService();
  }

  isLoading(): string {
    if (this.submitProgress === ResponseState.loading)
      return 'header-loading'
    return ''
  }

  async submit(textContent: string, textTitle: string) {
    console.log('response state', this.responseState)
    console.log(textContent, textTitle)
    console.log('submit', this.replyToPost)

    if (textContent === null || textContent === "")
    {
      this.$awn.warning("Text can't be empty", {
        durations: {
          error: 1500
        }
      })
      return;
    }

    if (this.responseState === 0)
    {
      this.$awn.warning("Logical error with 'responseState' sry", {
        durations: {
          error: 1500
        }
      })
      return;
    }

    if (this.responseState === 1)
    {
      if (this.replyToPost !== undefined && this.replyToPost !== null)
      {
        //textContent = await this.parseMarkdown(textContent);

        let commentToSend = {
          title: textTitle,
          text: textContent,
          postId: this.replyToPost.id,
          mentionList: this.mentionList,
        }

        let attachmentList: number[] = [];
        this.attachmentList.forEach(x => {
          attachmentList.push(x.id);
        })

        this.$awn.async(this._commentService.sendComment(commentToSend, attachmentList), (ok: AxiosResponse<IComment>) => {
          this.$root.$emit('comment-sent-success', ok)
          this.$awn.success('Comment sent!', {
            durations: {
              success: 1500
            }
          })
          this.attachmentList = []
          this.active = false;
          this.responseState = 0;
          
          if (ok.data.attachmentComment.length > 0)
            EventBus.emit("new-attachments", ok.data.attachmentComment)
          }, error => {
            console.log(error)
            this.$root.$emit('comment-sent-error', error)
            console.log(this.$awn)
            this.$awn.warning("Cant send comment", {
              durations: {
                error: 1500
              }
            })
          }, 'Sending comment')
      }
    } else {
      if (textTitle === null || textTitle === "")
      {
        this.$awn.warning("Title can't be empty", {
          durations: {
            error: 1500
          }
        })
        return;
      }

      if (this.board === undefined || this.board === null)
      {
        this.$awn.warning("Logical error with 'board' object sry", {
          durations: {
            error: 1500
          }
        })
        return;
      }

      let newThread = {
        title: textTitle,
        text: textContent,
        boardId: this.board.id,
        mentionList: this.mentionList,
      }

      let attachmentList: number[] = [];
      this.attachmentList.forEach(x => {
        attachmentList.push(x.id);
      })

      this.$awn.async(this._postService.sendNewPost(newThread, attachmentList), (ok: AxiosResponse<IPost>) => {
        this.$root.$emit('post-created-success', ok)
        this.$awn.success('New thread created!', {
          durations: {
            success: 1500
          }
        })
        this.attachmentList = []
        this.active = false;
        this.responseState = 0;
        if (this.board !== null)
          this.$router.push({path: `${this.board.name}/${ok.data.id}`})

        //this.content = "";
        }, error => {
          console.log(error)
          this.$root.$emit('post-created-error', error)
          console.log(this.$awn)
          this.$awn.warning("Cant create new thread", {
            durations: {
              error: 1500
            }
          })
        }, 'Creating thread')
    }
  }

  @Watch('hovered')
  hov(value) {
    //console.log(value)
  }

  onwheel(event) {
    //console.log(event)
    //if (this.hovered === true)
    //  event.preventDefault()
    if (event.toElement.className === "filepond--image-preview-wrapper" 
        || event.toElement.className === "ql-editor"
        || event.toElement.isContentEditable === true)
      return true

    event.preventDefault()
  }

  uploaded(attachment) {
    console.log('uploaded')
    this.isFileUploading = false;
    let atObj: IAttachment = attachment.data
    this.attachmentList.push(atObj);
  }

  created(): void {
      
  }

  mounted(): void {
    
  }

  beforeOpen(event): void {
    this.replyToPost = event.params.replyToPost;
    this.replyToComment = event.params.replyToComment;
    console.log(this.replyToPost, this.replyToComment)
    let a: any = this.$children[0];
    console.log(a)
    a.setPositionFromLocalStorage();
  }

  beforeClose (event) {
    let a: any = this.$children[0];
    console.log(a)
    a.savePositionToLocalStorage()
      //this.editor.destroy()
  }
}
</script>

<style lang="scss" scoped>
$blue: #1ebcc5;

.vdr {
  border: none;
}

.send-button {
  height: 26px;
  margin: 2px;
}

.close-button {
  cursor: pointer;
  float: right;
  width: 26px;
  height: 26px;
  margin-top: 2px;
  margin-right: 2px;
  box-shadow: 0px 10 10px 10px rgba(0, 0, 0, 0.25);
  border-radius: 10px;
  background: #000;
  position: relative;
  display: block;
  z-index: 200;
  text-indent: -9999px;
}
.close-button:before,
.close-button:after {
  content: '';
  width: 55%;
  height: 2px;
  background: #fff;
  position: absolute;
  top: 48%;
  left: 22%;
  -webkit-transform: rotate(-45deg);
  -moz-transform: rotate(-45deg);
  -ms-transform: rotate(-45deg);
  -o-transform: rotate(-45deg);
  transform: rotate(-45deg);
  -webkit-transition: all 0.3s ease-out;
  -moz-transition: all 0.3s ease-out;
  -ms-transition: all 0.3s ease-out;
  -o-transition: all 0.3s ease-out;
  transition: all 0.3s ease-out;
}
.close-button:after {
  -webkit-transform: rotate(45deg);
  -moz-transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  -o-transform: rotate(45deg);
  transform: rotate(45deg);
  -webkit-transition: all 0.3s ease-out;
  -moz-transition: all 0.3s ease-out;
  -ms-transition: all 0.3s ease-out;
  -o-transition: all 0.3s ease-out;
  transition: all 0.3s ease-out;
}
.close-button:hover:before,
.close-button:hover:after {
  -webkit-transform: rotate(180deg);
  -moz-transform: rotate(180deg);
  -ms-transform: rotate(180deg);
  -o-transform: rotate(180deg);
  transform: rotate(180deg);
}


</style>

<style lang="scss">
.draggable.vdr {
  position: fixed !important;
  z-index: 999 !important;
}
</style>

<style lang="scss" scoped>
@-webkit-keyframes MOVE-BG {
	from {
    background-position: 0 0;
  }
  to {
    background-position: 40px 0px;
  }
}

@keyframes MOVE-BG {
	from {
    background-position: 0 0;
  }
  to {
    background-position: 40px 0px;
  }
}

.header-draggable {
  background-image: linear-gradient(135deg, #588bae 25%, #4c516d 25%, #4c516d 50%, #588bae 50%, #588bae 75%, #4c516d 75%, #4c516d 100%);
  background-size: 40.00px 40.00px;
  cursor: move;
  &.header-loading {
    -webkit-animation-name: MOVE-BG;
    -webkit-animation-duration: .6s;
    -webkit-animation-timing-function: linear;
    -webkit-animation-iteration-count: infinite;
    
    animation-name: MOVE-BG;
    animation-duration: .6s;
    animation-timing-function: linear;
    animation-iteration-count: infinite;
  }
}
</style>

<style lang="scss">
$header-height: 30px;

.editor {
  display: flow-root !important;
  flex-direction: column;
}

.header-draggable {
  height: $header-height;
  max-height: $header-height;
  background-color: black;
}

.editor-attachment {
  //height: 130px;
  background-color: gray;
}
</style>
