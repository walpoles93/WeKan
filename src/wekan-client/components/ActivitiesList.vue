<template>
  <v-row>
    <v-col v-for="(activity, j) in activities" :key="j" cols="12">
      <v-card outlined>
        <v-card-text>
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
              <delete-activity-dialog :id="activity.id">
                <template v-slot:activator="{ on, attrs }">
                  <v-btn icon color="error" small v-bind="attrs" v-on="on">
                    <v-icon>mdi-delete-outline</v-icon>
                  </v-btn>
                </template>
              </delete-activity-dialog>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-col>
    <v-col cols="12">
      <create-edit-actvity-dialog :card-id="cardId">
        <template v-slot:activator="{ on, attrs }">
          <v-btn depressed block tile color="primary" v-bind="attrs" v-on="on">
            <v-icon left>mdi-plus</v-icon>
            Create Activity
          </v-btn>
        </template>
      </create-edit-actvity-dialog>
    </v-col>
  </v-row>
</template>

<script>
import CreateEditActvityDialog from '~/components/CreateEditActivityDialog'
import DeleteActivityDialog from '~/components/DeleteActivityDialog'

export default {
  components: {
    CreateEditActvityDialog,
    DeleteActivityDialog,
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
