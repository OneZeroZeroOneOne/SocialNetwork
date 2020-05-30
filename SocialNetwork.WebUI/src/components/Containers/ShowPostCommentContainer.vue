<template>
  <div class="show-post-comment-container">
    <div v-for="ins in listModal" :key="ins.keyId">
      <component class="abs-pos"
      @unhovered="unhovered"
      @hovered="hovered"
      @hided="hidedComponent"
      :ref="'modal'+ins.keyId"
      :is="ins.isComment == true ? 'CommentComponent': 'PostComponent'" 
      :keyId="ins.keyId"
      :obj="ins.obj"
      :fatherPost="ins.fatherPost"
      :event="ins.event"
      :isModal="true"
      :modalStyles="ins.modalStyles"/>
    </div>
  </div>
</template>


<script lang="ts">
//class="animate__animated animate__fadeInUp"
import { Component, Prop, Vue } from "vue-property-decorator";
import { IPost } from '@/models/responses/PostViewModel';
import { IComment } from '@/models/responses/CommentViewModel';
import CommentComponent from "@/components/Contents/CommentComponent.vue";
import AttachmentComponent from '@/components/Other/AttachmentComponent.vue';
import PostComponent from '@/components/Contents/PostComponent.vue';

import globalStorage from '@/services/Implementations/GlobalStorage';
import { ResponseState } from '@/models/enum/ResponseState';
import EventBus from '../../utilities/EventBus';
import animateCSS  from '../../utilities/AnimateCSS';

class ModalEntity {
  public isComment: boolean
  public keyId: number
  public obj: IPost|IComment
  public fatherPost: IPost
  public event: MouseEvent
  public elem: Element
}

class ThreadLink {
  public id: number;
  public linkTo: number;
  public linkFrom: number;

  public dissapearTimer: number;

  public isHovered: boolean;
  public isMain: boolean;
  public isHided: boolean;

  public childrens: ThreadLink[];
}

