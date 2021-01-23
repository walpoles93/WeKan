<template>
  <v-row>
    <v-col v-for="(card, i) in cards" :key="i" cols="12" sm="6" md="4" lg="3">
      <v-card outlined>
        <v-card-title>
          <h2>{{ card.title }}</h2>
        </v-card-title>

        <v-card-text>
          <activities-list
            :card-id="card.id"
            :activities="card.activities"
          ></activities-list>
        </v-card-text>
      </v-card>
    </v-col>

    <v-col cols="12" sm="6" md="4" lg="3">
      <create-card-dialog :board-id="boardId"></create-card-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateCardDialog from '~/components/CreateCardDialog'
import ActivitiesList from '~/components/ActivitiesList'

export default {
  components: {
    CreateCardDialog,
    ActivitiesList,
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
