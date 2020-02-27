<template>
    <modal name="editor-modal" 
        :reset="true"
        draggable=".header-draggable"
        :clickToClose="false"
        :resizable="true"
        :scrollable="true"
        :maxHeight="300"
        @before-open="beforeOpen"
        @before-close="beforeClose">
        <div class="header-draggable"/>
        <div class="editor-modal-show">
            <div class="editor">
                <editor-menu-bar :editor="editor" v-slot="{ commands, isActive }">
                <div class="menubar">
                    <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.heading({ level: 3 }) }"
                    @click="commands.heading({ level: 3 })"
                    >
                    H3
                    </button>

                    <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.bullet_list() }"
                    @click="commands.bullet_list"
                    >
                    <font-awesome-icon icon="list-ul" />
                    </button>

                    <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.ordered_list() }"
                    @click="commands.ordered_list"
                    >
                    <font-awesome-icon icon="list-ol" />
                    </button>

                    <button
                    class="menubar__button"
                    :class="{ 'is-active': isActive.blockquote() }"
                    @click="commands.blockquote"
                    >
                    <font-awesome-icon icon="quote-left" />
                    </button>
                    
                    <button
                    class="menubar__button"
                    @click="commands.horizontal_rule"
                    >
                    <font-awesome-icon icon="grip-lines" />
                    </button>

                    <button
                    class="menubar__button"
                    @click="commands.undo"
                    >
                    <font-awesome-icon icon="undo" />
                    </button>

                    <button
                    class="menubar__button"
                    @click="commands.redo"
                    >
                    <font-awesome-icon icon="redo" />
                    </button>

                </div>
                </editor-menu-bar>
                <editor-menu-bubble :editor="editor" :keep-in-bounds="keepInBounds" v-slot="{ commands, isActive, menu }">

                  
                  <div
                    class="menububble"
                    :class="{ 'is-active': menu.isActive }"
                    :style="`left: ${menu.left}px; bottom: ${menu.bottom}px;`"
                  >

                    
                    <button
                      class="menububble__button"
                      :class="{ 'is-active': isActive.strike() }"
                      @click="commands.strike"
                    >
                      <font-awesome-icon icon="strikethrough" />
                    </button>

                    <button
                      class="menububble__button"
                      :class="{ 'is-active': isActive.underline() }"
                      @click="commands.underline"
                    >
                      <font-awesome-icon icon="underline" />
                    </button>

                    <button
                      class="menububble__button"
                      :class="{ 'is-active': isActive.bold() }"
                      @click="commands.bold"
                    >
                      <font-awesome-icon icon="bold" />
                    </button>

                    <button
                      class="menububble__button"
                      :class="{ 'is-active': isActive.italic() }"
                      @click="commands.italic"
                    >
                      <font-awesome-icon icon="italic" />
                    </button>

                    <button
                      class="menububble__button"
                      :class="{ 'is-active': isActive.code() }"
                      @click="commands.code"
                    >
                      <font-awesome-icon icon="code" />
                    </button>

                  </div>
                </editor-menu-bubble>
                <editor-content class="editor__content" :editor="editor" />
            </div>
        </div>
    </modal>
</template>

<script lang="ts">
import Icon from '../components/Icon.vue'
import { Component, Prop, Vue } from "vue-property-decorator";
import { Editor, EditorContent, EditorMenuBar, EditorMenuBubble  } from 'tiptap'
import {
  Blockquote,
  CodeBlock,
  HardBreak,
  Heading,
  HorizontalRule,
  OrderedList,
  BulletList,
  ListItem,
  TodoItem,
  TodoList,
  Bold,
  Code,
  Italic,
  Link,
  Strike,
  Underline,
  History,
} from 'tiptap-extensions'

@Component({
    components: {
        EditorContent,
        Editor,
        EditorMenuBar,
        Icon,
        EditorMenuBubble,
    }
})
export default class PreviewModal extends Vue {
    public srcPath: string = "";
    public width: number = 20;
    public height: number = 20;
    public editor!: Editor;
    public keepInBounds: boolean = true;

