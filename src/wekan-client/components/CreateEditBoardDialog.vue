<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attr }">
      <slot name="activator" :on="on" :attr="attr"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline">
          {{ isCreate ? 'Create Board' : 'Edit Board' }}
        </span>
      </v-card-title>
      <v-card-text>
        <v-form v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="board.title"
                  label="Title"
                  required
                  :rules="[(v) => !!v || 'Title must not be empty']"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <delete-board-dialog v-if="!isCreate" :id="id">
          <template v-slot:activator="{ on, attrs }">
            <v-btn outlined color="error" v-bind="attrs" v-on="on">
              Delete
            </v-btn>
          </template>
        </delete-board-dialog>
        <v-spacer></v-spacer>
        <v-btn text @click="dialog = false"> Close </v-btn>
        <v-btn
          depressed
          color="primary"
          :loading="isSaving"
          :disabled="!valid"
          @click="() => (isCreate ? create() : edit())"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import DeleteBoardDialog from '~/components/DeleteBoardDialog'

export default {
  components: {
    DeleteBoardDialog,
  },
  props: {
    id: {
      type: Number,
      default: 0,
    },
    title: {
      type: String,
      default: '',
    },
  },
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    board: {
      boardId: 0,
      title: '',
    },
  }),
  computed: {
    isCreate() {
      return !this.id
    },
  },
  watch: {
    id: {
      handler(newId) {
        this.board.boardId = newId
      },
      immediate: true,
    },
    title: {
      handler(newTitle) {
        this.board.title = newTitle
      },
      immediate: true,
    },
  },
  methods: {
    async create() {
      this.isSaving = true

      const result = await this.$axios.$post('boards', this.board)
      this.$nuxt.$emit('board-created', result)

      this.$router.push({ name: 'boards-id', params: { id: result.boardId } })
    },
    async edit() {
      this.isSaving = true

      await this.$axios.$put('boards', this.board)
      this.$nuxt.$emit('board-edited', { boardId: this.board.boardId })

      this.isSaving = false
      this.dialog = false
    },
  },
}
</script>
