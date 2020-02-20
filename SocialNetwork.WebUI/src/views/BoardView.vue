<template>
  <div class="board-view">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IBoard } from '../models/responses/Board';
import { Guid } from '../utilities/guid';
import { BoardService } from '../services/BoardService';
import { ResponseState } from "@/models/enum/ResponseState";

@Component({
  components: { 
  }
})
export default class BoardView extends Vue {
  private requestBoardStatus: ResponseState = ResponseState.loading;
  private currentBoardName: string = "";
  private boardObj!: IBoard;

  constructor() {
    super();
    this.currentBoardName = this.boardName();
    this.loadBoardByName(this.currentBoardName)
  }

  boardName(): string {
    console.log(this.$route)
    return this.$route.params.boardname;
  }

  async loadBoardByName(name: string): Promise<void> {
      BoardService.getBoardByName(name)
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
}
</script>

<style lang="scss" scoped>

</style>