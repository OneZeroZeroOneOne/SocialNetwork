<template>
<span class="inline">
  <span 
  :class="getTags()"
  v-if="block_data.text !== undefined && block_data.text !== null && block_data.text !== ''">{{block_data.text}}</span>
  {{' '}}
  <span v-for="block in block_data.child" :key="block.node_id">
    <component :is="getEntityDependOnTag(block.tag)" :key="block.node_id" :all_blocks="all_blocks" :block_data="block" :block_parent="block_data"/>
  </span>
</span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IMarkdownNode } from '../../models/responses/MarkdownNode';

import LinkToComponent from '@/components/MarkdownComponents/LinkToComponent.vue';
import GreenComponent from '@/components/MarkdownComponents/GreenComponent.vue';
import SpoilerComponent from '@/components/MarkdownComponents/SpoilerComponent.vue';

import tagToEntity from '@/utilities/MarkdownUtilities';

@Component({
  components: { 
    LinkToComponent,
    GreenComponent,
    SpoilerComponent,
    TextComponent,
  }
})
export default class TextComponent extends Vue {
  @Prop() public text!: string;
  @Prop() public block_data!: IMarkdownNode;
  @Prop() public block_parent!: IMarkdownNode;
  @Prop() public all_blocks!: IMarkdownNode[];

  constructor() {
    super();
  }

  getSpaces(): string {
    let sp = "";
    let inv_sp = '‎';

    for (let index = 0; index < 10; index++) {
      sp += inv_sp;
    }

    return sp;
  }

  getTags(): string {
    //console.log(this.all_blocks)
    let tag = "";

    let parent: IMarkdownNode|null|undefined = this.block_parent

    while(parent !== null && parent !== undefined)
    {
      if (parent.tag !== undefined)
        if (tag.indexOf(parent.tag) == -1)
          tag += " " + parent.tag;
      // @ts-ignore: Object is possibly 'null' or 'undefined'
      parent = this.all_blocks.find(x => x.node_id == parent.parent_id)
    }

    return tag;
  }

  get_wrapped_text(): string {
    console.log(this.block_data.text)
    let tag = "";

    if (this.block_parent !== undefined)
      tag = this.block_parent.tag;

    if (this.block_data.text != null && this.block_data.text != undefined)
      if (tag !== undefined && tag != "")
        return '<' + tag + '>' + this.block_data.text + " " + '</' + tag + '>';
      else
        return this.block_data.text + " ";
    
    return '';
  }

  getEntityDependOnTag(tag: string): string {
    console.log('in TextComponent_' + tag)
    let ent = tagToEntity[tag]
    if (tag === undefined)
      ent = "TextComponent"
    console.log(ent)
    //return "LinkToComponent";
    return ent;
  }

  mounted(): void {
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.b {
  font-weight: bold;
}
.ins {
  text-decoration: underline;
}
.del {
  text-decoration: line-through;
}
.ins.del {
  text-decoration: line-through underline;
}
  
</style>
