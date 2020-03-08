<template>
    <div class="editor-text">
        <div class="editor-text-header">
            <input v-model="textTitle" placeholder="По существу, кратко">
            <div class="button" v-on:click.self="submit()">
                Submit
            </div>
        </div>
        <textarea 
            id="editorText"
            class="editor-text-area"
            v-model="textContent" 
            placeholder="Ну давай, че хотел то">
        </textarea>
        <div class="word-counter">
            {{ wordCount }}
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {}
})
export default class EditorText extends Vue {
    public textContent: string = "";
    public textTitle: string = "";

    public maxWordCount: number = 1500;
    @Prop() public height!: number;

    mounted() {
        this.$root.$on('add-text-to-editor', this.addText)
    }

    destroy() {
        this.$root.$off('add-text-to-editor')
    }

    addText(text: string) {
        let editor: any = document.getElementById("editorText")
        if (editor !== null)
        {
            this.textContent = [this.textContent.slice(0, editor.selectionStart), text, this.textContent.slice(editor.selectionStart)].join('');
        }
    }

    get wordCount(): number {
        return this.maxWordCount - this.textContent.length;
    }

    submit(): void {
        this.$emit('editor-text-submit', this.textContent, this.textTitle);
    }
}
</script>

<style lang="scss" scoped>
.editor-text {
    display: flex;
    flex-flow: column;
    .editor-text-header {
        display: grid;
        grid-auto-flow: column;
        height: 31px;
    }
    .editor-text-area {
        width: auto;
        resize: vertical;
        min-height: 250px;
    }
    .word-counter {
        background-color: white;
        text-align: right;
    }
}
</style>