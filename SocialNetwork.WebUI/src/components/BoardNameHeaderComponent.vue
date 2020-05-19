<template>
  <div class="board-name-header">
    <div v-on:click="goToBoard()" class="board-name-description" v-if="boardObj !== undefined">
        <div class="link">
            <div class="board-name">/{{boardObj.name}}/</div> - <div class="board-description">{{boardObj.description}}</div>
        </div>
    </div>
    <div class="create-thread">
        <div class="text" @click="openNewThreadEditor">
            Создать тред
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

    openNewThreadEditor(): void {
        console.log('new thread')
        this.$root.$emit('show-editor-modal-new-thread', this.boardObj)
    }
}
</script>

<style lang="scss" scoped>
.text {
    display: inline-block;
    left: 50%;
    position: relative;
    transform: translateX(-50%);
    font-size: 1.5em;
    text-align: center;
    cursor: pointer;
    color: var(--board-header-text-color);

    &:hover {
        color: var(--board-header-text-hover-color);
    }
}

.create-thread:before, .create-thread:after {
    content: "";
    height: 2px;
    background: linear-gradient(to right,  var(--board-header-gradient-from) 0%,var(--board-header-gradient-to) 50%,var(--board-header-gradient-from) 100%);

    display: block;
    margin-bottom: 5px;
    margin-top: 5px;
}

.link {
  cursor: pointer;
  width: max-content;
  height: max-content;
}

.board-name-description {
    display: flex;
    align-items: center;
    justify-content: center;
    color: white;
    text-align: center;
    font-size: 4em;  
    .board-description {
        display: inline;
        color: var(--board-header-text-color);
    }
    .board-name {
        display: inline;
        color: var(--board-header-text-hover-color);
    }
}
</style>