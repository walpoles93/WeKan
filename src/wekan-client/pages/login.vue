<template>
  <div></div>
</template>

<script>
export default {
  async mounted() {
    if (!this.$auth.loggedIn) {
      await this.$auth.loginWith('auth0')
    } else {
      this.$router.push({ name: 'boards-id' })
    }
  },
}
</script>
