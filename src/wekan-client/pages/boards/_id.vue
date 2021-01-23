<template>
  <sidebar-layout>
    <template v-slot:sidebar>
      <app-sidebar></app-sidebar>
    </template>

    <template v-slot:main>
      <v-container fluid class="pa-8">
        <v-row v-if="!boardId">
          <v-col cols="12">
            <create-board-dialog></create-board-dialog>
          </v-col>
        </v-row>

        <v-row v-if="boardId">
          <v-col cols="12">
            <h1>{{ board.title }}</h1>
          </v-col>
        </v-row>
      </v-container>
    </template>
  </sidebar-layout>
</template>

<script>
import SidebarLayout from '~/components/layouts/SidebarLayout'
import AppSidebar from '~/components/AppSidebar'
import CreateBoardDialog from '~/components/CreateBoardDialog'

export default {
  components: {
    SidebarLayout,
    AppSidebar,
    CreateBoardDialog,
  },
  data: () => ({
    board: {
      title: '',
      cards: [],
    },
  }),
  computed: {
    boardId() {
      return (this.$route.params && this.$route.params.id) || 0
    },
  },
  async mounted() {
    if (this.boardId) await this.getBoard(this.boardId)
  },
  methods: {
    async getBoard(boardId) {
      const board = await this.$axios.$get(`boards/${boardId}`)

      this.board = board
    },
  },
}
</script>
