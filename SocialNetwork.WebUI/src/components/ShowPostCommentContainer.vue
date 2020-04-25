<template>
  <div class="show-post-comment-container">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';
import CommentComponent from "@/components/CommentComponent.vue";
import { IPostService } from '../services/Abstractions/IPostService';
import PostComponent from './PostComponent.vue';

@Component({
  components: {}
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: Vue[] = [] 

  constructor() {
    super();
  }

  showComponent(isComment: boolean, object: IComment|IPostService, x: number, y: number) {
    let ComponentClass;

    if (isComment === true)
      ComponentClass = Vue.extend(CommentComponent)
    else
      ComponentClass = Vue.extend(PostComponent)

    var instance = new ComponentClass({
      propsData: {
        obj: object,
        isModal: true,
      }
    });

    instance.$mount() // pass nothing
    console.log(instance)

    this.$root.$el.appendChild(instance.$el)

    let el = instance.$el as any;
    el.attributeStyleMap.set('position', 'absolute')
    el.attributeStyleMap.set('left', (x - 20) + "px")
    el.attributeStyleMap.set('top', y + "px")
    el.attributeStyleMap.set('width', '80%')
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
