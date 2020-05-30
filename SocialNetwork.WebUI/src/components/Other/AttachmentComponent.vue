<template>
    <div class="attachment-content">
    <div class="attachment" v-for="attachment in attachmentObjs" v-bind:key="attachment.id">
        <div class="attachment-name">{{getName(attachment)}}</div>
        <filter-image v-if="attachment.preview === null"
            class="clickable"
            v-on:click.native="imgShow(attachment)" 
            :src="getAttachmentPath(attachment.path)"
            :src-placeholder="getPreloadPath(attachment.preload)"
            :intersectionOptions="intersectionOption" />
        <filter-image v-else
            class="clickable attachment-video gradient-border" 
            v-on:click.native="videoShow(attachment)" 
            :src="getAttachmentPath(attachment.preview)"
            :src-placeholder="getPreloadPath(attachment.preload)"
            :intersectionOptions="intersectionOption" />
    </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IAttachment } from '@/models/responses/Attachment';
import FilterImage from "@/components/Other/FilterImage.vue";


@Component({
  components: {
    FilterImage,
  }
})
export default class AttachmentComponent extends Vue {
  @Prop() public attachmentObjs!: IAttachment[]; 

  public intersectionOption: any = {
    rootMargin: '200px 0px 200px 0px'
  }
  
  constructor() {
    super();
  }

  getName(att: IAttachment): string {
    let ext = "." + att.displayName.split(".").pop();
    if (ext === undefined)
      ext = "";
    return (att.displayName.length > 15) ? att.displayName.substr(0, 15-1) + '[...]' + ext : att.displayName;
  }

  getAttachmentPath(path: string): string {
    return 'http://194.99.21.140/api/attachment/' + path;
  }

  getPreloadPath(path: string): string {
    let s = 'http://194.99.21.140/api/attachment/' + path;
    return s;
  }

  videoShow(attachment: IAttachment): void {
    this.$root.$emit('show-attachment-video', attachment)
  }

  imgShow(attachment: IAttachment): void {
    this.$root.$emit('show-attachment-image', attachment)
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
  border-color: var(--attachment-border-color);
  border-style: dashed;
  border-width: 1px;
  box-sizing: border-box;
}

.attachment {
  margin: 5px;
  grid-template-columns: 100px auto;
  align-items: center;
  display: inline;
  font-size: var(--attachment-text-size);
  text-align: center;
  color: var(--attachment-text-color);
  
  .clickable {
    cursor: pointer;
  }

  .attachment-name {
    margin-bottom: 2px;
  }
}

.attachment:not(:first-child) {
  margin-left: 5px;
}
</style>