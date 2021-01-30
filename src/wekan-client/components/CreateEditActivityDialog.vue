<template>
  <v-dialog v-model="dialog" persistent max-width="600px">
    <template v-slot:activator="{ on, attrs }">
      <slot name="activator" :on="on" :attrs="attrs"></slot>
    </template>

    <v-card>
      <v-card-title>
        <span class="headline">
          {{ isCreate ? 'Create Activity' : 'Edit Activity' }}
        </span>
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
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <delete-activity-dialog :id="id">
          <template v-slot:activator="{ on, attrs }">
            <v-btn outlined color="error" v-bind="attrs" v-on="on">
              Delete
            </v-btn>
          </template>
        </delete-activity-dialog>
        <v-spacer></v-spacer>
        <v-btn text @click="dialog = false"> Close </v-btn>
        <v-btn
          depressed
          color="primary"
          :loading="isSaving"
          :disabled="!valid"
          @click="() => (isCreate ? create() : edit())"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import DeleteActivityDialog from '~/components/DeleteActivityDialog'

export default {
  components: {
    DeleteActivityDialog,
  },
  props: {
    cardId: {
      type: Number,
      required: true,
    },
    id: {
      type: Number,
      default: 0,
    },
    title: {
      type: String,
      default: '',
    },
  },
  data: () => ({
    dialog: false,
    valid: false,
    isSaving: false,
    activity: {
      activityId: 0,
      title: '',
    },
  }),
  computed: {
    isCreate() {
      return !this.id
    },
  },
  watch: {
    id: {
      handler(newId) {
        this.activity.activityId = newId
      },
      immediate: true,
    },
    title: {
      handler(newTitle) {
        this.activity.title = newTitle
      },
      immediate: true,
    },
  },
  methods: {
    async create() {
      this.isSaving = true

      const result = await this.$axios.$post('activities', {
        ...this.activity,
        cardId: this.cardId,
      })
      this.$nuxt.$emit('activity-created', result)

      this.isSaving = false
      this.dialog = false
    },
    async edit() {
      this.isSaving = true

      const result = await this.$axios.$put('activities', {
        ...this.activity,
        cardId: this.cardId,
      })
      this.$nuxt.$emit('activity-edited', result)

      this.isSaving = false
      this.dialog = false
    },
  },
}
</script>
