<template>
  <div v-if="active">
    <vue-draggable-resizable
            :w="width + 10" 
            :h="height" 
            :x="x"
            :y="y"
            @dragging="onDrag" 
            @resizing="onResize"
            @dragstop="dragstop"
            :disable-user-select="false"
            :resizable="false"
            class-name="editor-modal"
            :drag-handle="'.header-draggable'"
            :drag-cancel="'.cant-drag'">
        <div :class="'header-draggable '+isLoading()">
          <span class="cant-drag close-button" v-on:click="hide"></span>
          <button class="cant-drag send-button" v-on:click="submit">Send!</button>
        </div>
        <div class="editor-modal-show">
            <div class="editor">
               <quill-editor v-model="content"
                            ref="myQuillEditor"
                            :options="editorOption"
                            @blur="onEditorBlur($event)"
                            @focus="onEditorFocus($event)"
                            @ready="onEditorReady($event)">
              </quill-editor>
              <div id="counter">0</div>
            </div>
        </div>
        <div @mouseover="hovered = true" @mouseleave="hovered = false" 
        class="editor-footer-modal">
          <div class="editor-attachment">
            <attachment-drop-component
            @uploaded-succesfully="uploaded"/>
          </div>
        </div>
    </vue-draggable-resizable>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import VueDraggableResizable from 'vue-draggable-resizable'
import Quill from 'quill'

import { IAttachment } from '../models/responses/Attachment';
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';

import AttachmentDropComponent from '../components/AttachmentDropComponent.vue';

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { CommentService } from '../services/Implementations/CommentService';
import { ResponseState } from '../models/enum/ResponseState';
import { parseNumber } from './vue-js-modal/src/parser';

Quill.register('modules/counter', function(quill, options) {
  let container: any = document.querySelector('#counter');
  quill.on('text-change', function() {
    let text = quill.getText();
    
    container.innerText = 20001 - text.length;
  });
})

@Component({
    components: {
        Quill,
        AttachmentDropComponent,
    }
})
export default class PreviewModal extends Vue {
  private submitProgress: ResponseState = ResponseState.success;

  public x: number = 0;
  public y: number = 0;

  public active: boolean = false;

  public srcPath: string = "";

  public width: number = 450;
  public height: number = 333;

  public keepInBounds: boolean = true;

  public content: string = "test";
  public hovered: boolean = false;

  public replyToPost!: IPost;
  public replyToComment!: IComment;

  public quill: any;

  public editorOption: any = {
    modules: {
      toolbar: [
        ['bold', 'italic', 'underline', 'strike'],
        ['blockquote', 'code-block'],
        [{ 'script': 'sub' }, { 'script': 'super' }],
        [{ 'color': [] }, { 'background': [] }],
        ['link'],
        ['clean']
      ],
      counter: true,
    }
  };

  public attachmentList: IAttachment[] = [];

  private _commentService!: ICommentService;

  constructor() {
      super();
      this.$root.$on('comment-header-link-click', this.addTextToEditor)
      this.$root.$on('show-editor-modal-from-post', this.showEditorFromPost)
  }

  showEditorFromPost(post: IPost): void {
    if (this.active === false)
    {
      this.setPositionFromStorage();
      this.active = true;
    } else {
      console.log('already open')
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
  }

  addTextToEditor(text: string): void {
    this.$modal.show('editor-modal')
    this.$nextTick().then(x => {
      let selection = this.quill.getSelection(true);
      this.quill.insertText(selection.index, text + '\n');
    })
  }

  alreadyOpen() {
    console.log('already open')
    let modal = document.getElementsByClassName('v--modal-box')[0]
    //this.$el.classList.add('opened')
  }

  beforeCreate() {
    this._commentService = new CommentService();
  }

  isLoading(): string {
    if (this.submitProgress === ResponseState.loading)
      return 'header-loading'
    return ''
  }

  async submit(event) {
    if (this.replyToPost !== undefined)
    {
      let commentToSend = {
        text: this.content,
        postId: this.replyToPost.id,
      }
      let attachmentList: number[] = [];
      this.attachmentList.forEach(x => {
        attachmentList.push(x.id);
      })
      
      this.$awn.async(this._commentService.sendComment(commentToSend, attachmentList), ok => {
        this.$root.$emit('comment-sent-success', ok)
        this.$awn.success('Comment sent!', {
          durations: {
            success: 1500
          }
        })
        this.attachmentList = []
        this.$modal.hide('editor-modal')
      }, error => {
        console.log(error)
        this.attachmentList = []
        this.$root.$emit('comment-sent-error', error)
        this.$awn.error("Can't send comment", {})
      }, 'Sending comment')
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
    let atObj: IAttachment = attachment.data
    this.attachmentList.push(atObj);
  }

  onEditorBlur(quill) {
    console.log('editor blur!', quill)
  }

  onEditorFocus(quill) {
    console.log('editor focus!', quill)
  }

  onEditorReady(quill) {
    this.quill = quill;
    console.log('editor ready!', quill)
  }

  onEditorChange({ quill, html, text }) {
    console.log('editor change!', quill, html, text)
    this.content = html
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

.editor-modal {
  z-index: 999 !important;
  position: fixed !important;
}

.quill-editor {
  background-color: white;
}

.editor {
  display: flow-root !important;
  flex-direction: column;
}

#counter {
  text-align: right;
  padding-right: 5px;
  background-color: white;
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

.quill-editor {
  width: -webkit-fill-available;
}
.quill-code {
  border: none;
  height: auto;
  > code {
    width: 100%;
    margin: 0;
    padding: 1rem;
    border: 1px solid #ccc;
    border-top: none;
    border-radius: 0;
    height: 10rem;
    overflow-y: auto;
    resize: vertical;
  }
}

.ql-container.ql-snow {
  height: calc(300px - 60px);
}
</style>