<template>
  <div class="board-name-header">
    <div v-on:click="goToBoard()" class="board-name-description" v-if="boardObj !== undefined">
        <div class="link">
            <div class="board-name">/{{boardObj.name}}/</div> - <div class="board-description">{{boardObj.description}}</div>
        </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IBoard } from '../models/responses/Board';

@Component({})
export default class BoardNameHeaderComponent extends Vue {
    @Prop() public boardObj!: IBoard;

    constructor() {
        super();
    }

    goToBoard(): void {
        let toRoute = '/'+this.boardObj.name;
        if (this.$router.currentRoute.path !== toRoute)
            this.$router.push({path: toRoute});
        else
            location.reload();
    }
}
</script>

<style lang="scss" scoped>
.link {
  cursor: pointer;
  width: max-content;
  height: max-content;
}

.board-name-description {
    display: flex;
    align-items: center;
    justify-content: center;
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