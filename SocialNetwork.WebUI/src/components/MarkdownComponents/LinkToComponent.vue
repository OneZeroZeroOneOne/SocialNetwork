<template>
  <span v-html="'>>' + block_data.attr.id"
    :class="{'link-to': true, 'is-exist': isExist}"
    @mouseover="makeHovered"
    @mouseleave="hovered = false"
  ></span>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { Guid } from "@/utilities/guid";

import { ResponseState } from "@/models/enum/ResponseState";
import { IPost } from "@/models/responses/PostViewModel";

import AttachmentComponent from '../components/AttachmentComponent.vue';
import CommentComponent from "@/components/CommentComponent.vue";

import eventBus from "@/utilities/EventBus";
import { IMarkdownNode } from '../../models/responses/MarkdownNode';

@Component({
  components: {}
})
export default class LinkToComponent extends Vue {
  @Prop() public comment!: string;
  @Prop() public post!: string;
  @Prop() public all_blocks!: IMarkdownNode[];

  //@Prop() public text!: string;

  @Prop() public block_data!: IMarkdownNode;

  public hovered: boolean = false;
  public showing: boolean = true;
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
    eventBus.emit('new-link-to-component', this)
  }

  destroy(): void {
    eventBus.emit('destroy-link-to-component', this)
  }

  makeHovered(event: any) {
    console.log('Can show '+this.showing)
    if (this.isExist)
    {
      if (this.showing === true)
      {
        console.log(this.comment, this.post)
        this.hovered = true;
        this.showing = false;

        let commentObj = {
          id: 1,
          date: new Date(),
          attachmentComment: [],
          text: "qweqweqweqwe"
        }

        this.$root.$emit('show-link-component', this.post, commentObj, event.pageX, event.pageY)
      }
    }
  }

  @Watch('showing', {immediate: true})
  changedshowing(value: boolean): void {
    //console.log('SHOWING ' + value)
    //console.log(this)
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
