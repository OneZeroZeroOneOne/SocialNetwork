<template>
    <modal name="editor-modal" 
        :reset="true"
        draggable=".header-draggable"
        :clickToClose="false"
        :height="440"
        @before-open="beforeOpen"
        @before-close="beforeClose">
        <div class="header-draggable"/>
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
        <div class="editor-footer-modal">
          <div class="editor-attachment">
            <AttachmentDropComponent/>
          </div>
        </div>
    </modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import Quill from 'quill'
import AttachmentDropComponent from '../components/AttachmentDropComponent.vue';

@Component({
    components: {
        Quill,
        AttachmentDropComponent,
    }
})
export default class PreviewModal extends Vue {
    public srcPath: string = "";
    public width: number = 20;
    public height: number = 20;
    //public editor!: Editor;
    public keepInBounds: boolean = true;
    public content: string = "test";
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

<style lang="scss">
$header-height: 30px;

.header-draggable {
  height: $header-height;
  background-color: black;
}

.editor-attachment {
  height: 130px;
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