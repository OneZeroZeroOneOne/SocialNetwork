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
               />
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

import { IAttachment } from '../models/responses/Attachment';
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';
import EditorText from '@/components/EditorText.vue';

import AttachmentDropComponent from '../components/AttachmentDropComponent.vue';

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { CommentService } from '../services/Implementations/CommentService';
import { ResponseState } from '../models/enum/ResponseState';
import { parseNumber } from '@/utilities/parser';
import { IPostService } from '../services/Abstractions/IPostService';
import { PostService } from '../services/Implementations/PostService';


@Component({
    components: {
        AttachmentDropComponent,
        EditorText,
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

  public shake: boolean = false;

  public attachmentList: IAttachment[] = [];
  public mentionList: string[] = [];

  private _commentService!: ICommentService;
  private _postService!: IPostService;

  constructor() {
      super();
      this.$root.$on('show-editor-modal-from-post', this.showEditorFromPost)
      this.$root.$on('show-editor-modal-from-comment', this.showEditorFromComment)
  }

  toggleShakeAnimation(): void {
    if (this.shake === false)
    {
      this.shake = true;
      setTimeout(x => { 
        this.shake = false;
      }, 1000)
    }
  }


  showEditorFromComment(com: IComment, fatherPost: IPost): void {
    this.addTextToEditor(">>" + com.id)
    this.replyToPost = fatherPost;
    this.replyToComment = com;
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
    this.replyToPost = post;
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
  }

  addTextToEditor(text: string): void {
    //this.$modal.show('editor-modal')
    /*this.$nextTick().then(x => {
      let selection = this.quill.getSelection(true);
      this.quill.insertText(selection.index, text + '\n');
    })*/
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



  async parseMarkdown(textContent: string): Promise<string> {
    let md = textContent;

    console.log(md)

    this.mentionList = [];

    //green text
    //need to be first because of '>'
    Array.from(md['matchAll'](/(>>{1}(\d+))/g), (x: any) => {
      let mention: string = x[2]
      if (this.mentionList.indexOf(mention) === -1)
      {
        this.mentionList.push(mention)
      }
    })

    let commentList: string[] = []
    let postList: string[] = []
    let unresolvedList: string[] = []

    for (let index = 0; index < this.mentionList.length; index++) {
      await this._commentService.getCommentById(this.mentionList[index])
        .then(ok => {
          console.log(ok)
          commentList.push(this.mentionList[index])
        }).catch(async x => {
          await this._postService.getPost(this.replyToPost.boardId, this.mentionList[index])
            .then(ok => {
              postList.push(this.mentionList[index])
            }).catch(x => {
              unresolvedList.push(this.mentionList[index])
            })
        })
    }
    
    //link to post/comment
    //need to be first because of '>>'
    md = md.replace(/(>>{1}(\d+))/g, '<link-to comment={{_comment_link_$2_}} post={{_post_link_$2_}}[sign-bigger]$2</link-to[sign-bigger]');
     
    console.log(commentList, postList, unresolvedList, md)

    commentList.forEach(element => {
      let toReplaceCom = `{{_comment_link_${element}_}}`
      md = md.split(toReplaceCom).join(element)//md.replace(toReplaceCom, element)
      let toReplacePost = `{{_post_link_${element}_}}`
      md = md.split(toReplacePost).join('0')//.replace(toReplacePost, '0')
    });

    postList.forEach(element => {
      let toReplacePost = `{{_post_link_${element}_}}`
      md = md.split(toReplacePost).join(element)//.replace(toReplacePost, element)
      let toReplaceCom = `{{_comment_link_${element}_}}`
      md = md.split(toReplaceCom).join("0")//.replace(toReplaceCom, "0")
    });

    unresolvedList.forEach(element => {
      let toReplacePost = `{{_post_link_${element}_}}`
      md = md.replace(toReplacePost, "0")
      let toReplaceCom = `{{_comment_link_${element}_}}`
      console.log(toReplaceCom)
      md = md.replace(toReplaceCom, "0")
    });

    //green text
    //need to be first because of '>'
    md = md.replace(/(>)(.*)/g, '<green>$2</green>');
    
    //preformatted text
    md = md.replace(/(```\n)(.*\n)(```)/g, '<code>\n$2</code>');
    //b
    md = md.replace(/(\[b\])(.*)(\[\/b\])/g, '<b>$2</b>');
    //i
    md = md.replace(/(\[i\])(.*)(\[\/i\])/g, '<i>$2</i>');
    //strike
    md = md.replace(/(\[s\])(.*)(\[\/s\])/g, '<strike>$2</strike>');
    //spoiler
    md = md.replace(/(\[sp\])(.*)(\[\/sp\])/g, '<sp>$2</sp>');
    
    md = md.replace(/\[sign-bigger\]/g, '>');

    console.log(md)
  
    //replacing 'return character = ↵' with newline
    md = md.replace(/(\r\n|\n|\r)/gm, "<br/>");

    return md;
  }

  async submit(textContent: string, textTitle: string) {
    console.log(textContent, textTitle)
    console.log('submit', this.replyToPost)
    if (this.replyToPost !== undefined)
    {
      textContent = await this.parseMarkdown(textContent);

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

      this.$awn.async(this._commentService.sendComment(commentToSend, attachmentList), ok => {
        this.$root.$emit('comment-sent-success', ok)
        this.$awn.success('Comment sent!', {
          durations: {
            success: 1500
          }
        })
        this.attachmentList = []
        this.active = false;
        //this.content = "";
        }, error => {
          console.log(error)
          this.attachmentList = []
          this.$root.$emit('comment-sent-error', error)
          console.log(this.$awn)
          this.$awn.warning("Cant send comment", {
            durations: {
              error: 1500
            }
          })
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
@keyframes shake {
  10%, 90% {
    transform: translate3d(-1px, 0, 0);
  }
  
  20%, 80% {
    transform: translate3d(2px, 0, 0);
  }

  30%, 50%, 70% {
    transform: translate3d(-4px, 0, 0);
  }

  40%, 60% {
    transform: translate3d(4px, 0, 0);
  }
}

.shakeit {
  animation: shake 0.82s cubic-bezier(.36,.07,.19,.97) both;
}
</style>

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