<template>
    <div class="attachment-content">
    <div class="attachment" v-for="attachment in attachmentObjs" v-bind:key="attachment.id">
        <div class="attachment-name">{{getName(attachment)}}</div>
        <img v-if="attachment.preview === null"
            class="clickable"
            v-on:click="imgShow(attachment)" 
            v-bind:src="getAttachmentPath(attachment.path)" />
        <img v-else
            class="clickable attachment-video gradient-border" 
            v-on:click="videoShow(attachment)" 
            v-bind:src="getAttachmentPath(attachment.preview)" />
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
      let splitted = att.path.split("/");
      return splitted[splitted.length - 1]
  }

  getAttachmentPath(path: string): string {
    return 'http://16ch.tk/api/attachment/' + path;
  }

  videoShow(attachment: IAttachment): void {
    console.log(this.getAttachmentPath(attachment.path));
    this.$modal.show('preview-video-modal', {
      srcPath: this.getAttachmentPath(attachment.path),
      posterPath: this.getAttachmentPath(attachment.preview),
    }, {
      draggable: true,
      resizable: true,
    })
  }

  imgShow(attachment: IAttachment): void {
    this.$modal.show('preview-modal', {
      srcPath: this.getAttachmentPath(attachment.path)
    }, {
      draggable: true,
      resizable: true,
    })
  }
}
</script>

<style lang="scss" scoped>
.attachment-content {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: flex-start; //flex end for images to bottom
}

.attachment-video {
    border-color: darkgray;
    border-style: dashed;
    border-width: 2px;
    box-sizing: border-box;
}

.attachment {
    margin: 5px 0px;
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
    border-radius: 5px;
    width: 200px;
    height: auto;
    vertical-align: middle;
}

.attachment:not(:first-child) {
    margin-left: 5px;
}
</style>