@Component({
  components: {
    AttachmentComponent,
    PostComponent,
    CommentComponent
  }
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: ModalEntity[] = [] 
  private keyId: number = 0;

  public threadLinks: ThreadLink[] = [];
  public lastUnhovered: number = 0;
  public lastHovered: number = 0;

  //public threadMaster: Map<number, number> = new Map<number, number>();
  //public threadLinks: 

  public dissapearTimeout = 3 * 1000; //3 sec

  constructor() {
    super();
  }

  async showComponent(event: MouseEvent) 
  {
    if (event.target == null)
      return;

    let elem: HTMLLinkElement = event.target as HTMLLinkElement;

    if (elem.classList.contains('showing'))
    {
      //console.log('this modal already show')
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
          this.createComponent(event, comment.value, post.value, true);
          return;
        }
      }

      let post = await globalStorage.getPostGlobally(compId);

      if (post.state !== ResponseState.fail)
      {
        this.createComponent(event, post.value, post.value, true);
        return;
      }

      // @ts-ignore
      this.createComponent(event, {
        id: Number(compId),
        text: "Ошибка",
        attachmentComment: [],
      },{
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
          this.createComponent(event, comment.value, post.value, true);
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
        this.createComponent(event, post.value, post.value, false);
        return;
      }
    }

    // @ts-ignore
    this.createComponent(event, {
      id: Number(compId),
      text: "Ошибка",
      attachmentComment: [],
    }, {
      id: Number(compId),
      text: "Ошибка",
      attachmentComment: [],
    }, true);
  }

  hovered(keyId: number) {
    console.log('hovered', keyId);
    let father = this.findLinkedFather(keyId);

    let currentLink = this.threadLinks.filter(x => x.linkTo === keyId)
    if (currentLink[0] !== undefined)
    {
      currentLink[0].isHovered = true;
    }

    if (father !== undefined)
    {
      if (father.dissapearTimer !== undefined || father.dissapearTimer != null)
      {
        clearTimeout(father.dissapearTimer)
      }
    }

  }
  
  unhovered(keyId: number) {
    console.log('unhovered', keyId);
    let father = this.findLinkedFather(keyId);

    let currentLink = this.threadLinks.filter(x => x.linkTo === keyId)
    if (currentLink[0] !== undefined)
    {
      currentLink[0].isHovered = false;
    }

    if (father !== undefined)
    {

      if (father.dissapearTimer !== undefined || father.dissapearTimer != null)
      {
        clearTimeout(father.dissapearTimer)
      }

      if (father.childrens.filter(x => x.isHovered === true).length === 0)
      {
        father.dissapearTimer = setTimeout(() => {
          this.hideThread((father as ThreadLink).linkTo);
        }, this.dissapearTimeout)
      }
      //console.log('last unhovered', this.lastUnhovered);
    }
  }

  findLinkedFather(keyId: number): ThreadLink|undefined {
    let linkObj = this.threadLinks.find(x => x.linkTo === keyId);

    let found = false;
    let father: ThreadLink;

    while(found === false)
    {
      // @ts-ignore
      if (linkObj.isMain === true)
        return linkObj;
      // @ts-ignore
      linkObj = this.threadLinks.find(x => x.linkTo === linkObj.linkFrom)
    }
  }

  findAllFather(): ThreadLink[] {
    return this.threadLinks.filter(x => x.isMain === true);
  }

  createComponent(event: MouseEvent, object: IPost|IComment, fatherPost: IPost, isComment: boolean) {
    let elem: HTMLLinkElement = event.target as HTMLLinkElement;

    let curKeyId = this.keyId++;

    // @ts-ignore
    let check = elem.parentNode.dataset;

    //console.log(check)

    let threadLink = new ThreadLink()
    threadLink.isHided = false;
    threadLink.id = object.id
    threadLink.linkTo = curKeyId;

    if (check['fatherKeyid'] === undefined || check['fatherKeyid'] === null)
    {
      console.log('first', check['fatherKeyid'])
      //this.threadMaster.set(curKeyId, object.id)
      threadLink.isMain = true;
      threadLink.dissapearTimer = setTimeout(() => {
        this.hideThread(curKeyId);
      }, this.dissapearTimeout)
      threadLink.childrens = [];
    } else {
      threadLink.linkFrom = Number(check['fatherKeyid'])
      let parent = this.findLinkedFather(threadLink.linkFrom)
      if (parent !== undefined)
        parent.childrens.push(threadLink);
    }

    this.threadLinks.push(threadLink);
    //console.log(threadLink);

    let ent = new ModalEntity();
    ent.isComment = isComment;
    ent.keyId = curKeyId;
    ent.obj = object;
    ent.fatherPost = fatherPost;
    ent.elem = elem;
    ent.event = event;

    this.listModal.push(ent);
  }

  hidedComponent(keyId: number) {
    console.log('hided', keyId)

    let thisModal = this.threadLinks.find(x => x.linkTo === keyId)

    if (thisModal !== undefined)
    {
      thisModal.isHided = true;
    }

    let allThreadFather = this.findLinkedFather(keyId);

    if (allThreadFather !== undefined 
      && allThreadFather.childrens.every(x => x.isHided === true)
      && allThreadFather.isHided === true)
    {
      let allThread = allThreadFather.childrens.map(x => x.linkTo);
      allThread.push(allThreadFather.linkTo);

      allThread.forEach(element => {
        let domObj = this.listModal.find(x => x.keyId === element)
        if (domObj !== undefined)
        {
          domObj.elem.classList.remove('showing')
        }
      });

      this.listModal = this.listModal.filter(obj => allThread.lastIndexOf(obj.keyId) === -1)//obj.keyId !== curKeyId);
    }
  }

  hideThread(curKeyId: number) {
    let allThreadFather = this.findLinkedFather(curKeyId);

    if (allThreadFather === undefined)
      return;

    let allThread = allThreadFather.childrens.map(x => x.linkTo);
    allThread.push(curKeyId);

    let obj = this.threadLinks.find(obj => obj.linkTo === (allThreadFather as ThreadLink).linkTo)

    if (obj !== undefined)
    {
      //obj.elem.classList.remove('showing');
      
      let toHide = this.listModal.filter(obj => allThread.lastIndexOf(obj.keyId) !== -1)

      toHide.forEach(element => {
        EventBus.emit('hide-component', element.keyId)
      });

      //this.listModal = this.listModal.filter(obj => allThread.lastIndexOf(obj.keyId) === -1)//obj.keyId !== curKeyId);
    } else {
      console.warn('obj is undefined', curKeyId, allThreadFather)
      console.log(this.threadLinks)
      console.log(this.listModal)
    }
  }

  /*hideComponent(component: Vue, id: number) {
    // @ts-ignore
    let obj = this.listModal.find(obj => obj.keyId === id)
    // @ts-ignore
    obj.elem.classList.remove('showing');

    // @ts-ignore
    this.listModal = this.listModal.filter(obj => obj.keyId !== id);
  }*/

  beforeCreate() {
    //
  }

  beforeDestroy() {
    EventBus.unsubscribe('show-link-component', this.showComponent)
    //EventBus.unsubscribe('hide-link-component', this.hideComponent)
  }

  mounted(): void {
    EventBus.subscribe('show-link-component', this.showComponent)
    //EventBus.subscribe('hide-link-component', this.hideComponent)
  }

  created() {
    //
  }
}
</script>

<style lang="scss">
.abs-pos {
  position: absolute;
  z-index: 9999;
}
</style>
