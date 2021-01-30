<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app>
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
}
</script>
