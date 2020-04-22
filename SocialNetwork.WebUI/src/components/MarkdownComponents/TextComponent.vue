<template>
  <div v-html="text"></div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {}
})
export default class TextComponent extends Vue {
  @Prop() public text!: string;

  constructor() {
    super();
  }

  mounted(): void {
    console.log(this.text)
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">

</style>
