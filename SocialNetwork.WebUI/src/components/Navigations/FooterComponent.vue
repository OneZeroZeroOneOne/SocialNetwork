<template>
  <div v-view="viewHandler" class="footer">
    <div class="footer-body">
    FOOTER
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";
import { ResponseState } from "@/models/enum/ResponseState";
import _ from 'lodash'

@Component({})
export default class FooterComponent extends Vue {
  constructor() {
    super();
  }

  viewHandler(e: any): void {
    ///console.log(e.type) // 'enter', 'exit', 'progress'
    //console.log(e.percentInView) // 0..1 how much element overlap the viewport
    //console.log(e.percentTop) // 0..1 position of element at viewport 0 - above , 1 - below
    //console.log(e.percentCenter) // 0..1 position the center of element at viewport 0 - center at viewport top, 1 - center at viewport bottom
    //console.log(e.scrollPercent) // 0..1 current scroll position of page
    //console.log(e.scrollValue) // 0..1 last scroll value (change of page scroll offset)
    //console.log(e.target.rect) // element.getBoundingClientRect() result
    if(e.type == "enter")//(e.percentInView >= 0.8 && e.percentTop >= 0.8 && e.percentCenter >= 0.8 && e.scrollPercent >= 0.8)
    {
      this.$root.$emit('footerInView');
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$post-body-color: rgb(2, 16, 32);
$text-color: #ccc;

$post-body-border-color: rgb(56, 68, 77);

.footer {
  text-align: center;
  clear: both;
  position: relative;
  height: 5em;
  color: $text-color;
  background-color: $post-body-color;
}
</style>
