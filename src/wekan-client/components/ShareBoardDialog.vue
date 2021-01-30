<template>
  <v-dialog v-model="dialog" max-width="600px">
    <template v-slot:activator="{ on, attr }">
      <slot name="activator" :on="on" :attr="attr"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline"> Share Board </span>
      </v-card-title>
      <v-card-text>
        Share this access code with your collaborators:
        {{ accessCode }}
      </v-card-text>

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text @click="dialog = false"> Close </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    accessCode: {
      type: String,
      required: true,
    },
  },
  data: () => ({
    dialog: false,
  }),
}
</script>
