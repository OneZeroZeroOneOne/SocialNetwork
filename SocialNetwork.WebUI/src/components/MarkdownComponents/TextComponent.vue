<script lang="ts">
/*


*/
import { Component, Prop, Vue } from "vue-property-decorator";
import { IMarkdownNode } from '../../models/responses/MarkdownNode';

import LinkToComponent from '@/components/MarkdownComponents/LinkToComponent.vue';
import GreenComponent from '@/components/MarkdownComponents/GreenComponent.vue';
import SpoilerComponent from '@/components/MarkdownComponents/SpoilerComponent.vue';

//import TextComponentHtml from '@/components/MarkdownComponents/TextComponentHtml.vue';

import tagToEntity from '@/utilities/MarkdownUtilities';

//Vue.component("TextComponentHtml", TextComponentHtml)

/*Vue.component('TextComponentHtml', {
  render: function (createElement) {
    return createElement(
      'span',   // имя тега
      this.text // массив дочерних элементов
    )
  },
  props: {
    text: {
      type: String,
      required: true
    }
  }
})*/

@Component({
  components: { 
    LinkToComponent,
    GreenComponent,
    SpoilerComponent,
    TextComponent
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

  buildElementBlocks(el: IMarkdownNode, createElement, rendered_blocks: number[]): any {
      console.log(el)
      let results: any = []

      if (el === undefined) return;

      if (el.child !== undefined && el.child !== null) {
        el.child.forEach(element => {
          results.push(this.buildElementBlocks(element, createElement, rendered_blocks))
        });
      }
      
      console.log(JSON.stringify(rendered_blocks))
      if (rendered_blocks.indexOf(el.node_id) === -1)
      {
        if (el.text === undefined)
          el.text = "";

        console.log(results)
        //{ props: {text: el.text, block_data: this.block_data, block_parent: this.block_parent, all_blocks: this.all_blocks} }
        let tag = el.tag;
        if (tag === undefined || tag === null)
        {
          let parent: IMarkdownNode|null|undefined = this.all_blocks.find(x => x.node_id == el.parent_id)//element.

          if (parent !== undefined && parent !== null)
            tag = parent.tag;
        }

        rendered_blocks.push(el.node_id)
        let entity = this.getEntityDependOnTag(tag);
        if (entity !== 'br')
          results.push(createElement(entity, {class: this.getTags(el), props: {block_data: el}}, el.text))
        else
          results.push(createElement(entity))
      }
      return results;
    }

  render(createElement) {
    console.log('render', this.block_data.tag)
    let els: IMarkdownNode[] = []
    let rendered_blocks: number[] = []
    let result = null
    let root = this.buildElementBlocks(this.block_data, createElement, rendered_blocks)
    return createElement('span', {}, [root])
  }

  getSpaces(): string {
    let sp = "";
    let inv_sp = '‎';

    for (let index = 0; index < 10; index++) {
      sp += inv_sp;
    }

    return sp;
  }

  getTags(element: IMarkdownNode): string {
    //console.log(this.all_blocks)
    let tag = "";

    let parent: IMarkdownNode|null|undefined = this.all_blocks.find(x => x.node_id == element.parent_id)//element.

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
    //console.log(this.block_data.text)
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
    //console.log('in TextComponent_' + tag)

    let newTagToEntity = {
      b: "span",
      p: "span",
      ins: "span",
      del: "span",
      br: 'br',
      linktocomponent: "LinkToComponent",
      null: "span",
      undefined: "span"
    }

    let ent = newTagToEntity[tag]
    if (tag === undefined)
      ent = "span"
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
span {
  white-space: pre-wrap;
}

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
