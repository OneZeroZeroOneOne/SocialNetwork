<template>
  <div class="post">
    <b-field label="From Home">
      <label>Post ID: {{postId}}</label>
    </b-field>
    <b-field label="From Backend" v-if="requestStatus === 1">
      <label>Post Text: {{postObj.text}}</label>
    </b-field>
    <b-field label="From Backend" v-if="requestStatus === 0">
      <label>Post Text: LOADING</label>
    </b-field>
    <b-field label="From Backend" v-if="requestStatus === 2">
      <label>Post Text: error</label>
    </b-field>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { PostService } from "@/services/PostService";
import { IPost } from "@/models/responses/PostViewModel";
import { ResponseState } from "@/models/enum/ResponseState";
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

Vue.use(Buefy)
@Component({})
export default class PostComponent extends Vue {
  @Prop() public postId!: string;
  private postObj!: IPost; 
  private requestStatus: ResponseState = ResponseState.loading;

  constructor() {
    super();
    this.loadPost();
  }

  async loadPost(): Promise<void> {
    PostService.getPost(this.postId)
      .then(response => {
        this.postObj = response;
        this.requestStatus = ResponseState.success;
      })
      .catch(error => {
        this.requestStatus = ResponseState.fail;
      });
    console.log(this.postObj);
  }

}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
  .post {
    background-color: rgb(185, 182, 6);
    width: 30%;
    margin: auto;
    border: 4px ridge;
    border-color: rgb(165, 162, 9);
    border-width: 25;

  }
</style>
