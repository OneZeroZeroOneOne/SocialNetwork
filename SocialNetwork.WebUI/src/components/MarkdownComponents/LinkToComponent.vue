<template>
  <span 
    :class="{'link-to': true, 'is-exist': isExist}"
    @mouseover="makeHovered"
    @mouseleave="hovered = false"
  >>><slot></slot></span>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";

import { ResponseState } from "@/models/enum/ResponseState";
import { IPost } from "@/models/responses/PostViewModel";

import AttachmentComponent from '../components/AttachmentComponent.vue';
import CommentComponent from "@/components/CommentComponent.vue";

@Component({
  components: {}
})
export default class LinkToComponent extends Vue {
  @Prop() public comment!: string;
  @Prop() public post!: string;

  public hovered: boolean = false;
  public showing: boolean = false;
  public isExist: boolean = true;
  
  constructor() {
    super();
  }

  mounted(): void {
    if (this.comment === '0')
    {
      if (this.post === '0')
      {
        this.isExist = false;
      }
    }
  }

  makeHovered(event: any) {
    if (this.isExist)
    {
      if (!this.showing)
      {
        console.log(this.comment, this.post)
        this.hovered = true;

        let commentObj = {
          id: 1,
          date: new Date(),
          attachmentComment: [],
          text: "qweqweqweqwe"
        }

        this.$root.$emit('show-link-component', this.post, commentObj, event.pageX, event.pageY)
        this.showing = true;
      }
    }
  }

  @Watch('hovered', {immediate: true})
  changedHovered(value: boolean): void {
    if (value === true) {
      //
    }
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.link-to {
  &.is-exist {
    width: fit-content;
    color: orange;
    
    &:hover {
      color: orangered;
      cursor: pointer;
    }
  }
}
</style>
