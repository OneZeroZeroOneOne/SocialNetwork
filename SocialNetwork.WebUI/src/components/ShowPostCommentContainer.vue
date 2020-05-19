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

@Component({
  components: {
    AttachmentComponent,
    PostComponent,
    CommentComponent
  }
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: object[] = [] 
  public modalDict: Map<number, number> = new Map<number, number>();
  public keyId: number = 0;
  public modals: number[] = [];

  private _commentService!: ICommentService;
  private _postService!: IPostService;

  public dissapearTimeout = 3 * 1000; //3 sec

  constructor() {
    super();
  }

  showComponent(event: MouseEvent) {
    let ComponentClass;
    let instance;
    //console.log(event)
    // @ts-ignore
    if (this.modals.indexOf(Number(event.target.dataset['id'])) > -1)//(this.modalDict.has(event.target.dataset['id']))
    {
      console.log('this modal already show')
      return;
    }
    // @ts-ignore
    this._commentService.getCommentById(event.target.dataset['id']).then(x => {
      //console.log(x)
      ComponentClass = Vue.extend(CommentComponent)

      instance = new ComponentClass({
        propsData: {
          obj: x.data,
          isModal: true,
          position: {
            x: event.pageX,
            y: event.pageY,
          }
        }
      });
      
      instance.$mount() // pass nothing
      //console.log(instance)

      this.$root.$el.appendChild(instance.$el);
      this.modals.push(Number(event.target.dataset['id']));

      /*let timer = setTimeout(() => {
        this.$root.$el.removeChild(instance.$el);
        this.modalDict.delete(event.target.dataset['id']);
      }, this.dissapearTimeout)

      this.modalDict.set(event.target.dataset['id'], timer);*/

      /*this.listModal.push({
        isComment: true,
        keyId: this.keyId++,
        obj: x.data,
        isModal: true,
        position: {
          x: event.pageX,
          y: event.pageY,
        }
      })*/
    }).catch(err => {
      ComponentClass = Vue.extend(CommentComponent)

      instance = new ComponentClass({
        propsData: {
          obj: {
            id: Number(event.target.dataset['id']),
            text: "Ошибка",
            attachmentComment: [],
          },
          isModal: true,
          position: {
            x: event.pageX,
            y: event.pageY,
          }
        }
      });
      
      instance.$mount() // pass nothing
      //console.log(instance)

      this.$root.$el.appendChild(instance.$el);
      this.modals.push(Number(event.target.dataset['id']));
    })
    
  }

  hideComponent(component: Vue|number) {
    this.modals = this.modals.filter(x => x !== component[1])
    this.$root.$el.removeChild(component[0].$el);
    //let comp = this.listModal.find(x => x.keyId == component.modalId)
    // @ts-ignore
    //this.listModal = this.listModal.filter(obj => obj.keyId !== component.modalId);
    /*console.log(component)
    this.$root.$el.removeChild(component.$el)*/
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
