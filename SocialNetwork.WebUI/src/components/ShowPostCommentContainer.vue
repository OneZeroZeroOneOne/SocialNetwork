<template>
  <div class="show-post-comment-container">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IPost } from '../models/responses/PostViewModel';
import { IComment } from '../models/responses/CommentViewModel';
import CommentComponent from "@/components/CommentComponent.vue";

@Component({
  components: {}
})
export default class ShowPostCommentContainer extends Vue {
  public listModal: Vue[] = [] 

  constructor() {
    super();
  }

  showLinkTo(el: any): void {
    console.log(el)
    this.listModal.push(el);
    console.log(this.listModal);
  }

  showComponent(post: IPost, comment: IComment, x: number, y: number) {
    let ComponentClass;

    if (comment !== null)
        ComponentClass = Vue.extend(CommentComponent)

    var instance = new ComponentClass({
        propsData: {
            commentObj: comment,
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

  mounted(): void {
    console.log(this.$on)

    this.$root.$on('show-link-component', this.showComponent)
    this.$root.$on('hide-link-component', this.hideComponent)

    this.$root.$on('showed-link-to', this.showLinkTo)
  }

  created() {
    console.log(this.$root)
      //
  }
}
</script>

<style lang="scss">

</style>
