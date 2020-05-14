<template>
  <div id="app">
    <nav-bar-component/>
    <new-modal/>
    <router-view>
    </router-view>
    <show-post-comment-container/>
    <editor-modal/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import EditorModal from '@/components/EditorModal.vue'
import apiClient from '@/services/Implementations/ApiClient';
import NewModal from "@/components/NewModal.vue";
import ShowPostCommentContainer from '@/components/ShowPostCommentContainer.vue';
import NavBarComponent from '@/components/NavBarComponent.vue';

import eventBus from "@/utilities/EventBus";

@Component({
  components: {
    EditorModal,
    NewModal,
    ShowPostCommentContainer,
    NavBarComponent,
  }
})
export default class App extends Vue {
  public listModal: Vue[] = [] 

  constructor() {
    super();
    apiClient.updateToken()
  }

  mounted(): void {
    //
    this.$root.$on('new-link-to-component', this.new_link_to)
    this.$root.$on('destroy-link-to-component', this.destroy_link_to)
  }

  destroy_link_to(): void {
    console.log('destroy link to')
  }

  new_link_to(): void {
    //console.log('new link to')
  }

  created() {
    this.$refs.container = this;
    console.log(this.$refs)
  }
}
</script>

<style lang="scss">
@import '~vue-awesome-notifications/dist/styles/style.css';

img {
  width: 100%;
  height: 100%;
}
</style>

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

<style>
.vb {
  margin-top: 10px;
  margin-bottom: 10px;
}

.vb > .vb-dragger {
  z-index: 5;
  width: 12px;
  right: 0;
}

.vb > .vb-dragger > .vb-dragger-styler {
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  -webkit-transform: rotate3d(0,0,0,0);
  transform: rotate3d(0,0,0,0);
  -webkit-transition:
      background-color 100ms ease-out,
      margin 100ms ease-out,
      height 100ms ease-out;
  transition:
      background-color 100ms ease-out,
      margin 100ms ease-out,
      height 100ms ease-out;
  background-color: rgba(48, 121, 244,.1);
  margin: 5px 5px 5px 0;
  border-radius: 20px;
  height: calc(100% - 10px);
  display: block;
}

.vb.vb-scrolling-phantom > .vb-dragger > .vb-dragger-styler {
  background-color: rgba(48, 121, 244,.3);
}

.vb > .vb-dragger:hover > .vb-dragger-styler {
  background-color: rgba(48, 121, 244,.5);
  margin: 0px;
  height: 100%;
}

.vb.vb-dragging > .vb-dragger > .vb-dragger-styler {
  background-color: rgba(48, 121, 244,.5);
  margin: 0px;
  height: 100%;
}

.vb.vb-dragging-phantom > .vb-dragger > .vb-dragger-styler {
  background-color: rgba(48, 121, 244,.5);
}
</style>
