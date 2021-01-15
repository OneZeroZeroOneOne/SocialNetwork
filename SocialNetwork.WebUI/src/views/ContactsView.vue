<template>
  <div class="contacts-view">
    <section>
        <h2>Мої контакти</h2>
        <p>t.me/forevka</p>
        <p>zebestforevka@gmail.com</p>
    </section>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

import Nprogress from "nprogress"

@Component({
  components: { 
  }
})
export default class ContactsView extends Vue {
  constructor() {
    super();
  }

  async created() {
    Nprogress.done()
  }
}
</script>

<style lang="scss" scoped>
h2, p {
  color: white;
}
</style>
