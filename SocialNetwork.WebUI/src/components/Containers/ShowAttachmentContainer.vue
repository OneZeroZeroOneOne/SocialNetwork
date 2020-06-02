<template>
  <div class="attachment-container">
    <attachment-modal 
        v-for="att in attachments" 
        :key="att.id"
        :attachment="att"
        @close="closeModal"/>
    <attachment-side-bar 
      :class="{'active': sideBarActive, 'sidebar': true}"
      :current="currentAttachment"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IAttachment } from "@/models/responses/Attachment";
import AttachmentModal from "@/components/Modals/AttachmentModal.vue";
import AttachmentSideBar from '@/components/Other/AttachmentSideBar.vue';
import EventBus from '@/utilities/EventBus';

@Component({
  components: {
    AttachmentModal,
    AttachmentSideBar,
  }
})
export default class ShowAttachmentContainer extends Vue {
    public sideBarActive: boolean = false;
    public currentAttachment: IAttachment|any = {
      id: 0,
    };

    public attachments: IAttachment[] = [];

    constructor() {
        super();
    }

    mounted() {
        EventBus.subscribe("show-side-bar", this.showSideBar);
        EventBus.subscribe("hide-side-bar", this.hideSideBar);
        
        this.$root.$on('show-attachment-image', this.showAttachment)
        this.$root.$on('show-attachment-video', this.showAttachment)
    }

    showSideBar() {
        this.sideBarActive = true;
    }

    hideSideBar() {
        this.sideBarActive = false;
    }

    closeModal(attachment: IAttachment) {
      console.log('close modal')
      this.hideSideBar()
      this.attachments = this.attachments.filter(x => x.id !== attachment.id);
    }

    showAttachment(attachment: IAttachment) {
      console.log('open modal')
      this.showSideBar()
      if (this.attachments.find(x => x.id === attachment.id) === undefined)
      {
        this.currentAttachment = attachment;
        this.attachments.push(attachment)
      }/*else{
        this.closeModal(attachment);
      }*/
    }
}
</script>

<style lang="scss" scoped>
.sidebar {
  position: fixed;
  transition: .5s ease;
  top: 0px;
  right: -190px;
  opacity: .7;
  width: 10%;

  &.active {
    position: fixed;
    top: 0px;
    right: 0px;
  }

  &:hover {
    opacity: 1;
  }

  &:before {
    content: "";
    position: fixed;
    z-index: 99999;
    top: 0;
    right: inherit;
    width: inherit;
    height: 45px;
    //box-shadow: inset 0px 35px 35px -20px red;
    background-image: linear-gradient(to bottom, black, transparent);
  }

  &:after {
    content: "";
    position: fixed;
    z-index: 99999;
    bottom: 0;
    right: inherit;
    width: inherit;
    height: 45px;
    //box-shadow: inset 0px 35px 35px -20px red;
    background-image: linear-gradient(to top, black, transparent);
  }
}


</style>
