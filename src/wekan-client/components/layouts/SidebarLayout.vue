<template>
  <v-app>
    <v-app-bar v-if="!isMdAndUp" app flat color="white">
      <v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" app :permanent="isMdAndUp">
      <slot name="sidebar"></slot>
    </v-navigation-drawer>

    <v-main>
      <slot name="main"></slot>
    </v-main>

    <snackbar-queue></snackbar-queue>
  </v-app>
</template>

<script>
import SnackbarQueue from '~/components/SnackbarQueue'

export default {
  components: {
    SnackbarQueue,
  },
  data: () => ({
    drawer: null,
  }),
  computed: {
    isMdAndUp() {
      return this.$vuetify.breakpoint.mdAndUp
    },
  },
}
</script>
