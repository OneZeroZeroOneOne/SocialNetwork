<template>
  <div class="board-list">
    <div class="board-list-container" v-if="boardRequestStatus === 1">
      <card v-for="item in boards" 
      v-bind:key="item.id"
      :link-to="item.name"
      :data-images="['http://img0.joyreactor.cc/pics/post/art-%D0%BE%D0%BB%D0%B4%D1%84%D0%B0%D0%B3-2ch-4699278.jpeg', 'https://2ch.hk/char/src/2777/14717699796150.jpg']">
        <h1 slot="header">/{{item.name}}/</h1>
        <p slot="content">{{item.description}}</p>
      </card>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { ResponseState } from "@/models/enum/ResponseState";
import { Guid } from "@/utilities/guid";

import { IBoard } from "@/models/responses/Board";

import { IBoardService } from '@/services/Abstractions/IBoardService';

import { BoardService } from '../services/Implementations/BoardService';
import Card from '@/components/BoardCardComponent.vue';

import Nprogress from "nprogress"
import _ from 'lodash'

@Component({
  components: { 
    Card,
  }
})
export default class BoardListComponent extends Vue {
  private boardRequestStatus: ResponseState = ResponseState.loading;

  private boards: IBoard[] = [];

  private _boardService!: IBoardService;

  constructor() {
    super();
    this.loadBoards();
  }

  beforeCreate() {
    this._boardService = new BoardService();
  }

  async loadBoards(): Promise<void> {
    this.boardRequestStatus = ResponseState.loading;

    this._boardService.getBoards()
      .then(response => {
        this.boards = response.data;
        this.boardRequestStatus = ResponseState.success;
      })
      .catch(error => {
        this.boardRequestStatus = ResponseState.fail;
      });
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
$board-name-color: #ba6b57;
$board-description-color: #ffa41b;
$board-hover-color: #e7b2a5;

.board-list-container {
  //padding: 40px 80px;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}

.board-link {
    .board-name {
        //display: inline;
        color: $board-name-color;
        cursor: pointer;
    }

    .board-name:hover {
        color: $board-hover-color;
    }

    .board-description {
        //display: inline;
        color: $board-description-color;
        cursor: pointer;
    }
}
</style>
