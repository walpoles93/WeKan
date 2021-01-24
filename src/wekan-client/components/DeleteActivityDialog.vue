<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attr }">
      <slot name="activator" :on="on" :attr="attr"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline"> Delete Activity </span>
      </v-card-title>
      <v-card-text>Are you sure you want to delete this activity?</v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text @click="dialog = false"> No </v-btn>
        <v-btn
          depressed
          color="error"
          :loading="isDeleting"
          @click="deleteActivity"
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    id: {
      type: Number,
      required: true,
    },
  },
  data: () => ({
    dialog: false,
    isDeleting: false,
  }),
  methods: {
    async deleteActivity() {
      this.isDeleting = true

      const result = await this.$axios.$delete(`activities/${this.id}`)
      this.$nuxt.$emit('activity-deleted', result)

      this.isDeleting = false
      this.dialog = false
    },
  },
}
</script>
