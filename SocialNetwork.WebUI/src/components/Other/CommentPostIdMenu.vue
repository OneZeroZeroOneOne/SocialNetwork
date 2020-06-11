<template>
    <div class="wrapper">
        <span
            @click="openEditor"
        >#{{comment === undefined ? post.id : comment.id}}</span>
        <ul>
            <li @click="openEditor">Reply</li>
            <li>
                <a
                    :href="'/'+boardName+'/'+post.id+(comment===undefined ? '' : '#'+comment.id)"
                    target="_blank"
                    rel="noopener noreferrer">
                    To Thread
                </a>
                </li>
            <li>Dont Show</li>
        </ul>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { IComment } from '../../models/responses/CommentViewModel';
import { IPost } from '../../models/responses/PostViewModel';
import { IBoard } from '../../models/responses/Board';

@Component({
  components: {
  }
})
export default class CommentPostIdMenuComponent extends Vue {
    @Prop() comment!: IComment;
    @Prop() post!: IPost;
    @Prop() boardName!: string;
    @Prop() isPost!: boolean;

    openEditor(event: MouseEvent): void {
        event.preventDefault();
        if (this.isPost === true)
        {
            this.$root.$emit('show-editor-modal-from-post', this.post)
        }else{
            this.$root.$emit('show-editor-modal-from-comment', this.comment, this.post)
        }
    }

}
</script>

<style lang="scss" scoped>
.wrapper {
  position: relative;
  z-index: 60;
  
  a {
    text-decoration: none;
    color: inherit;
  }

  ul {
    opacity: 0;
    visibility: hidden;
    
    text-align: left;
    
    transition: all 200ms;
  }
  
  &:hover ul {
    opacity: 1;
    visibility: visible;
  }
}

ul {
  position: absolute;
  padding: 4px 0;
  top: 14px;
  right: 0;
  width: max-content;
  
  background-color: white;
  border: 1px solid hsl(0, 0%, 80%);
  border-radius: 4px;
  box-shadow: 0 3px rgba(0, 0, 0, 0.05);
  list-style-type: none;
  
  &:before {
    content: '';
    
    position: absolute;
    top: -12px;
    height: 12px;
    width: 100%;
    
    background-color: transparent;
  }
  
  &:after {
    content: '';
    
    position: absolute;
    right: 6px;
    top: -16px;
    height: 0;
    width: 0;
    
    border: 8px solid transparent;
    border-bottom-color: hsl(0, 0%, 80%);
  }
}

li {
  padding: 5px 10px;
  
  color: hsl(0, 0%, 60%);
  cursor: pointer;
  
  transition: all 100ms;
  
  &:hover {
    background-color: hsl(0, 0%, 90%);
    color: black;
  }
  
  &:first-of-type:after {
    content: '';
    
    position: absolute;
    right: 6px;
    top: -15px;
    height: 0;
    width: 0;
    z-index: 1;
    
    border: 8px solid transparent;
    border-bottom-color: white;
    pointer-events: none;
  }
}
</style>