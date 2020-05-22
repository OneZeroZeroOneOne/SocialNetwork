<template>
  <div class="show-post-comment-container">
    <div v-for="ins in listModal" :key="ins.keyId">
      <component 
      :is="ins.isComment == true ? 'CommentComponent': 'PostComponent'" 
      :keyId="ins.keyId"
      :obj="ins.obj" 
      :isModal="true" 
      :linkToFather="ins.linkToFather"
      :modalId="ins.keyId"
      :position="ins.position"/>
    </div>
  </div>
</template>


<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IPost } from '@/models/responses/PostViewModel';
import { IComment } from '@/models/responses/CommentViewModel';
import CommentComponent from "@/components/Contents/CommentComponent.vue";
import AttachmentComponent from '@/components/Other/AttachmentComponent.vue';
import PostComponent from '@/components/Contents/PostComponent.vue';

import globalStorage from '@/services/Implementations/GlobalStorage';
import { ResponseState } from '@/models/enum/ResponseState';

@Component({
  components: {
    AttachmentComponent,
    PostComponent,
    CommentComponent
  }
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: object[] = [] 
  private keyId: number = 0;

  public dissapearTimeout = 3 * 1000; //3 sec

  constructor() {
    super();
  }

  offset(el: HTMLElement) {
    let rect = el.getBoundingClientRect(),
        scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,
        scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    return { 
      top: rect.top + scrollTop, 
      left: rect.left + scrollLeft 
    }
}

  async showComponent(event: MouseEvent) 
  {
    // @ts-ignore
    //console.log(event.target);

    if (event.target == null)
      return;

    let elem: HTMLLinkElement = event.target as HTMLLinkElement;

    if (elem.classList.contains('showing'))
    {
      console.log('this modal already show')
      return;
    }

    elem.classList.add('showing');

    let compId: string = "1";

    // @ts-ignore
    if (event.target.dataset['comment'] !== undefined && event.target.dataset['post'] !== undefined)
    {
      // @ts-ignore
      compId = event.target.dataset['comment']
      let comment = await globalStorage.getComment(compId);

      if (comment.state !== ResponseState.fail)
      {
        this.createComponent(compId, comment.value, true, elem);
        return;
      }

      let post = await globalStorage.getPost(globalStorage.currentBoard.id, compId);

      if (post.state !== ResponseState.fail)
      {
        this.createComponent(compId, post.value, true, elem);
        return;
      }

      // @ts-ignore
      this.createComponent(event, compId, {
        id: Number(compId),
        text: "Ошибка",
        attachmentComment: [],
      }, true);
      return;
    }

    // @ts-ignore
    if (event.target.dataset['comment'] !== undefined)
    {
      // @ts-ignore
      compId = event.target.dataset['comment']

      let comment = await globalStorage.getComment(compId);

      if (comment.state !== ResponseState.fail)
      {
        this.createComponent(compId, comment.value, true, elem);
        return;
      }
    }

    // @ts-ignore
    if (event.target.dataset['post'] !== undefined)
    {
      // @ts-ignore
      compId = event.target.dataset['post']

      let post = await globalStorage.getPost(globalStorage.currentBoard.id, compId);

      if (post.state !== ResponseState.fail)
      {
        this.createComponent(compId, post.value, false, elem);
        return;
      }
    }

    // @ts-ignore
    this.createComponent(event, compId, {
      id: Number(compId),
      text: "Ошибка",
      attachmentComment: [],
    }, true);
  }

  createComponent(id: string, object: IPost|IComment, isComment: boolean, elem: HTMLLinkElement) {
    let coords = this.offset(elem);

    console.log(elem)

    let offsetHeight = 0;
    if (elem.offsetParent != null)
      //@ts-ignore
      offsetHeight = elem.offsetParent.offsetHeight;

    this.listModal.push({
      isComment: isComment,
      keyId: this.keyId++,
      obj: object,
      isModal: true,
      elem: elem,
      position: {
        x: coords.left + elem.offsetWidth / 2,
        y: coords.top + elem.offsetHeight,
      }
    })
  }

  hideComponent(component: Vue|number) {
    // @ts-ignore
    let obj = this.listModal.find(obj => obj.keyId === component[1])
    // @ts-ignore
    obj.elem.classList.remove('showing');

    // @ts-ignore
    this.listModal = this.listModal.filter(obj => obj.keyId !== component[1]);
  }

  beforeCreate() {
    //
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
