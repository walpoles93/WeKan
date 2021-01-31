<template>
  <v-container style="height: 100%" fluid>
    <v-row
      justify="center"
      align="center"
      style="height: 60%"
      class="grey lighten-4"
    >
      <v-col class="text-center" cols="12">
        <v-container>
          <h1 class="text-h1">WeKan</h1>
          <p class="text-subtitle-1">Project planning made easy</p>
          <v-btn x-large depressed color="primary" @click="onClick">
            Sign Up / Log In
          </v-btn>
        </v-container>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-container style="max-width: 800px">
          <h2 class="text-h2">What is WeKan?</h2>
          <p class="text-body">
            WeKan is an online Kanban system for managing your projects. Create
            boards, organise cards and tasks, and share with your colleagues.
            Project planning has never been easier!
          </p>
          <v-btn depressed color="primary" @click="onClick">Try it out</v-btn>
        </v-container>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  layout: 'index',
  methods: {
    onClick() {
      this.$router.push({ name: 'login' })
    },
  },
}
</script>
