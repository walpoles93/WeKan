<template>
  <v-row>
    <v-col v-for="(activity, j) in activities" :key="j" cols="12">
      <v-card outlined>
        <v-card-title>{{ activity.title }}</v-card-title>
        <v-card-text>{{ activity.description }}</v-card-text>
      </v-card>
    </v-col>
    <v-col cols="12">
      <create-actvity-dialog :card-id="cardId"></create-actvity-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateActvityDialog from '~/components/CreateActivityDialog'

export default {
  components: {
    CreateActvityDialog,
  },
  props: {
    cardId: {
      type: Number,
      required: true,
    },
    activities: {
      type: Array,
      default: () => [],
    },
  },
}
</script>
