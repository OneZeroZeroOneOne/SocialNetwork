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
import { ICommentService } from '../../services/Abstractions/ICommentService';
import { IPostService } from '../../services/Abstractions/IPostService';
import { CommentService } from '../../services/Implementations/CommentService';
import { PostService } from '../../services/Implementations/PostService';
import { IComment } from '../../models/responses/CommentViewModel';


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
  public isExist: boolean = false;
  
  public id: number = 0;
  public isPost: boolean = false;
  public isComment: boolean = false;

  private _commentService!: ICommentService;
  private _postService!: IPostService;

  constructor() {
    super();
  }

  beforeCreate() {
    this._commentService = new CommentService();
    this._postService = new PostService();

    //this.$root.$on('hide-link-component', this.hiding)
  }

  hiding(component: Vue)
  {
    console.log(component)
  }

  mounted(): void {
    /*if (this.comment === '0')
    {
      if (this.post === '0')
      {
        this.isExist = false;
      }
    }*/
    if (this.block_data.attr.id !== undefined)
    {
      this.id = this.block_data.attr.id;

      if (this.block_data.attr.isPost !== undefined && this.block_data.attr.isPost === "true")
      {
        this.isPost = true;
        this.isExist = true;
      }

      if (this.block_data.attr.isComment !== undefined && this.block_data.attr.isComment === "true")
      {
        this.isComment = true;
        this.isExist = true;
      }
    }
    eventBus.emit('new-link-to-component', this)
  }

  destroy(): void {
    eventBus.emit('destroy-link-to-component', this)
  }

  makeHovered(event: any) {
    console.log('Can show '+this.showing)
    console.log('BLOCK DATA')
    console.log(this.block_data)
    if (this.isExist)
    {
      if (this.showing === true)
      {
        this.hovered = true;
        this.showing = false;

        if (this.isComment)
        {
          this._commentService.getCommentById(this.id).then(x => {
            
            this.$root.$emit('show-link-component', true, x.data, event.pageX, event.pageY, this)
          })
          return;
        }

        if (this.isPost)
        {
          this._postService.getPostGlobal(this.id).then(x => {

            this.$root.$emit('show-link-component', false, x.data, event.pageX, event.pageY, this)
          })
          return;
        }
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
