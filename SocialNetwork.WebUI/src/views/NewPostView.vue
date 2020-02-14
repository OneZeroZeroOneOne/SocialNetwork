<template>
  <div class="posts">
    <ul id="posts">
      <li v-for="post in postObjs"/>
        <div class="post">
          <PostComponent :postObj="post" v-if="requestPostStatus === 1"/>
          <ul id="comments">
            <li v-for="(item, index) in commentObjs" v-bind:key="item.id">
              <CommentComponent :commentObj="item" :commentNum="index+1"/>
            </li>
          </ul>
        </div>
      </li>
    </ul>
  </div>
</template>


<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import PostComponent from '@/components/PostComponent.vue'
import CommentComponent from "@/components/CommentComponent.vue";
import { ResponseState } from "@/models/enum/ResponseState";
import { IPagedResult } from '../models/responses/PagedResult';
import { IPost } from "@/models/responses/PostViewModel";
import { IComment } from '../models/responses/CommentViewModel';
import { Guid } from "@/utilities/guid";
import { PostService } from "@/services/PostService";
import { CommentService } from "@/services/CommentService";

@Component({
  components: { 
    CommentComponent,
    PostComponent
  }
})
export default class NewPostView extends Vue {
  private requestCommentsStatus: ResponseState = ResponseState.loading;
  private requestPostsStatus: ResponseState = ResponseState.loading;
  private postObjs!: IPost[];
  private commentObjs: IComment[] = [];
  private currentPage: number = 1;
  private scrolledToBottom: boolean = false;

    constructor() {
    super();
  }
}
</script>