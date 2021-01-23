<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attrs }">
      <v-btn depressed block tile color="primary" v-bind="attrs" v-on="on">
        <v-icon left>mdi-plus</v-icon>
        Create Activity
      </v-btn>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline">Create Activity</span>
      </v-card-title>
      <v-card-text>
        <v-form v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="activity.title"
                  label="Title"
                  required
                  :rules="[(v) => !!v || 'Title must not be empty']"
                ></v-text-field>
                <v-text-field
                  v-model="activity.description"
                  label="Description"
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
    cardId: {
      type: Number,
      required: true,
    },
  },
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    activity: {
      title: '',
      description: '',
    },
  }),
  methods: {
    async onClickSave() {
      this.isSaving = true

      await this.$axios.$post('activities', {
        ...this.activity,
        cardId: this.cardId,
      })
      this.$nuxt.$emit('activity-created')

      this.isSaving = false
      this.dialog = false
      this.activity.title = ''
      this.activity.description = ''
    },
  },
}
</script>
