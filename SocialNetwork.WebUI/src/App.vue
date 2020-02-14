<template>
  <div id="app">
    <div id="nav"></div>
    <router-view/>
    <notifications group="foo" position="bottom right"/>
    <!--<FooterComponent/>-->
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
}

#app {
  display: relative;
}
</style>
