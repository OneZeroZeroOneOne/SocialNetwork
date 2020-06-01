<template>
  <div class="attachment-container">
    <attachment-modal 
        v-for="att in attachments" 
        :key="att.id"
        :attachment="att"
        @close="closeModal"/>
    <attachment-side-bar :class="{'active': sideBarActive, 'sidebar': true}"/>
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
      this.hideSideBar()
      this.attachments = this.attachments.filter(x => x.id !== attachment.id);
    }

    showAttachment(attachment: IAttachment) {
      this.showSideBar()
      if (this.attachments.find(x => x.id === attachment.id) === undefined)
      {
          this.attachments.push(attachment)
      }
    }
}
</script>

<style lang="scss" scoped>
.sidebar {
  position: fixed;
  transition: .5s ease;
  top: 0px;
  right: -190px;

  &.active{
    position: fixed;
    top: 0px;
    right: 0px;
  }
}


</style>
