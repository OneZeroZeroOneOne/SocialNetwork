<template>
<span class="inline">
  <span>{{text}}</span>
</span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
export default class TextComponentHtml extends Vue {
    @Prop() public text!: string;
    
}
</script>>