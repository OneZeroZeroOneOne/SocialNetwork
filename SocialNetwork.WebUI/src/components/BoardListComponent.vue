<template>
  <div class="board-list">
    <div class="container-wrapper popular">
      <div class="board-list-container" v-if="boardRequestStatus === 1">
        <card v-for="item in popularBoards" 
        v-bind:key="item.id"
        :link-to="item.name"
        :data-images="getBoardPreview(item)">
          <h1 slot="header">/{{item.name}}/</h1>
          <p slot="content">{{item.description}}</p>
        </card>
      </div>
    </div>
    <div class="container-wrapper other">
      <div class="thematics">
        Тематика
        <span v-for="item in thematicsBoards" v-bind:key="item.id" v-on:click="$router.push(item.name)">
          <span class='link'>/{{item.name}}/</span> - {{item.description}}
        </span>
      </div>
      <div class="politicians">
        Политика и новости
        <span v-for="item in politiciansBoards" v-bind:key="item.id" v-on:click="$router.push(item.name)">
          <span class='link'>/{{item.name}}/</span> - {{item.description}}
        </span>
      </div>
      <div class="non">
        Прочее
        <span v-for="item in otherBoards" v-bind:key="item.id" v-on:click="$router.push(item.name)">
          <span class='link'>/{{item.name}}/</span> - {{item.description}}
        </span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { ResponseState } from "@/models/enum/ResponseState";
import { Guid } from "@/utilities/guid";

import { IBoard, IBoards } from "@/models/responses/Board";

import { IBoardService } from '@/services/Abstractions/IBoardService';

import { BoardService } from '../services/Implementations/BoardService';
import Card from '@/components/BoardCardComponent.vue';

import Nprogress from "nprogress";

@Component({
  components: { 
    Card,
  }
})
export default class BoardListComponent extends Vue {
  private boardRequestStatus: ResponseState = ResponseState.loading;

  private boards: IBoard[] = [];

  private _boardService!: IBoardService;

  private popularBoards: IBoard[] = [];

  private thematicsBoards: IBoard[] = [];
  private politiciansBoards: IBoard[] = [];
  private otherBoards: IBoard[] = [];

  constructor() {
    super();
    this.loadBoards();
  }

  getBoardPreview(board: IBoard) {
    return board.settings.filter(x => x.name === "ImagePreview").map(x => x.value);
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
        this.sortBoards()
      })
      .catch(error => {
        this.boardRequestStatus = ResponseState.fail;
      });
  }

  sortBoards() {
    this.popularBoards = []
    this.thematicsBoards = []
    this.politiciansBoards = []
    this.otherBoards = []

    this.boards.forEach(board => {
      let popSetting = board.settings.filter(x => x.name === 'Popular')
      if (popSetting.length > 0)
        this.popularBoards.push(board)

      let thematicsSettings = board.settings.filter(x => x.name === 'Thematic')
      if (thematicsSettings.length > 0)
        this.thematicsBoards.push(board)

      let politicsSettings = board.settings.filter(x => x.name === 'Politicians')
      if (politicsSettings.length > 0)
        this.politiciansBoards.push(board)

      let otherSettings = board.settings.filter(x => x.name === 'Other')
      if (otherSettings.length > 0)
        this.otherBoards.push(board)
    })
  }
}
</script>

<style scoped lang="scss">
.board-list {
  display: inline-flex;
  flex-direction: column;
}

.container-wrapper {
  background-color: var(--main-board-list-background-color);
  display: inline-block;
  border-color: var(--main-board-list-border-color);
  border-style: solid;
  border-width: 1px;
  padding: 12px;
  &.other {
    text-align: left;
    margin-top: 15px;
    display: flex;
    column-gap: 30px;
    justify-content: space-between;

    span:first-child {
      padding-top: 10px !important;
    }

    span:hover {
      cursor: pointer;
      .link {
        color: var(--main-board-list-text-hover-color);
      }
    }

    span:not(.link) {
      padding: 1px;
    }

    div {
      padding: 10px;
      display: flex;
      flex-direction: column;
      margin-left:10px;
      margin-right:10px;
    }
    .link {
      color: var(--main-board-list-text-color);
    }
  }
}

.board-list-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
</style>
