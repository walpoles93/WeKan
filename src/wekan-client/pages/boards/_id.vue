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
            <h1 class="mb-4">{{ board.title }}</h1>
            <cards-grid :board-id="boardId" :cards="board.cards"></cards-grid>
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
import CardsGrid from '~/components/CardsGrid'

export default {
  components: {
    SidebarLayout,
    AppSidebar,
    CreateBoardDialog,
    CardsGrid,
  },
  data: () => ({
    board: {
      title: '',
      cards: [],
    },
  }),
  computed: {
    boardId() {
      return (this.$route.params && Number(this.$route.params.id)) || 0
    },
  },
  async mounted() {
    if (this.boardId) {
      await this.getBoard(this.boardId)

      this.$nuxt.$on('card-created', () => this.getBoard(this.boardId))
      this.$nuxt.$on('activity-created', () => this.getBoard(this.boardId))
    }
  },
  methods: {
    async getBoard(boardId) {
      const board = await this.$axios.$get(`boards/${boardId}`)

      this.board = board
    },
  },
}
</script>
