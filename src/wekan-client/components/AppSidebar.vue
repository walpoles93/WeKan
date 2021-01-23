<template>
  <v-row>
    <v-col>
      <v-sheet class="pa-4">
        <v-avatar class="mb-4" color="grey darken-1" size="64"></v-avatar>

        <div>username</div>
      </v-sheet>

      <v-divider></v-divider>

      <v-list>
        <v-list-item
          v-for="(board, i) in boards"
          :key="i"
          link
          :to="{ name: 'boards-id', params: { id: board.id } }"
          color="primary"
        >
          <v-list-item-icon>
            <v-icon>mdi-bulletin-board</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>{{ board.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>

      <create-board-dialog></create-board-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateBoardDialog from '~/components/CreateBoardDialog'

export default {
  components: {
    CreateBoardDialog,
  },
  data: () => ({
    boards: [],
  }),
  async mounted() {
    await this.getBoards()
  },
  methods: {
    async getBoards() {
      const { boards } = await this.$axios.$get('boards')

      this.boards = boards
    },
  },
}
</script>
