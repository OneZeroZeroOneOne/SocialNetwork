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
            :resizable="false"
            :disable-user-select="false"
            class-name="draggable-class">
            <div class="attachment-show"
            @mouseover="hovered = true" @mouseleave="hovered = false">
                <div class="attachment-modal-description noselect">
                    {{attachment.displayName}} ({{attachment.width}}×{{attachment.height}})
                </div>
                <img class="attachment-modal-content" :src="srcPath" v-if="loaded === true && showType === 0">
                <video class="attachment-modal-content dont-pause"
                id="attachment-player"
                :width="width"
                :height="height"
                @volumechange.self="volumeChange"
                controls 
                autoplay 
                loop 
                v-if="loaded === true && showType === 1">
                    <source :poster="posterPath" :src="srcPath">
                </video>
            </div>
        </vue-draggable-resizable>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import VueDraggableResizable from 'vue-draggable-resizable'
import { IAttachment } from '@/models/responses/Attachment';
import { parseNumber } from '@/utilities/parser';

enum ShowType {
    img,
    video,
}

@Component({
  components: {}
})
export default class AttachmentModal extends Vue {
    private attachment!: IAttachment;

    public width: number = 200;
    public height: number = 200;
    public initialWidth: number = 200;
    public initialHeight: number = 200;
    public x: number = 0;
    public y: number = 0;

    public active: boolean = false;
    public hovered: boolean = false;
    public loaded: boolean = true;

    public srcPath: string = '';
    public posterPath: string = '';

    public ratio: number = 0;

    public minWidth: number = 200;
    public minHeight: number = 200;

    public showType: ShowType = ShowType.img;
    

    constructor() {
        super();
        this.$root.$on('show-attachment-image', this.showImage)
        this.$root.$on('show-attachment-video', this.showVideo)
        document.addEventListener('mousedown', this.eventClick, {passive: false});
        //window.onclick = this.mouseUp;
        document.addEventListener('click', this.mouseUp, {passive:false});
        document.addEventListener("wheel", this.handleWheel, {passive: false})
    }

    handleWheel(event: any): void {
        if (this.hovered) {
            let newW = Math.round((this.initialWidth / this.ratio) / 10)
            let newH = Math.round((this.initialHeight / this.ratio) / 10)

            if (event.deltaY < 0)
            {
                this.x -= Math.round(newW / 2);
                this.y -= Math.round(newH / 2);

                this.width += newW;
                this.height += newH;
            } else {
                if (this.width - newW > this.minWidth || this.height - newH > this.minHeight)
                {
                    this.x += Math.round(newW / 2);
                    this.y += Math.round(newH / 2);

                    this.width -= newW;
                    this.height -= newH;
                }
            }
            this.width = Math.max(this.width, 200)
            this.height = Math.max(this.height, 200)
            event.preventDefault()
        }
    }

    @Watch('active',{ immediate: true}) 
    onRouteChange(obj): void {
        //console.log('active', obj)
    }

    @Watch('hovered',{ immediate: true}) 
    onhoveredChange(obj): void {
        //console.log('hovered', obj)
    }

    volumeChange(event: any) {
        var video: any = document.getElementById('attachment-player');
        if (video !== null)
            localStorage.setItem('attachment-player-volume', video.volume)
    }

    mouseUp(event: any): boolean {
        if (this.hovered)
        {
            event.preventDefault();
            return false;
        }
        return true;
    }

    eventClick(event: any): void {
        //console.log(event, this.hovered)
        if (this.hovered) {
            this.active = true;
            event.preventDefault()
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
        return 'http://194.99.21.140/api/attachment/' + path;
    }

    adjustSize() {
        while(this.width >= window.innerWidth - 100 
            || this.height >= window.innerHeight - 30) {
            let newW = Math.round((this.initialWidth / this.ratio) / 10)
            let newH = Math.round((this.initialHeight / this.ratio) / 10)

            if (this.width - newW > this.minWidth || this.height - newH > this.minHeight) {
                this.x += Math.round(newW / 2);
                this.y += Math.round(newH / 2);

                this.width -= newW;
                this.height -= newH;
            }
        }
    }

    showVideo(attachment: IAttachment) {
        this.attachment = attachment;

        this.showType = ShowType.video;
        this.active = true;
        this.posterPath = attachment.preview;

        this.initialWidth = attachment.width
        this.initialHeight = attachment.height
        this.width = this.initialWidth 
        this.height = this.initialHeight

        this.ratio = this.width / this.height

        this.x = window.innerWidth / 2 - this.width / 2; //localStorage.getItem('modal-image-x') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-x')).value
        this.y = window.innerHeight / 2 - this.height / 2;//localStorage.getItem('modal-image-y') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-y')).value

        this.srcPath = this.getAttachmentPath(attachment.path);
        
        this.adjustSize()

        this.$nextTick().then(x => {
            var video: any = document.getElementById('attachment-player');
            let vol = localStorage.getItem('attachment-player-volume')
            video.volume = vol !== null ? parseFloat(vol) : 0.5
        })
    }

    showImage(attachment: IAttachment) {
        this.attachment = attachment;

        this.showType = ShowType.img;
        this.active = true;

        this.initialWidth = attachment.width
        this.initialHeight = attachment.height
        this.width = this.initialWidth 
        this.height = this.initialHeight

        this.ratio = this.width / this.height

        this.x = window.innerWidth / 2 - this.width / 2; //localStorage.getItem('modal-image-x') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-x')).value
        this.y = window.innerHeight / 2 - this.height / 2;//localStorage.getItem('modal-image-y') === undefined ? 500 : parseNumber(localStorage.getItem('modal-image-y')).value

        this.srcPath = this.getAttachmentPath(attachment.path);

        this.adjustSize()
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
        //console.log('DRAGSTOP')
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
    position: fixed !important;
}

.attachment-modal-content {
    border-color: var(--attachment-modal-border-color);
    border-style: solid;
    border-width: 5px;
    box-sizing: content-box;
    border-top-width: 20px;
    background-color: var(--attachment-modal-border-color);
}

.attachment-modal-description {
    position: relative !important;
    top: 20px;
    text-align: center;
    color: var(--attachment-modal-text-color);
}

.attachment-modal-content:focus {
    outline: 0 !important;
}
</style>