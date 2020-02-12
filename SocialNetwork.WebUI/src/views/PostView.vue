<template>
  <div class="home">
    <PostComponent :postId="postId"/>
  </div>
</template>

<script>
// @ is an alias to /src
import PostComponent from '@/components/PostComponent.vue'

export default {
  name: 'PostView',
  components: {
    PostComponent
  },
  computed: {
    // a computed getter
    postId: function () {
      // `this` points to the vm instance
      console.log(this.$route)
      if (this.$route.query.hasOwnProperty('id'))
        return this.$route.query.id
      return 'error'
    }
  }
}
</script>

