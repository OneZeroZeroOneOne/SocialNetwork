<template>
  <div class="footer">
    <div class="footer-body">
    FOOTER
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";
import { ResponseState } from "@/models/enum/ResponseState";
import Nprogress from "nprogress"
import _ from 'lodash'

@Component({})
export default class FooterComponent extends Vue {
  constructor() {
    super();
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$post-body-color: rgb(2, 16, 32);
$text-color: #ccc;

$post-body-border-color: rgb(56, 68, 77);

.footer {
  left: 8px;  
  float: right;
  text-align: center;
  clear: both;
  position: relative;
  width: 100vw;
  height: 5em;
  color: $text-color;
  background-color: $post-body-color;
}
</style>
