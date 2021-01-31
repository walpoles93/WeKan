<template>
  <draggable
    :list="activities"
    group="activities"
    class="row"
    @change="onDragActivity"
  >
    <v-col v-for="(activity, j) in activities" :key="j" cols="12">
      <v-sheet>
        <v-row>
          <v-col>
            {{ activity.title }}
          </v-col>
          <v-col class="text-right">
            <create-edit-activity-dialog
              :id="activity.id"
              :card-id="cardId"
              :title="activity.title"
            >
              <template v-slot:activator="{ on, attrs }">
                <v-btn icon small v-bind="attrs" v-on="on">
                  <v-icon>mdi-pencil-outline</v-icon>
                </v-btn>
              </template>
            </create-edit-activity-dialog>
          </v-col>
        </v-row>
      </v-sheet>
      <v-divider class="mt-2 mb-1"></v-divider>
    </v-col>

    <template v-slot:footer>
      <v-col cols="12">
        <create-edit-actvity-dialog :card-id="cardId">
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              depressed
              block
              tile
              color="primary"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon left>mdi-plus</v-icon>
              Create Activity
            </v-btn>
          </template>
        </create-edit-actvity-dialog>
      </v-col>
    </template>
  </draggable>
</template>

<script>
import Draggable from 'vuedraggable'
import CreateEditActvityDialog from '~/components/CreateEditActivityDialog'
export default {
  components: {
    Draggable,
    CreateEditActvityDialog,
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
  methods: {
    async onDragActivity(event) {
      if (!event.added && !event.moved) return

      if (event.added) {
        const command = {
          activityId: event.added.element && event.added.element.id,
          cardId: this.cardId,
        }
        await this.$axios.$put('activities/movetocard', command)
      }

      const activityIds = this.activities.map((a) => a.id)
      const result = await this.$axios.$put('activities/reorder', {
        activityIds,
      })

      this.$nuxt.$emit('activity-moved', result)
    },
  },
}
</script>
