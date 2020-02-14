<template>
  <footer class="footer">This footer will always be positioned at the bottom of the page, but <strong>not fixed</strong>.</footer>
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
.footer {
  position: relative;
  text-align: center;
  width:100%;
  height:60px;			/* Height of the footer */
  background:#6cf;
}
</style>
