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
import { IPostService } from '../services/Abstractions/IPostService';
import AttachmentComponent from '@/components/AttachmentComponent.vue';
import PostComponent from './PostComponent.vue';

@Component({
  components: {
    AttachmentComponent,
    PostComponent,
    CommentComponent
  }
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: object[] = [] 
  public keyId: number = 0;

  constructor() {
    super();
  }

  showComponent(isComment: boolean, object: IComment|IPost, x: number, y: number, linkToComponentFather: Vue) {
    /*let ComponentClass;

    if (isComment === true)
      ComponentClass = Vue.extend(CommentComponent)
    else
      ComponentClass = Vue.extend(PostComponent)

    var instance = new ComponentClass({
      propsData: {
        obj: object,
        isModal: true,
        linkToFather: linkToComponentFather,
      }
    });

    instance.$mount() // pass nothing
    console.log(instance)

    this.$root.$el.appendChild(instance.$el)

    let el = instance.$el as any;
    el.attributeStyleMap.set('position', 'absolute')
    el.attributeStyleMap.set('left', (x - 20) + "px")
    el.attributeStyleMap.set('top', y + "px")
    el.attributeStyleMap.set('width', '80%')*/
    this.listModal.push({
      isComment: isComment,
      keyId: this.keyId++,
      obj: object,
      isModal: true,
      linkToFather: linkToComponentFather,
      position: {
        x: x,
        y: y,
      }
    })
  }

  hideComponent(component: Vue) {
    console.log(component)
    this.$root.$el.removeChild(component.$el)
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
