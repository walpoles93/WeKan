<template>
  <v-app>
    <v-app-bar v-if="!isMdAndUp" app flat color="white">
      <v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" app :permanent="isMdAndUp">
      <slot name="sidebar"></slot>
      <template v-slot:append>
        <div class="pa-2">
          <v-btn block depressed @click="logout">
            <v-icon left>mdi-logout</v-icon>
            Logout
          </v-btn>
        </div>
      </template>
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
  methods: {
    async logout() {
      await this.$auth.logout()
      this.$router.push({ name: 'index' })
    },
  },
}
</script>
