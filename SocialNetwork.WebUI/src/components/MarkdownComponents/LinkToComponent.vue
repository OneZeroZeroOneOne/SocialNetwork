<template>
  <span class="link-to">{{getSource()}}</span>
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
export default class LinkToComponent extends Vue {
  @Prop() public comment!: string;
  @Prop() public post!: string;
  
  constructor() {
    super();
  }

  getSource(): string {
    return this.comment === null ? this.post : this.comment;
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.link-to {
  width: fit-content;
  color: orange;
  
  &:before {
    content: ">>";
  }

  &:hover {
    color: orangered;
    cursor: pointer;
  }
}
</style>
