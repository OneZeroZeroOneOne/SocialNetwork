<template>
  <div id="app">
    <div id="nav"></div>
    <router-view>
    </router-view>
    <notifications group="foo" position="bottom right"/>
    <preview-modal/>
    <preview-video-modal/>
    <editor-modal/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import PreviewModal from '@/components/PreviewModal.vue'
import PreviewVideoModal from '@/components/PreviewVideoModal.vue';
import EditorModal from '@/components/EditorModal.vue'
import apiClient from '@/services/Implementations/ApiClient';

@Component({
  components: {
    PreviewModal,
    EditorModal,
    PreviewVideoModal,
  }
})
export default class App extends Vue {
  constructor() {
    super();
    apiClient.updateToken()
  }
}
</script>

<style lang="scss">
//editor and text styles
$color-black: #000000;
$color-white: #ffffff;
$color-grey: #dddddd;

$reply-color: rgb(19, 154, 11);

.menubar {
  display: flex;
  /* align-items: center; */
  justify-content: center;
  align-items: center;
  flex-direction: column;
}

.editor {
  position: relative;
  //max-width: 30rem;
  margin: 0rem 0rem 0rem 0rem;
  display: flex;
  flex-direction: row;

  &__content {
    margin-left: 1rem;
    overflow-wrap: break-word;
    word-wrap: break-word;
    word-break: break-word;
    overflow-y: scroll;
    height: calc(calc(300px - 20px) - 1rem);
    width: -webkit-fill-available;

    * {
      caret-color: currentColor;
    }

    pre {
      padding: 0.7rem 1rem;
      border-radius: 5px;
      background: $color-black;
      color: $color-white;
      font-size: 0.8rem;
      overflow-x: auto;

      code {
        display: block;
      }
    }

    p code {
      display: inline-block;
      padding: 0 0.4rem;
      border-radius: 5px;
      font-size: 0.8rem;
      font-weight: bold;
      background: rgba($color-black, 0.1);
      color: rgba($color-black, 0.8);
    }

    ul,
    ol {
      padding-left: 1.5rem;
    }

    li > p,
    li > ol,
    li > ul {
      margin: 0;
    }

    a {
      color: inherit;
    }

    blockquote { //it`s a reply
      margin-inline-start: 10px;
      color: $reply-color;
      font-style: italic;
      ::before {
        content: ">";
      }
      p {
        margin: 0;
      }
    }

    img {
      max-width: 100%;
      border-radius: 3px;
    }

    table {
      border-collapse: collapse;
      table-layout: fixed;
      width: 100%;
      margin: 0;
      overflow: hidden;

      td, th {
        min-width: 1em;
        border: 2px solid $color-grey;
        padding: 3px 5px;
        vertical-align: top;
        box-sizing: border-box;
        position: relative;
        > * {
          margin-bottom: 0;
        }
      }

      th {
        font-weight: bold;
        text-align: left;
      }

      .selectedCell:after {
        z-index: 2;
        position: absolute;
        content: "";
        left: 0; right: 0; top: 0; bottom: 0;
        background: rgba(200, 200, 255, 0.4);
        pointer-events: none;
      }

      .column-resize-handle {
        position: absolute;
        right: -2px; top: 0; bottom: 0;
        width: 4px;
        z-index: 20;
        background-color: #adf;
        pointer-events: none;
      }
    }

    .tableWrapper {
      margin: 1em 0;
      overflow-x: auto;
    }

    .resize-cursor {
      cursor: ew-resize;
      cursor: col-resize;
    }
  }
}

.menububble {
  position: absolute;
  display: flex;
  z-index: 20;
  background: $color-black;
  border-radius: 5px;
  padding: 0.3rem;
  margin-bottom: 0.5rem;
  transform: translateX(-50%);
  visibility: hidden;
  opacity: 0;
  transition: opacity 0.2s, visibility 0.2s;

  &.is-active {
    opacity: 1;
    visibility: visible;
  }

  &__button {
    display: inline-flex;
    background: transparent;
    border: 0;
    color: $color-white;
    padding: 0.2rem 0.5rem;
    margin-right: 0.2rem;
    border-radius: 3px;
    cursor: pointer;

    &:last-child {
      margin-right: 0;
    }

    &:hover {
      background-color: rgba($color-white, 0.1);
    }

    &.is-active {
      background-color: rgba($color-white, 0.2);
    }
  }

  &__form {
    display: flex;
    align-items: center;
  }

  &__input {
    font: inherit;
    border: none;
    background: transparent;
    color: $color-white;
  }
}
</style>

<style lang="scss">
.v--modal-overlay {
    background: rgba(0, 0, 0, 0) !important;
}

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
