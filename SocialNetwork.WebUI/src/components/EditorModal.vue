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
                    @click="commands.horizontal_rule"
                    >
                    <font-awesome-icon icon="grip-lines" />
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
                      :class="{ 'is-active': isActive.blockquote() }"
                      @click="commands.blockquote"
                    >
                      <font-awesome-icon icon="quote-left" />
                    </button>

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
            content: 'This is just a boring paragraph',
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

<style lang="scss">

</style>