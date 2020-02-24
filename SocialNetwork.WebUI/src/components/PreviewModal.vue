<template>
    <modal name="preview-modal" height="70%"  width="70%"
         :draggable="true"
         :resizable="true"
         :adaptive="true"
         @before-open="beforeOpen"
         @before-close="beforeClose">
        <div class="attachment-show">
            <img :src="srcPath">
        </div>
    </modal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Dictionary } from 'vue-router/types/router';

@Component({})
export default class PreviewModal extends Vue {
    public srcPath: string = "";
    public width: string = "100%"

    constructor() {
        super();
    }

    beforeOpen(event): void {
        console.log(event)
        this.srcPath = event.params.srcPath;
        console.log(this.srcPath)
    }

    beforeClose (event) {
    }
}
</script>

<style lang="scss">
</style>