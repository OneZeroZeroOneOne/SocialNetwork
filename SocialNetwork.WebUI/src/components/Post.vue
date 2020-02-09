<template>
  <div class="post">
    <b-field label="Post Id">
      <b-input v-model="postId"></b-input>
      <b-button @click="loadPost">Load Post</b-button>
    </b-field>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { PostService } from "@/services/PostService";

@Component({})
export default class Post extends Vue {
  public postId: string = "";
  constructor() {
    super();
  }

  async loadPost(): Promise<void> {
    let post = await PostService.getPost(this.postId);
    console.log(post);
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">

</style>
