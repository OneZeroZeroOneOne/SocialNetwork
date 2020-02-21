<template>
  <div class="board-view">
    <div class="board-name-description" v-if="requestBoardStatus == 1">
      <div class="board-name">{{boardObj.name}}</div> - <div class="board-description">{{boardObj.description}}</div>
    </div>
    <ul id="comments" v-if="requestPostsStatus === 1">
      <li v-for="(item, index) in postObjs" v-bind:key="item.id">
        <PostComponent :postObj="item" :postNum="index+1" :showEnterButton="true"/>
      </li>
    </ul>
    <li v-if="postObjs.length > 0">
      <FooterComponent/>
    </li>
    <li v-else>
      <FooterComponent class="foo"/>
    </li>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IBoard } from '../models/responses/Board';
import { Guid } from '../utilities/guid';
import { BoardService } from '../services/BoardService';
import { ResponseState } from "@/models/enum/ResponseState";
import { IPost } from '../models/responses/PostViewModel';
import PostComponent from '../components/PostComponent.vue'
import FooterComponent from "@/components/FooterComponent.vue";
import Nprogress from "nprogress"
import _ from 'lodash'

@Component({
  components: { 
    PostComponent,
    FooterComponent
  }
})
export default class BoardView extends Vue {
  private requestBoardStatus: ResponseState = ResponseState.loading;
  private requestPostsStatus: ResponseState = ResponseState.loading;
  private currentBoardName: string = "";
  private boardObj!: IBoard;

  private postObjs: IPost[] = [];
  private postIds: number[] = [];
  private currentPage: number = 1;

  constructor() {
    super();
    this.currentBoardName = this.boardName();
    this.loadBoardByName(this.currentBoardName)
      .then(x => {
        this.loadPagePosts()
      })
    
    this.$root.$on('footerInView', () => {
      this.throttleLoadPosts();
    })
  }

  throttleLoadPosts = _.throttle(() => this.loadPagePosts(), 2000);

  boardName(): string {
    return this.$route.params.boardname;
  }

  async loadBoardByName(name: string): Promise<void> {
    await BoardService.getBoardByName(name)
      .then(response => {
        if (response.status == 200)
        {
          this.boardObj = response.data;
          this.requestBoardStatus = ResponseState.success;
        } else {
          this.requestBoardStatus = ResponseState.fail;
          this.$router.push({name: "notfound"})
        }
      })
      .catch(error => {
        this.requestBoardStatus = ResponseState.fail;
        this.$router.push({name: "notfound"})
      });
  }

  async loadPagePosts(): Promise<void> {
    Nprogress.start()
    await BoardService.getPostsOnBoard(this.boardObj.id, this.currentPage, 10)
      .then(response => {
        if (response.status == 200)
        {
          if (response.data.currentPage < response.data.pageCount)
          {
            this.currentPage += 1;
          }

          let newPostCount: number = 0;
          console.log(response.data)
          response.data.results.forEach(x => {
            if (this.postIds.indexOf(x.id) === -1)
            {
              this.postIds.push(x.id);
              this.postObjs.push(x);
              newPostCount += 1;
            }
          })
          this.$notify({
            group: 'foo',
            title: 'Loaded posts',
            text: newPostCount === 0 ? 
              'No new posts' : 
              'Loaded ' + newPostCount.toString() + " posts",
          });

          this.requestPostsStatus = ResponseState.success
        } else {
          this.requestPostsStatus = ResponseState.fail;
          this.$router.push({name: "notfound"})
        }
      })
      .catch(error => {
        this.requestPostsStatus = ResponseState.fail;
        this.$router.push({name: "notfound"})
      });
    Nprogress.done();
  }
}
</script>

<style lang="scss" scoped>
.foo {
	position: absolute;
	left: 0;
	bottom: 0;
	width: 100%;
	height: 80px;
}

.board-name-description {
  margin-top: 10px;
  color: white;
  text-align: center;
  font-size: 4em;  
  .board-description {
    display: inline;
    color: orange;
  }
  .board-name {
    display: inline;
    color: orangered;
  }

}

</style>