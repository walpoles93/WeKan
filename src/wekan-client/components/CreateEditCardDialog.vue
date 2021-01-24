<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attrs }">
      <slot name="activator" :on="on" :attrs="attrs"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline">
          {{ isCreate ? 'Create Card' : 'Edit Card' }}
        </span>
      </v-card-title>
      <v-card-text>
        <v-form v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="card.title"
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
export default {
  props: {
    boardId: {
      type: Number,
      required: true,
    },
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
    card: {
      cardId: 0,
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
        this.card.cardId = newId
      },
      immediate: true,
    },
    title: {
      handler(newTitle) {
        this.card.title = newTitle
      },
      immediate: true,
    },
  },
  methods: {
    async create() {
      this.isSaving = true

      const result = await this.$axios.$post('cards', {
        ...this.card,
        boardId: this.boardId,
      })
      this.$nuxt.$emit('card-created', result)

      this.isSaving = false
      this.dialog = false
    },
    async edit() {
      this.isSaving = true

      const result = await this.$axios.$put('cards', {
        ...this.card,
        boardId: this.boardId,
      })
      this.$nuxt.$emit('card-edited', result)

      this.isSaving = false
      this.dialog = false
    },
  },
}
</script>
