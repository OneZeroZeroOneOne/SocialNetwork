<template>
    <modal class="editor-modal"
        name="editor-modal" 
        :reset="true"
        draggable=".header-draggable"
        :clickToClose="false"
        height="auto"
        @before-open="beforeOpen"
        @before-close="beforeClose"
        @onwheeleditor="onwheel">
        <div class="header-draggable">
          <span class="close-button" v-on:click="$modal.hide('editor-modal')"></span>
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
            </div>
        </div>
        <div @mouseover="hovered = true" @mouseleave="hovered = false" 
        class="editor-footer-modal">
          <div class="editor-attachment">
            <attachment-drop-component
            @uploaded-succesfully="uploaded"/>
          </div>
        </div>
    </modal>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import Quill from 'quill'
import AttachmentDropComponent from '../components/AttachmentDropComponent.vue';
import { IAttachment } from '../models/responses/Attachment';

@Component({
    components: {
        Quill,
        AttachmentDropComponent,
    }
})
export default class PreviewModal extends Vue {
    public srcPath: string = "";
    public width: number = 20;
    public height: number = 630;
    //public editor!: Editor;
    public keepInBounds: boolean = true;
    public content: string = "test";
    public hovered: boolean = false;
    public editorOption: any = {
      modules: {
        toolbar: [
          ['bold', 'italic', 'underline', 'strike'],
          ['blockquote', 'code-block'],
          [{ 'script': 'sub' }, { 'script': 'super' }],
          [{ 'color': [] }, { 'background': [] }],
          ['link']
        ],
      }
    };

    public attachmentList: IAttachment[] = [];

    constructor() {
        super();
        /*this.editor = new Editor({
            extensions: [
                new Blockquote(),
                new CodeBlock(),
                new Link(),
                new Bold(),
                new Code(),
                new Italic(),
                new Strike(),
                new Underline(),
                new History(),
            ],
            content: 'This is just a boring paragraph',
        })*/
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
      console.log(atObj)
      this.attachmentList.push(atObj);
    }

    onEditorBlur(quill) {
      console.log('editor blur!', quill)
    }

    onEditorFocus(quill) {
      console.log('editor focus!', quill)
    }

    onEditorReady(quill) {
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
    }

    beforeClose (event) {
        //this.editor.destroy()
    }
}
</script>

<style lang="scss" scoped>
$blue: #1ebcc5;

.close-button {
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
.header-draggable {
  background-image: linear-gradient(135deg, #588bae 25%, #4c516d 25%, #4c516d 50%, #588bae 50%, #588bae 75%, #4c516d 75%, #4c516d 100%);
  background-size: 40.00px 40.00px;
}
</style>

<style lang="scss">
$header-height: 30px;

.header-draggable {
  height: $header-height;
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