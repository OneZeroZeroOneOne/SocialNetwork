<template>
  <span 
    class="link-to"
    @mouseover="makeHovered"
    @mouseleave="hovered = false"
  >{{getSource()}}</span>
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
  
  constructor() {
    super();
  }

  getSource(): string {
    return this.comment === null ? this.post : this.comment;
  }

  makeHovered(event: any) {
    console.log(event)
    var ComponentClass = Vue.extend(CommentComponent)
    var instance = new ComponentClass({
      propsData: {
        commentObj: {
          id: 1,
          date: new Date(),
          attachmentComment: [],
          text: "qweqweqweqwe"
        },
        isModal: true,
      }
    });

    instance.$mount() // pass nothing
    console.log(instance)
    let el = instance.$el as any;
    el.attributeStyleMap.set('position', 'absolute')
    el.attributeStyleMap.set('left', event.pageX + "px")
    el.attributeStyleMap.set('top', event.pageY + "px")
    el.attributeStyleMap.set('width', '80%')
    this.$root.$el.appendChild(instance.$el)
    setTimeout(x => this.$root.$el.removeChild(instance.$el), 1000)
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
