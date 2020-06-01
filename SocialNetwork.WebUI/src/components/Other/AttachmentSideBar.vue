<template>
    <div class="attachment-side-bar-overlay">
        <div class="attachment-side-bar">
            <div class="attachment-side-bar-preview" 
                v-for="att in attachmentList" 
                :key="att.id" 
                v-on:mouseup="showAttachment(att, $event)">
                <img class="image-preview"
                    :src="getAttachmentPreview(att)">
                <div class="play-button" v-if="att.preview !== null">
                    <font-awesome-icon class="icon" icon="play"/>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import GlobalStorage from "../../services/Implementations/GlobalStorage";
import { IAttachment } from '../../models/responses/Attachment';
import EventBus from '../../utilities/EventBus';

@Component({
  components: {
  },
})
export default class AttachmentSideBar extends Vue {
    public attachmentList: IAttachment[] = []

    constructor() {
        super();
    }

    mounted() {
        EventBus.subscribe("new-attachments", this.newAttachments);
        EventBus.subscribe("clear-attachments", this.clearAttachment);
    }

    showAttachment(att: IAttachment, event: MouseEvent) {
        console.log('show att', att, event)
        this.$root.$emit('show-attachment-image', att)
    }

    getAttachmentPreview(att: IAttachment) {
        if (att.preview === null)
            return 'http://194.99.21.140/api/attachment/' + att.path;
        
        return 'http://194.99.21.140/api/attachment/' + att.preview;
    }

    clearAttachment() {
        this.attachmentList = [];
    }

    newAttachments(atts: IAttachment[]) {
        atts.forEach(x => {
            if (this.attachmentList.map(xx => xx.id).indexOf(x.id) === -1)
            {
                this.attachmentList.push(x);
            }
        })
        console.log(this.attachmentList);
    }

}
</script>

<style scoped lang="scss">

.attachment-side-bar-overlay {
    background-color: rgba($color: #232355, $alpha: 0.5);
    max-height: 100%; 
    overflow-y: scroll;
    -ms-overflow-style: none;

    &::-webkit-scrollbar {
        display: none;
    }
}

.attachment-side-bar {
    margin: 20px;

    .attachment-side-bar-preview {
        cursor: pointer;
        position: relative;

        .play-button {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            height: 100%;
            width: 100%;
            opacity: .8;
            transition: opacity .3s ease;
            
            &:hover {
                opacity: 0;
            }

            .icon {
                color: black;
                font-size: 100px;
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                -ms-transform: translate(-50%, -50%);
                text-align: center;
            }
        }

        .image-preview {
            margin-bottom: 5px;
            max-width: 150px;
        }
    }
}

</style>
