<template>
<div class="button-actions">
    <div v-if="canUp">
        <div class="to-top clickable" @click="toTop()">
            <font-awesome-icon class="svg-icon" icon="angle-double-up" />
        </div>
    </div>
    <div v-if="canDown">
        <div class="to-down clickable" @click="toBottom()">
            <font-awesome-icon class="svg-icon" icon="angle-double-down" />
        </div>
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
        this.catchScroll({});
    }

    toTop(): void {
        //scrollTo(0, 300)
        window.scrollTo({ top: 0, behavior: 'smooth' })
    }

    toBottom(): void {
        let body = document.body,
            html = document.documentElement;
        //console.log(Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight))
        //scrollTo(Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight), 300)
        window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' })
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