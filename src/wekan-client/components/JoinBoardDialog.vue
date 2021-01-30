<template>
  <v-dialog v-model="dialog" max-width="600px" hide-overlay>
    <template v-slot:activator="{ on, attr }">
      <slot name="activator" :on="on" :attr="attr"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline"> Join Board </span>
      </v-card-title>
      <v-card-text>
        <v-form v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="board.accessCode"
                  label="Access Code"
                  required
                  :rules="[(v) => !!v || 'Access code must not be empty']"
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
          @click="joinBoard"
        >
          Join
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    board: {
      accessCode: '',
    },
  }),
  methods: {
    async joinBoard() {
      this.isSaving = true

      const result = await this.$axios.$put('boards/join', this.board)
      this.$router.push({ name: 'boards-id', params: { id: result.boardId } })

      this.isSaving = false
    },
  },
}
</script>
