<template>
  <div id="app">
    <div id="nav"></div>
    <router-view>
    </router-view>
    <notifications group="foo" position="bottom right"/>
    <preview-modal/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import PreviewModal from '@/components/PreviewModal.vue'

@Component({
  components: {
    PreviewModal,
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

.noselect {
  -webkit-touch-callout: none; /* iOS Safari */
  -webkit-user-select: none; /* Safari */
    -khtml-user-select: none; /* Konqueror HTML */
      -moz-user-select: none; /* Old versions of Firefox */
      -ms-user-select: none; /* Internet Explorer/Edge */
          user-select: none; /* Non-prefixed version, currently
                                supported by Chrome, Opera and Firefox */
}

.button {
  padding:0.2em 1.45em;
  border:0.15em solid #CCCCCC;
  box-sizing: border-box;
  text-decoration:none;
  font-weight:400;
  color:#000000;
  background-color:#CCCCCC;
  text-align: center;
  position:relative;
}
  
.button:active{
  background-color:#999999;
}
  
.button:hover{
  border-color:#7a7a7a;
  cursor: pointer;
}
</style>
