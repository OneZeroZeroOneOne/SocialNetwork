<template>
  <div id="app">
    <div id="nav"></div>
    <router-view/>
    <FooterComponent/>
    <notifications group="foo" position="bottom right"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import FooterComponent from "@/components/FooterComponent.vue";

@Component({
  components: {
    FooterComponent
  }
})
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
