<template>
  <div class="show-post-comment-container">
    <div v-for="ins in listModal" :key="ins.keyId">
      <component 
      :is="ins.isComment == true ? 'CommentComponent': 'PostComponent'" 
      :obj="ins.obj" 
      :isModal="true" 
      :linkToFather="ins.linkToFather"
      :modalId="ins.keyId"
      :position="ins.position"/>
    </div>
  </div>
</template>


<script lang="ts">
//<component :is="getEntityDependOnTag(block.tag)" :key="block.position" :block_data="block" :all_blocks="flattenedData"/>
import { Component, Prop, Vue } from "vue-property-decorator";
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';
import CommentComponent from "@/components/CommentComponent.vue";
import AttachmentComponent from '@/components/AttachmentComponent.vue';
import PostComponent from './PostComponent.vue';

import { IBoardService } from '@/services/Abstractions/IBoardService';
import { ICommentService }from '@/services/Abstractions/ICommentService';
import { IPostService } from '../services/Abstractions/IPostService';

import { BoardService } from '../services/Implementations/BoardService';
import { CommentService } from '../services/Implementations/CommentService';
import { PostService } from '../services/Implementations/PostService';

import globalStorage from '@/services/Implementations/GlobalStorage';
import { ResponseState } from '../models/enum/ResponseState';

@Component({
  components: {
    AttachmentComponent,
    PostComponent,
    CommentComponent
  }
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: object[] = [] 

  private _commentService!: ICommentService;
  private _postService!: IPostService;

  public dissapearTimeout = 3 * 1000; //3 sec

  constructor() {
    super();
  }

  async showComponent(event: MouseEvent) {

    // @ts-ignore
    if (event.target.dataset['id'] === undefined)
    {
      // @ts-ignore
      /*this.createComponent(event, 0, {
        id: 0,
        text: "Ошибка",
        attachmentComment: [],
      }, true);*/
      return;
    }

    // @ts-ignore
    let compId = Number(event.target.dataset['id'])
    // @ts-ignore
    if (this.listModal.find(x => x.keyId === compId))
    {
      console.log('this modal already show')
      return;
    }

    //@ts-ignore
    let comment = await globalStorage.getComment(event.target.dataset['id']);

    if (comment.state !== ResponseState.fail)
    {
      this.createComponent(event, compId, comment.value, true);
      return;
    }

    // @ts-ignore
    this.createComponent(event, compId, {
      id: compId,
      text: "Ошибка",
      attachmentComment: [],
    }, true);

    /*this._commentService.getCommentById(compId).then(x => {
      this.createComponent(event, compId, x.data, true);
    }).catch(err => {
      // @ts-ignore
      this.createComponent(event, compId, {
        id: compId,
        text: "Ошибка",
        attachmentComment: [],
      }, true);
    })*/
    
  }

  createComponent(event: MouseEvent, id: number, object: IPost|IComment, isComment: boolean) {
    this.listModal.push({
      isComment: isComment,
      keyId: id,
      obj: object,
      isModal: true,
      position: {
        x: event.pageX,
        y: event.pageY,
      }
    })
  }

  hideComponent(component: Vue|number) {
    // @ts-ignore
    this.listModal = this.listModal.filter(obj => obj.keyId !== component[1]);
  }

  beforeCreate() {
    //this._boardService = new BoardService();
    this._commentService = new CommentService();
    this._postService = new PostService();
  }

  beforeDestroy() {
    this.$root.$off('show-link-component', this.showComponent)
    this.$root.$off('hide-link-component', this.hideComponent)
  }

  mounted(): void {
    this.$root.$on('show-link-component', this.showComponent)
    this.$root.$on('hide-link-component', this.hideComponent)
  }

  created() {
    //
  }
}
</script>

<style lang="scss">

</style>