    constructor() {
        super();
        this.editor = new Editor({
            extensions: [
                new Blockquote(),
                new BulletList(),
                new CodeBlock(),
                new HardBreak(),
                new Heading({ levels: [1, 2, 3] }),
                new HorizontalRule(),
                new ListItem(),
                new OrderedList(),
                new TodoItem(),
                new TodoList(),
                new Link(),
                new Bold(),
                new Code(),
                new Italic(),
                new Strike(),
                new Underline(),
                new History(),
            ],
            content: '<p>This is just a boring paragraph</p>',
        })
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
$color-black: #000000;
$color-white: #ffffff;
$color-grey: #dddddd;

.editor {
  position: relative;
  max-width: 30rem;
  margin: 0 auto 5rem auto;

  &__content {

    overflow-wrap: break-word;
    word-wrap: break-word;
    word-break: break-word;

    * {
      caret-color: currentColor;
    }

    pre {
      padding: 0.7rem 1rem;
      border-radius: 5px;
      background: $color-black;
      color: $color-white;
      font-size: 0.8rem;
      overflow-x: auto;

      code {
        display: block;
      }
    }

    p code {
      display: inline-block;
      padding: 0 0.4rem;
      border-radius: 5px;
      font-size: 0.8rem;
      font-weight: bold;
      background: rgba($color-black, 0.1);
      color: rgba($color-black, 0.8);
    }

    ul,
    ol {
      padding-left: 1rem;
    }

    li > p,
    li > ol,
    li > ul {
      margin: 0;
    }

    a {
      color: inherit;
    }

    blockquote {
      border-left: 3px solid rgba($color-black, 0.1);
      color: rgba($color-black, 0.8);
      padding-left: 0.8rem;
      font-style: italic;

      p {
        margin: 0;
      }
    }

    img {
      max-width: 100%;
      border-radius: 3px;
    }

    table {
      border-collapse: collapse;
      table-layout: fixed;
      width: 100%;
      margin: 0;
      overflow: hidden;

      td, th {
        min-width: 1em;
        border: 2px solid $color-grey;
        padding: 3px 5px;
        vertical-align: top;
        box-sizing: border-box;
        position: relative;
        > * {
          margin-bottom: 0;
        }
      }

      th {
        font-weight: bold;
        text-align: left;
      }

      .selectedCell:after {
        z-index: 2;
        position: absolute;
        content: "";
        left: 0; right: 0; top: 0; bottom: 0;
        background: rgba(200, 200, 255, 0.4);
        pointer-events: none;
      }

      .column-resize-handle {
        position: absolute;
        right: -2px; top: 0; bottom: 0;
        width: 4px;
        z-index: 20;
        background-color: #adf;
        pointer-events: none;
      }
    }

    .tableWrapper {
      margin: 1em 0;
      overflow-x: auto;
    }

    .resize-cursor {
      cursor: ew-resize;
      cursor: col-resize;
    }

  }
}
.header-draggable {
    height: 20px;
    background-color: black;
}

.menububble {
  position: absolute;
  display: flex;
  z-index: 20;
  background: $color-black;
  border-radius: 5px;
  padding: 0.3rem;
  margin-bottom: 0.5rem;
  transform: translateX(-50%);
  visibility: hidden;
  opacity: 0;
  transition: opacity 0.2s, visibility 0.2s;

  &.is-active {
    opacity: 1;
    visibility: visible;
  }

  &__button {
    display: inline-flex;
    background: transparent;
    border: 0;
    color: $color-white;
    padding: 0.2rem 0.5rem;
    margin-right: 0.2rem;
    border-radius: 3px;
    cursor: pointer;

    &:last-child {
      margin-right: 0;
    }

    &:hover {
      background-color: rgba($color-white, 0.1);
    }

    &.is-active {
      background-color: rgba($color-white, 0.2);
    }
  }

  &__form {
    display: flex;
    align-items: center;
  }

  &__input {
    font: inherit;
    border: none;
    background: transparent;
    color: $color-white;
  }
}
</style>

<style lang="scss">

.v--modal-overlay {
    background: rgba(0, 0, 0, 0) !important;
}

img {
    width: 100%;
    height: 100%;
}
</style>