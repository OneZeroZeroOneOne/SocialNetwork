<template>
  <div class="show-post-comment-container">
    <div v-for="ins in listModal" :key="ins.keyId">
      <component class="animate__animated animate__fadeInUp"
      :is="ins.isComment == true ? 'CommentComponent': 'PostComponent'" 
      :keyId="ins.keyId"
      :obj="ins.obj"
      :fatherPost="ins.fatherPost"
      :isModal="true"
      :modalStyles="ins.modalStyles"/>
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
import EventBus from '../../utilities/EventBus';

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

  offset(el: HTMLElement) 
  {
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
        let post = await globalStorage.getPostGlobally(comment.value.postId.toString())
        if (post.state !== ResponseState.fail)
        {
          this.createComponent(event, compId, comment.value, post.value, true);
          return;
        }
      }

      let post = await globalStorage.getPostGlobally(compId);

      if (post.state !== ResponseState.fail)
      {
        this.createComponent(event, compId, post.value, post.value, true);
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
        let post = await globalStorage.getPostGlobally(comment.value.postId.toString())
        if (post.state !== ResponseState.fail)
        {
          this.createComponent(event, compId, comment.value, post.value, true);
          return;
        }
      }
    }

    // @ts-ignore
    if (event.target.dataset['post'] !== undefined)
    {
      // @ts-ignore
      compId = event.target.dataset['post']

      let post = await globalStorage.getPostGlobally(compId);

      if (post.state !== ResponseState.fail)
      {
        this.createComponent(event, compId, post.value, post.value, false);
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

  newOffset(el: HTMLElement, xy: string) {
    let c = 0;
    while (el) {
        c += el[xy];
        el = el.offsetParent as HTMLElement;
    }
    return c;
  }

  createComponent(event: MouseEvent, id: string, object: IPost|IComment, fatherPost: IPost, isComment: boolean) {
    let elem: HTMLLinkElement = event.target as HTMLLinkElement;

    let coords = this.offset(elem);


    let scrW = document.body.clientWidth || document.documentElement.clientWidth;
    let scrH = window.innerHeight || document.documentElement.clientHeight;

    console.log(event.clientY, Math.floor(scrH * 0.75))

    let x = coords.left + elem.offsetWidth / 2;
    let y = coords.top + elem.offsetHeight;

    let xx = (x < scrW / 2 ? 'left:' + x : 'right:' + (scrW - x + 2)) + 'px;';
    let yy = (event.clientY < Math.floor(scrH * 0.75) ? 'top:' + y : 'bottom:' + (scrH - y - 4)) + 'px;';

    /*
      'left': x + 'px',
      'top': y +'px', 
    */
    console.log(xx, yy)
    let modalStyles = 'position: absolute;'+ xx + yy;

    this.listModal.push({
      isComment: isComment,
      keyId: this.keyId++,
      obj: object,
      fatherPost: fatherPost,
      modalStyles: modalStyles,
      elem: elem
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
    EventBus.unsubscribe('show-link-component', this.showComponent)
    EventBus.unsubscribe('hide-link-component', this.hideComponent)
  }

  mounted(): void {
    EventBus.subscribe('show-link-component', this.showComponent)
    EventBus.subscribe('hide-link-component', this.hideComponent)
  }

  created() {
    //
  }
}
</script>

<style lang="scss">

</style>
