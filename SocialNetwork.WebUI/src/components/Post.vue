<template>
  <div class="post">
    <b-field label="From Home">
      <label>Post ID: {{postId}}</label>
    </b-field>
    <b-field label="From Backend" v-if="isLoading === false">
      <label>Post Text: {{postObj.text}}</label>
    </b-field>
    <b-field label="From Backend" v-else>
      <label>Post Text: LOADING</label>
    </b-field>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { PostService } from "@/services/PostService";
import { IPost } from "@/models/responses/PostViewModel"

@Component({})
export default class PostComponent extends Vue {
  @Prop() public postId!: string;
  private postObj!: IPost; 
  private isLoading: boolean = true;
  constructor() {
    super();
    this.loadPost();
  }

  async loadPost(): Promise<void> {
    this.postObj = await PostService.getPost(this.postId);
    this.isLoading = false;
    console.log(this.postObj);
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
