<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <template v-slot:activator="{ on, attrs }">
        <v-btn depressed block tile color="primary" v-bind="attrs" v-on="on">
          <v-icon left>mdi-plus</v-icon>
          Create Board
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Create Board</span>
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
          <v-spacer></v-spacer>
          <v-btn text @click="dialog = false"> Close </v-btn>
          <v-btn
            depressed
            color="primary"
            :loading="isSaving"
            :disabled="!valid"
            @click="onClickSave"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    board: {
      title: '',
    },
  }),
  methods: {
    async onClickSave() {
      this.isSaving = true

      await this.$axios.$post('boards', this.board)
      this.$nuxt.$emit('board-created')

      this.isSaving = false
      this.dialog = false
      this.board.title = ''
    },
  },
}
</script>
