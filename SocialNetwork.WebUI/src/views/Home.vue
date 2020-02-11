<template>
  <div class="home">
    <PostComponent postId="84ef93b0-1a13-429b-82c3-4c72ee0deecb"/>
  </div>
</template>

<script>
// @ is an alias to /src
import PostComponent from '@/components/Post.vue'

export default {
  name: 'home',
  components: {
    PostComponent
  }
}
</script>
