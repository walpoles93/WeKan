<template>
  <v-row>
    <v-col>
      <v-sheet class="pa-4">
        <h2>Boards</h2>
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

      <create-edit-board-dialog>
        <template v-slot:activator="{ on, attr }">
          <v-btn depressed block tile color="primary" v-bind="attr" v-on="on">
            <v-icon left>mdi-plus</v-icon>
            Create Board
          </v-btn>
        </template>
      </create-edit-board-dialog>
      <join-board-dialog>
        <template v-slot:activator="{ on, attr }">
          <v-btn
            depressed
            block
            tile
            color="secondary"
            v-bind="attr"
            class="mt-1"
            v-on="on"
          >
            <v-icon left>mdi-account-group-outline</v-icon>
            Join Board
          </v-btn>
        </template>
      </join-board-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateEditBoardDialog from '~/components/CreateEditBoardDialog'
import JoinBoardDialog from '~/components/JoinBoardDialog'

export default {
  components: {
    CreateEditBoardDialog,
    JoinBoardDialog,
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
