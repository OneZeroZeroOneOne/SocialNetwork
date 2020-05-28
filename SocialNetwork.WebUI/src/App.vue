<template>
  <div id="app">
    <nav-bar-component/>
    <attachment-modal/>
    <router-view>
    </router-view>
    <show-post-comment-container/>
    <editor-modal/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import EditorModal from '@/components/Modals/EditorModal.vue'
import apiClient from '@/services/Implementations/ApiClient';
import AttachmentModal from "@/components/Modals/AttachmentModal.vue";
import ShowPostCommentContainer from '@/components/Containers/ShowPostCommentContainer.vue';
import AttachmentComponent from '@/components/Other/AttachmentComponent.vue';
import NavBarComponent from '@/components/Navigations/NavBarComponent.vue';

import { loadCss, onloadCSS } from '@/utilities/loadStyle';

import db from '@/services/Implementations/IDBService';

import eventBus from "@/utilities/EventBus";
import GlobalStorage from './services/Implementations/GlobalStorage';

@Component({
  components: {
    EditorModal,
    AttachmentModal,
    ShowPostCommentContainer,
    NavBarComponent,
    AttachmentComponent,
  }
})
export default class App extends Vue {
  public listModal: Vue[] = [] 

  constructor() {
    super();

    let defaultStyle = loadCss('/themes/default.css')
    onloadCSS(defaultStyle, () => {
      console.log('loaded default theme')
    })

    apiClient.updateToken()
  }

  mounted(): void {
    /*let iAm = document.getElementById('app');

    if (iAm === null)
      return;
    */
    document.addEventListener('mouseover', (e: Event) => {
      //console.log(e)
      // @ts-ignore
      if (e.target && e.target.matches('.link-to')) {
        // @ts-ignore
        //console.log(e.target.dataset)
        //console.log(e)
        eventBus.emit('show-link-component', e)
      }
    })
  }

  async created() {
    this.$refs.container = this;
    console.log(this)
    await GlobalStorage.getBoards()
  }
}
</script>

<style lang="scss">
@import '~vue-awesome-notifications/dist/styles/style.css';
@import './styles/scss/markdown.scss';
@import './styles/dragger.css';

html {
  background: url('assets/background-tiled-memphis.png');
  overflow-x: hidden;
}

body {
  margin: 5px;
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

  &:active{
    background-color:#999999;
  }
  &:hover{
    border-color:#7a7a7a;
    cursor: pointer;
  } 
}
</style>
