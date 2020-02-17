<template>
  <div class="main-view">
    <h1>16ch</h1>
    <h2>Welcome back</h2>
    <BoardListComponent/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import BoardListComponent from '@/components/BoardListComponent.vue';

@Component({
  components: { 
    BoardListComponent
  }
})
export default class MainView extends Vue {
  
}
</script>

<style lang="scss" scoped>
.main-view {
  text-align: center;
  position: absolute;
  width: 30%;
  min-width: 30%;
  top: 15%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
  color: white;
  font-family: BlinkMacSystemFont, -apple-system, "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", "Helvetica", "Arial", sans-serif;
  h1 {
    font-size: 3em;
    margin: 10px;
  }
  h2 {
    font-size: 1.5em;
  }
}
</style>