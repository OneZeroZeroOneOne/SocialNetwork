<template>
    <div class="board-list">
        <div class="board-list-container" v-if="boardRequestStatus === 1">
        <li v-for="item in boards" v-bind:key="item.id" class="board-link">
            <span @click="$router.push(item.name)">
                <div class="board-name">/{{item.name}}/</div> - <div class="board-description">{{item.description}}</div>
            </span>
        </li>
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

import Nprogress from "nprogress"
import _ from 'lodash'

@Component({})
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
    text-align: left;
}

.board-link {
    .board-name {
        display: inline;
        color: $board-name-color;
        cursor: pointer;
    }

    .board-name:hover {
        color: $board-hover-color;
    }

    .board-description {
        display: inline;
        color: $board-description-color;
        cursor: pointer;
    }
}
</style>
