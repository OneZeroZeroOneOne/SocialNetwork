<template>
    <vue-draggable-resizable
        :w="width" 
        :h="height" 
        :x="x"
        :y="y"
        @dragging="onDrag" 
        @resizing="onResize"
        @dragstop="dragstop"
        :resizable="false"
        :disable-user-select="false"
        class-name="draggable-class">
        <div class="attachment-show"
        @mouseover="hovered = true" @mouseleave="hovered = false"
        v-if="active">
            <img :src="srcPath" v-if="loaded === true">
        </div>
    </vue-draggable-resizable>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import VueDraggableResizable from 'vue-draggable-resizable'
import { IAttachment } from '../models/responses/Attachment';
import { parseNumber } from './vue-js-modal/src/parser';

@Component({
  components: {}
})
export default class NewModal extends Vue {
    public width: number = 200;
    public height: number = 200;
    public x: number = 0;
    public y: number = 0;

    public active: boolean = false;
    public hovered: boolean = false;

    public loaded: boolean = true;
    public srcPath: string = 'http://16ch.tk/api/attachment/Files/132272046934306078.jpg';

    constructor() {
        super();
        this.$root.$on('show-attachment-image', this.showImage)
        document.addEventListener('mousedown', this.eventClick);
    }

    @Watch('active',{ immediate: true}) 
    onRouteChange(obj): void {
        console.log('active', obj)
    }

    eventClick(event: any): void {
        //console.log(event, this.hovered)
        if (this.hovered) {
            this.active = true;
            return;
        }else {
            this.active = false;
        } /*else
            if (event.toElement.className.indexOf('clickable') === -1)
            {
                //console.log('not clickable')
                this.active = false;
                //console.log(this.active)
            } */
    }

    getAttachmentPath(path: string): string {
        return 'http://16ch.tk/api/attachment/' + path;
    }

    showImage(attachment: IAttachment) {
        console.log('showimage')
        this.active = true;

        this.x = localStorage.getItem('modal-image-x') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-x')).value
        this.y = localStorage.getItem('modal-image-y') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-y')).value
        this.srcPath = this.getAttachmentPath(attachment.path);
    }

    onResize(x, y, width, height) {
        this.x = x
        this.y = y
        this.width = width
        this.height = height
    }

    getX() {
        return this.active ? this.x : -99999;
    }

    getY() {
        return this.active ? this.y : -99999;
    }

    dragstop(left, top) {
        //
    }

    onDrag(x, y) {
        localStorage.setItem('modal-image-x', x)
        localStorage.setItem('modal-image-y', y)
        this.x = x
        this.y = y
    }
}
</script>

<style lang="scss">
.draggable-class {
    z-index: 999 !important;
}
</style>