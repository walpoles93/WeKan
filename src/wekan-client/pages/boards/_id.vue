<template>
  <sidebar-layout>
    <template v-slot:sidebar>
      <app-sidebar></app-sidebar>
    </template>

    <template v-slot:main>
      <v-container fluid class="pa-8">
        <v-row v-if="!boardId">
          <v-col cols="12">
            <create-edit-board-dialog>
              <template v-slot:activator="{ on, attr }">
                <v-btn
                  depressed
                  block
                  tile
                  color="primary"
                  v-bind="attr"
                  v-on="on"
                >
                  <v-icon left>mdi-plus</v-icon>
                  Create Board
                </v-btn>
              </template>
            </create-edit-board-dialog>
          </v-col>
        </v-row>

        <v-row v-if="boardId">
          <v-col cols="12">
            <v-row align="center" class="px-3">
              <h1>{{ board.title }}</h1>
              <v-spacer></v-spacer>
              <create-edit-board-dialog
                v-if="isOwner"
                :id="boardId"
                :title="board.title"
              >
                <template v-slot:activator="{ on, attr }">
                  <v-btn icon v-bind="attr" small v-on="on">
                    <v-icon>mdi-pencil-outline</v-icon>
                  </v-btn>
                </template>
              </create-edit-board-dialog>
              <share-board-dialog :access-code="board.accessCode">
                <template v-slot:activator="{ on, attr }">
                  <v-btn icon v-bind="attr" v-on="on">
                    <v-icon>mdi-share-variant-outline</v-icon>
                  </v-btn>
                </template>
              </share-board-dialog>
            </v-row>
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
import CreateEditBoardDialog from '~/components/CreateEditBoardDialog'
import ShareBoardDialog from '~/components/ShareBoardDialog'
import CardsGrid from '~/components/CardsGrid'

export default {
  components: {
    SidebarLayout,
    AppSidebar,
    CreateEditBoardDialog,
    ShareBoardDialog,
    CardsGrid,
  },
  data: () => ({
    board: {
      title: '',
      accessCode: '',
      boardUserType: '',
      cards: [],
    },
  }),
  computed: {
    boardId() {
      return (this.$route.params && Number(this.$route.params.id)) || 0
    },
    isOwner() {
      return this.board.boardUserType === 'owner'
    },
  },
  async mounted() {
    if (this.boardId) {
      await this.getBoard(this.boardId)

      this.$nuxt.$on('card-created', () => this.getBoard(this.boardId))
      this.$nuxt.$on('card-edited', () => this.getBoard(this.boardId))
      this.$nuxt.$on('card-deleted', () => this.getBoard(this.boardId))

      this.$nuxt.$on('activity-created', () => this.getBoard(this.boardId))
      this.$nuxt.$on('activity-edited', () => this.getBoard(this.boardId))
      this.$nuxt.$on('activity-deleted', () => this.getBoard(this.boardId))

      this.$nuxt.$on('board-edited', () => this.getBoard(this.boardId))
      this.$nuxt.$on('board-deleted', () =>
        this.$router.push({ name: 'boards-id' })
      )
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
