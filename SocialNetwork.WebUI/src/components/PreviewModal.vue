<template>
    <modal name="preview-modal" 
        :reset="true"
        :draggable="true"
        @before-open="beforeOpen"
        @before-close="beforeClose"
        @not-hovered-close="notHoveredClose">
        <div class="attachment-show">
            <img :src="srcPath" :width="width">
        </div>
    </modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class PreviewModal extends Vue {
    public srcPath: string = "";
    public width: number = 20;
    public height: number = 20;

    constructor() {
        super();
    }

    notHoveredClose() {
        this.$modal.hide('preview-modal')//toggle(false)
    }

    beforeOpen(event): void {
        let a: any = this.$children[0];
        this.srcPath = event.params.srcPath;
        var img = new Image();

        img.addEventListener("load", (e: any) => {
            this.width = e.path[0].width
            this.height = e.path[0].height
            /* there is two way setup size for image
            this.$set(a, "width", this.width)
            this.$set(a, "height", this.height)
            a.setInitialSize()

            second
            */

            if (this.height >= window.innerHeight || this.width >= window.innerWidth)
            {
                this.height = this.height/2
                this.width = this.width/2
            }

            a.handleModalResize({size: {width: this.width, height: this.height}})
        });

        img.src = this.srcPath;
    }

    beforeClose (event) {
    }
}
</script>

<style lang="scss">
.v--modal-overlay {
    background: rgba(0, 0, 0, 0) !important;
}

img {
    width: 100%;
    height: 100%;
}
</style>