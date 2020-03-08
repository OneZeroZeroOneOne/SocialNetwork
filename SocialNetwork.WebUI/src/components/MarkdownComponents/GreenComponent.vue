<template>
  <span class="green"><slot></slot></span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";

import { ResponseState } from "@/models/enum/ResponseState";
import { IPost } from "@/models/responses/PostViewModel";

import AttachmentComponent from '../components/AttachmentComponent.vue';
import CommentComponent from "./CommentComponent.vue";

@Component({
  components: {}
})
export default class GreenComponent extends Vue {
  constructor() {
    super();
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.green {
  width: fit-content;
  color: green;
  font-style: italic;
  &:before {
    content: ">";
  }
}
</style>
