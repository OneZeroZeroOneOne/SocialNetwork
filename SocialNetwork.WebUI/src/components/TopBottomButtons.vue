<template>
<div class="button-actions">
    <div class="to-top clickable" v-if="canUp">
        <font-awesome-icon class="svg-icon" icon="angle-double-up" />
    </div>
    <div class="to-down clickable" v-if="canDown">
        <font-awesome-icon class="svg-icon" icon="angle-double-down" />
    </div>
</div>
</template>


<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";


export default class TopBottomButtons extends Vue {
    public canUp: boolean = false;
    public canDown: boolean = true;

    constructor() {
        super();
        window.addEventListener('wheel', this.catchScroll)
    }

    catchScroll(event: any): void {
        console.log(event)
      /*let visibleoffsetbottom = 400;
      const pastTopOffset = window.pageYOffset > 400
      const pastBottomOffset = window.innerHeight + window.pageYOffset >= document.body.offsetHeight - parseInt(this.visibleoffsetbottom)
      this.canUp = visibleoffsetbottom > 0 ? pastTopOffset && !pastBottomOffset : pastTopOffset*/
      this.canUp = event.pageY > 200
      console.log(this.canUp)
      //this.scrollFn(this)
    }
}
</script>

<style lang="scss" scoped>
.button-actions {

    .clickable {
        cursor: pointer;
    }
    .to-top {
        color: white;
        position: fixed;
        right: 5%;
        top: 10%;
    }
    .to-down {
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