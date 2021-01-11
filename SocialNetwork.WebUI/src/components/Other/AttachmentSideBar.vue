<template>
    <overlay-scrollbars
        :options="{ className : 'os-theme-block-dark',
        overflowBehavior : {
            x : 'hidden',
            y : 'scroll'
        } }"
        class="attachment-side-bar-overlay">
        <div class="attachment-side-bar">
            <div :class="{'attachment-side-bar-preview': true, current: current.id === att.id}"
                v-for="att in attachmentList" 
                :key="att.id" 
                @click="showAttachment(att, $event)">
                <div class="container">
                    <img class="image-preview"
                        :src="getAttachmentPreview(att)">
                    <div class="play-button" v-if="att.preview !== null">
                        <font-awesome-icon class="icon" icon="play"/>
                    </div>
                </div>
                <hr/>
            </div>
        </div>
    </overlay-scrollbars>
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
    @Prop() current!: IAttachment;
    public attachmentList: IAttachment[] = []

    constructor() {
        super();
    }

    mounted() {
        EventBus.subscribe("new-attachments", this.newAttachments);
        EventBus.subscribe("clear-attachments", this.clearAttachment);

        let scrollers = document.getElementsByClassName('os-scrollbar-handle')
        for (let index = 0; index < scrollers.length; index++) {
            const scroller = scrollers[index];
            if (scroller !== null)
            {
                scroller.addEventListener("mousedown", (event: Event) => {
                    EventBus.emit("dont-close-attachment-modal");
                })
            }
        }
    }

    showAttachment(att: IAttachment, event: MouseEvent) {
        event.preventDefault();
        this.$root.$emit('show-attachment-image', att)
    }

    getAttachmentPreview(att: IAttachment) {
        if (att.preview === null)
            return 'https://16ch.ml/api/attachment/' + att.path;
        
        return 'https://16ch.ml/api/attachment/' + att.preview;
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
    }

}
</script>

<style scoped lang="scss">

.attachment-side-bar-overlay {
    position: relative;
    z-index: 100;
    

    background-color: rgb(48, 29, 17);
    max-height: 100%; 
    overflow-y: scroll;
    -ms-overflow-style: none;

    &::-webkit-scrollbar {
        display: none;
    }

    .attachment-side-bar {
        display: flex;
        flex-direction: column;
    }
}

.attachment-side-bar {
    //margin: 20px;

    .attachment-side-bar-preview {
        position: relative;
        display: flex;
        flex-direction: column;
        position: relative;

        transition-duration: 300ms;
        transition-timing-function: ease-out;
        transition-property: background-color;
        
        &:hover {
            background-color: #ddd;
        }

        .container {
        }

        img {
            margin: 10px 20px;
            border: 1px solid #ddd;
            cursor: pointer;
        }

        &.current {
            background-color: #3f7cac;
            
        }

        hr {
            display: none;
            border: 1px solid red;
            width: 100%;
            margin: 0;
        }

        .play-button {
            cursor: pointer;
            position: absolute;
            top: 50%;
            bottom: 0;
            left: 50%;
            right: 0;
            opacity: .8;
            transition: opacity .3s ease;
            transform: translate(-50%, -50%);
            
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
            max-width: 150px;
        }
    }
}

</style>
