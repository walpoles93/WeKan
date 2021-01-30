<template>
  <v-snackbar
    v-if="item"
    :value="true"
    absolute
    bottom
    right
    outlined
    color="primary"
  >
    {{ item.message }}
  </v-snackbar>
</template>

<script>
export default {
  data() {
    return {
      timeout: 2000,
      item: null,
    }
  },
  computed: {
    queue() {
      return this.$store.state.snackbarQueue.items
    },
  },
  watch: {
    queue: {
      handler() {
        this.processQueue()
      },
      immediate: true,
    },
  },
  methods: {
    processQueue() {
      if (this.queue.length > 0) {
        this.item = this.queue[0]

        setTimeout(() => {
          this.removeItem(this.item.id)
        }, this.timeout)
      } else {
        this.item = null
      }
    },
    removeItem(id) {
      this.$store.dispatch('snackbarQueue/removeItem', { id })

      this.processQueue()
    },
  },
}
</script>
