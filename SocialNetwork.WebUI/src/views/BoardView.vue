<template>
  <div class="board-view">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IBoard } from '../models/responses/Board';
import { Guid } from '../utilities/guid';

@Component({
  components: { 
  }
})
export default class BoardView extends Vue {
  private currentBoardName: string = "";
  private boardObj!: IBoard;

  constructor() {
    super();
    this.currentBoardName = this.boardName();
  }

  boardName(): string {
    return this.$route.path;
  }

  async loadBoardByName(name: string): Promise<void> {
      
  }
}
</script>

<style lang="scss" scoped>

</style>