<template>
<div class="button-to-top">
    <font-awesome-icon icon="angle-double-up" />
</div>
</template>


<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";


export default class TopBottomButtons extends Vue {
    constructor() {
        super();
    }
}
</script>

<style lang="scss" scoped>
.button-to-top {
    color: white;
    //width: 200%;
    //height: 200%;
    position: fixed;
    right: 50%;
    bottom: 50%;
}
</style>