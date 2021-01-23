<template>
  <v-row>
    <v-col v-for="(card, i) in cards" :key="i" cols="12" sm="6" md="4" lg="3">
      <v-card>
        {{ card.title }}
      </v-card>
    </v-col>

    <v-col cols="12" sm="6" md="4" lg="3">
      <create-card-dialog :board-id="boardId"></create-card-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateCardDialog from '~/components/CreateCardDialog'

export default {
  components: {
    CreateCardDialog,
  },
  props: {
    boardId: {
      type: Number,
      required: true,
    },
    cards: {
      type: Array,
      default: () => [],
    },
  },
}
</script>
