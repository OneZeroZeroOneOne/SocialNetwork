<template>
    <div class="attachment-content">
    <div class="attachment" v-for="attachment in attachmentObjs" v-bind:key="attachment.id">
        <div class="attachment-name">{{getName(attachment)}}</div>
        <img v-if="attachment.preview === null"
            class="clickable"
            v-on:click="imgShow(attachment)" 
            v-lazy="getAttachmentPath(attachment.path)" />
        <img v-else
            class="clickable attachment-video gradient-border" 
            v-on:click="videoShow(attachment)" 
            v-lazy="getAttachmentPath(attachment.preview)" />
    </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { ResponseState } from "@/models/enum/ResponseState";
import {IAttachment} from '../models/responses/Attachment';
import _ from 'lodash'

@Component({})
export default class AttachmentComponent extends Vue {
  @Prop() public attachmentObjs!: IAttachment[]; 
  
  constructor() {
    super();
  }

  getName(att: IAttachment): string {
      if (att.displayName === undefined || att.displayName === null)
        return att.path;

      if (att.displayName.length - 20 >= 20)
      {
        let ext = att.displayName.slice(att.displayName.length - 7, att.displayName.length)//splitted[splitted.length - 1]
        return att.displayName.slice(0, att.displayName.length - 20) + "[...]" + ext;
      }

      return att.displayName;
  }

  getAttachmentPath(path: string): string {
    return 'http://16ch.tk/api/attachment/' + path;
  }

  videoShow(attachment: IAttachment): void {
    /*this.$modal.show('preview-video-modal', {
      srcPath: this.getAttachmentPath(attachment.path),
      posterPath: this.getAttachmentPath(attachment.preview),
    }, {
      draggable: true,
      resizable: true,
    })*/
    this.$root.$emit('show-attachment-video', attachment)
  }

  imgShow(attachment: IAttachment): void {
    /*this.$modal.show('preview-modal', {
      srcPath: this.getAttachmentPath(attachment.path)
    }, {
      draggable: true,
      resizable: true,
    })*/
    this.$root.$emit('show-attachment-image', attachment)
  }
}
</script>

<style lang="scss" scoped>
img[lazy=loading] {
  background: url('../assets/Ripple-1s-200px.svg');
}
img[lazy=error] {
  /*your style here*/
}
img[lazy=loaded] {
  /*your style here*/
}
</style>

<style lang="scss" scoped>
.attachment-content {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: flex-start; //flex end for images to bottom
}

.attachment-video {
  border-color: #7d7d7d;
  border-style: dashed;
  border-width: 1px;
  box-sizing: border-box;
}

.attachment {
  margin: 5px;
  grid-template-columns: 200px auto;
  align-items: center;
  display: inline;
  font-size: 0.8em;
  text-align: center;
  color: #d89315;
  
  .clickable {
    cursor: pointer;
  }

  .attachment-name {
    margin-bottom: 2px;
  }

  &:hover {
    color: #906310;
  }
}

.attachment img {
  width: 200px;
  height: auto;
  vertical-align: middle;
}

.attachment:not(:first-child) {
  margin-left: 5px;
}
</style>