<template>
    <modal name="preview-video-modal" 
        :reset="true"
        :draggable="true"
        @before-open="beforeOpen"
        @before-close="beforeClose"
        @not-hovered-close="notHoveredClose">
        <div class="attachment-show">
            <video @mouseup.self="click" id="preview-video-modal" controls autoplay loop :width="width" :height="height">
                <source :poster="posterPath" :src="srcPath">
            </video>
        </div>
    </modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class PreviewVideoModal extends Vue {
    public srcPath: string = "";
    public posterPath: string = "";
    public width: number = 1;
    public height: number = 1;

    private videoElem!: HTMLVideoElement;

    constructor() {
        super();
    }

    notHoveredClose() {
        this.$modal.hide('preview-video-modal')//toggle(false)
    }

    click(event: any): void {
        let elem: HTMLElement | null = document.getElementById("preview-video-modal")
        if (elem !== null)
        {
            elem.onclick = (e: any) => {
                e.preventDefault()
            }
        }
    }

    mounted(): void {
        this.$root.$on('resize', (args) => {
            let elem: HTMLElement | null = document.getElementById("preview-video-modal")
            if (elem !== null)
            {
                this.width = args.width
                this.height = args.height
            }
        })
    }

    beforeOpen(event): void {
        let a: any = this.$children[0];
        this.srcPath = event.params.srcPath;
        this.videoElem=document.createElement("video");
        this.videoElem.oncanplay = (e: any) => {
            e.src="about:blank";
            document.body.removeChild(this.videoElem);   
            this.width = e.srcElement.videoWidth;
            this.height = e.srcElement.videoHeight
            a.handleModalResize({size: {width: this.width, height: this.height}})
        };

        document.body.appendChild(this.videoElem);

        this.videoElem.src=this.srcPath;
    }

    beforeClose (event) {
    }
}
</script>

<style lang="scss">
.v--modal-overlay {
    background: rgba(0, 0, 0, 0) !important;
}

.v--modal-background-click {
    z-index: -999;
}

img {
    width: 100%;
    height: 100%;
}
</style>