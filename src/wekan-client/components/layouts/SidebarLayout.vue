<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app>
      <slot name="sidebar"></slot>
    </v-navigation-drawer>

    <v-main>
      <slot name="main"></slot>
    </v-main>
  </v-app>
</template>

<script>
export default {
  data: () => ({
    drawer: null,
  }),
}
</script>
