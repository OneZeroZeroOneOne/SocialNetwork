<template>
  <div class="gallery-view myGallery">
    <img v-for="i in images" :key="i" :src="i">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

import Nprogress from "nprogress"

@Component({
  components: { 
  }
})
export default class GalleryView extends Vue {
    private images = [
        'https://16ch.ml/api/attachment/Files/132530410222735766.jpg',
        'https://16ch.ml/api/attachment/Files/132530411242186521.jpg',
        'https://16ch.ml/api/attachment/Files/132530412412944963.jpg',
        'https://16ch.ml/api/attachment/Files/132530412416115558.jpg',
    ]

  constructor() {
    super();
  }

  async created() {
    Nprogress.done()
  }
}
</script>


<style lang="scss" scoped>
/*
  The grid itself needs only 4 CSS declarations:
*/

.gallery-view {
    margin: 2rem;
}

.myGallery {
  display: grid;
  grid-gap: 10px;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
}

.myGallery img {
  width: 100%;
}

/*
  And here are some declarations for the image caption.
  Just hover over one of the last 5 images to see it.
*/

.myGallery .item {
  position: relative;
  overflow: hidden;
}

.myGallery .item img {
  vertical-align: middle;
}

.myGallery .caption {
  margin: 0;
  padding: 1em;
  position: absolute;
  z-index: 1;
  bottom: 0;
  left: 0;
  width: 100%;
  max-height: 100%;
  overflow: auto;
  box-sizing: border-box;
  transition: transform 0.5s;
  transform: translateY(100%);
  background: rgba(0, 0, 0, 0.7);
  color: rgb(255, 255, 255);
}

.myGallery .item:hover .caption {
  transform: translateY(0%);
}

/*
  The rest is only styling for this example page
*/

@import url("https://fonts.googleapis.com/css2?family=Vollkorn:wght@400;900&display=swap");

body {
  font: 400 1.5em/1.58 Vollkorn, serif;
}

h1,
p {
  text-align: center;
}

.myGallery {
  font-size: 1rem;
}

</style>
