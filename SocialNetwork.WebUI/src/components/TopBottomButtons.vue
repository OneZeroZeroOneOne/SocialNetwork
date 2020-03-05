<template>
<div class="button-actions">
    <div class="to-top clickable" v-if="canUp" @click="toTop()">
        <font-awesome-icon class="svg-icon" icon="angle-double-up" />
    </div>
    <div class="to-down clickable" v-if="canDown" @click="toBottom()">
        <font-awesome-icon class="svg-icon" icon="angle-double-down" />
    </div>
</div>
</template>


<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { scrollTo } from "@/utilities/scrollTo";

@Component({})
export default class TopBottomButtons extends Vue {
    public canUp: boolean = false;
    public canDown: boolean = true;

    constructor() {
        super();
        window.addEventListener('scroll', this.catchScroll)
    }

    toTop(): void {
        scrollTo(0, 300)
    }

    toBottom(): void {
        let body = document.body,
            html = document.documentElement;
        scrollTo(Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight), 300)
    }

    catchScroll(event: any): void {
        this.canUp = window.scrollY > 200
        let body = document.body,
            html = document.documentElement;

        let maxHeight = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
        this.canDown = window.scrollY + window.innerHeight < maxHeight - 200
    }
}
</script>

<style lang="scss" scoped>

.button-actions {
    .clickable {
        cursor: pointer;
    }
    .to-top {
        background-color: gray;
        border-color: gray;
        border-style: solid;
        border-width: 5px;
        border-right-width: 15px;
        border-left-width: 15px;
        border-radius: 5px;
        color: white;
        position: fixed;
        right: 5%;
        top: 10%;
    }
    .to-down {
        background-color: gray;
        border-color: gray;
        border-style: solid;
        border-width: 5px;
        border-right-width: 15px;
        border-left-width: 15px;
        border-radius: 5px;
        color: white;
        position: fixed;
        right: 5%;
        bottom: 10%;
    }
    .svg-icon {
        width: 30px;
        height: auto;
    }
}
</style>