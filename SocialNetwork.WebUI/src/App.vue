<template>
  <div id="app">
    <div id="nav"></div>
    <router-view/>
    <notifications group="foo" position="bottom right"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class App extends Vue {
  constructor() {
    super();
  }
}
</script>

<style lang="scss">
html {
  background: url('assets/background-tiled-memphis.png');
  font-family: BlinkMacSystemFont, -apple-system, "Segoe UI", "Roboto", "Oxygen", "Ubuntu", "Cantarell", "Fira Sans", "Droid Sans", "Helvetica Neue", "Helvetica", "Arial", sans-serif;
}
</style>
