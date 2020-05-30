<template>
  <div class="attachment-container">
    <attachment-modal 
        v-for="att in attachments" 
        :key="att.id"
        :attachment="att"
        @close="closeModal"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IAttachment } from "@/models/responses/Attachment";
import AttachmentModal from "@/components/Modals/AttachmentModal.vue";

@Component({
  components: {
    AttachmentModal
  }
})
export default class ShowAttachmentContainer extends Vue {
    public attachments: IAttachment[] = [];

    constructor() {
        super();
    }

    mounted() {
        this.$root.$on('show-attachment-image', this.showAttachment)
        this.$root.$on('show-attachment-video', this.showAttachment)
    }

    closeModal(attachment: IAttachment) {
        this.attachments = this.attachments.filter(x => x.id !== attachment.id);
    }

    showAttachment(attachment: IAttachment) {
        if (this.attachments.find(x => x.id === attachment.id) === undefined)
        {
            this.attachments.push(attachment)
        }
    }
}
</script>

<style lang="scss" scoped></style>
