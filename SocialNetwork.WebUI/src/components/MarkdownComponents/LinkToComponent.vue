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
            console.log(x);

            let commentObj = {
              id: 1,
              date: new Date(),
              attachmentComment: [],
              text: '{ "node_id": 1, "parent_id": 0, "node": "Element", "tag": "p", "child": [ { "node_id": 2, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 3, "parent_id": 2, "node": "Text", "text": "there is bold" } ] }, { "node_id": 5, "parent_id": 1, "node": "Element", "tag": "ins", "child": [ { "node_id": 6, "parent_id": 5, "node": "Text", "text": "bol " }, { "node_id": 7, "parent_id": 5, "node": "Element", "tag": "ins", "child": [ { "node_id": 8, "parent_id": 7, "node": "Text", "text": "in  " }, { "node_id": 9, "parent_id": 7, "node": "Element", "tag": "linktocomponent", "attr": { "id": "999" } }, { "node_id": 11, "parent_id": 7, "node": "Element", "tag": "del", "child": [ { "node_id": 12, "parent_id": 11, "node": "Text", "text": "qwe" } ] }, { "node_id": 13, "parent_id": 7, "node": "Text", "text": " side" } ] }, { "node_id": 14, "parent_id": 5, "node": "Text", "text": " d" } ] }, { "node_id": 15, "parent_id": 1, "node": "Text", "text": " inside " }, { "node_id": 16, "parent_id": 1, "node": "Element", "tag": "linktocomponent", "attr": { "id": "34" } }, { "node_id": 17, "parent_id": 1, "node": "Text", "text": " qwe " }, { "node_id": 18, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 19, "parent_id": 18, "node": "Text", "text": "there is bold" } ] } ] }'
            }

            this.$root.$emit('show-link-component', this.post, commentObj, event.pageX, event.pageY)
          })
          return;
        }

        if (this.isPost)
        {
          this._postService.getPostGlobal(this.id).then(x => {
            console.log(x);

            let commentObj = {
              id: 1,
              date: new Date(),
              attachmentComment: [],
              text: '{ "node_id": 1, "parent_id": 0, "node": "Element", "tag": "p", "child": [ { "node_id": 2, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 3, "parent_id": 2, "node": "Text", "text": "there is bold" } ] }, { "node_id": 5, "parent_id": 1, "node": "Element", "tag": "ins", "child": [ { "node_id": 6, "parent_id": 5, "node": "Text", "text": "bol " }, { "node_id": 7, "parent_id": 5, "node": "Element", "tag": "ins", "child": [ { "node_id": 8, "parent_id": 7, "node": "Text", "text": "in  " }, { "node_id": 9, "parent_id": 7, "node": "Element", "tag": "linktocomponent", "attr": { "id": "999" } }, { "node_id": 11, "parent_id": 7, "node": "Element", "tag": "del", "child": [ { "node_id": 12, "parent_id": 11, "node": "Text", "text": "qwe" } ] }, { "node_id": 13, "parent_id": 7, "node": "Text", "text": " side" } ] }, { "node_id": 14, "parent_id": 5, "node": "Text", "text": " d" } ] }, { "node_id": 15, "parent_id": 1, "node": "Text", "text": " inside " }, { "node_id": 16, "parent_id": 1, "node": "Element", "tag": "linktocomponent", "attr": { "id": "34" } }, { "node_id": 17, "parent_id": 1, "node": "Text", "text": " qwe " }, { "node_id": 18, "parent_id": 1, "node": "Element", "tag": "b", "child": [ { "node_id": 19, "parent_id": 18, "node": "Text", "text": "there is bold" } ] } ] }'
            }

            this.$root.$emit('show-link-component', this.post, commentObj, event.pageX, event.pageY)
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
