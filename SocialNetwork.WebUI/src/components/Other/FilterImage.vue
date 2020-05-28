<template>
  <div>
    <svg xmlns="http://www.w3.org/2000/svg" version="1.1" class="filter hidden">
      <defs>
        <filter id="blur">
          <feGaussianBlur in="SourceGraphic" :stdDeviation="deviation" />
        </filter>
      </defs>
    </svg>
    <v-lazy-image
      :src="src"
      :src-placeholder="srcPlaceholder"
      :intersectionOptions="intersectionOptions"
      @load="animate"
    ></v-lazy-image>
  </div>
</template>

<script>
export default {
  props: {
    src: String,
    srcPlaceholder: String,
    blurLevel: {
      type: Number,
      default: 5,
    },
    duration: {
      type: Number,
      default: 500,
    },
    intersectionOptions: {
      type: Object,
      default: () => ({}),
    },
  },
  data: () => ({ rate: 1 }),
  computed: {
    deviation() {
      return this.blurLevel * this.rate;
    },
  },
  methods: {
    animate() {
      const start = Date.now() + this.duration;

      const step = () => {
        const remaining = start - Date.now();

        if (remaining < 0) {
          this.rate = 0;
        } else {
          this.rate = remaining / this.duration;
          requestAnimationFrame(step);
        }
      };

      requestAnimationFrame(step);
    },
  },
};
</script>

<style scoped>
.filter {
  display: none;
}

.v-lazy-image {
  width: 200px;
  filter: url("#blur");
  height: auto;
  vertical-align: middle;
}
</style>
