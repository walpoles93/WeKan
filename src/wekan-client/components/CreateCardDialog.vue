<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attrs }">
      <v-hover v-slot="{ hover }">
        <v-card
          height="15rem"
          :color="hover ? 'grey lighten-4' : undefined"
          style="cursor: pointer"
          v-bind="attrs"
          v-on="on"
        >
          <v-row align="center" justify="center" style="height: 100%">
            <v-icon left>mdi-plus</v-icon>
            Create Card
          </v-row>
        </v-card>
      </v-hover>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline">Create Card</span>
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
          @click="onClickSave"
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
  },
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    card: {
      title: '',
    },
  }),
  methods: {
    async onClickSave() {
      this.isSaving = true

      await this.$axios.$post('cards', { ...this.card, boardId: this.boardId })
      this.$nuxt.$emit('card-created')

      this.isSaving = false
      this.dialog = false
      this.card.title = ''
    },
  },
}
</script>
