<template>
  <div class="info-view">
    <section>
        <h2>Курсова робота Лущик Богдан 4 курс КН-414</h2>
        <p>Цей сайт був написаний для того щоб здати і захистити курсову роботу, з використанням таких технологій як postgresql, aspnet-core, vue-js та інших. Більш детально в звіті</p>
    </section>

    <section>
        <h2>Про сайт</h2>
        <p>Моя перша спроба створити сайт схожий на 2ch.hk<br>Тобто спроба створити анонімну іміджборду для обмінювання новинами, мемами та просто для спілкування.</p>
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
export default class InfoView extends Vue {
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
