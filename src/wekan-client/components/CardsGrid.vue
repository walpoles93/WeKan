<template>
  <draggable :list="cards" group="cards" class="row" @change="onDragCard">
    <v-col v-for="(card, i) in cards" :key="i" cols="12" sm="6" md="4">
      <v-card outlined>
        <v-card-actions>
          <v-spacer></v-spacer>
          <create-edit-card-dialog
            :id="card.id"
            :board-id="boardId"
            :title="card.title"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn v-bind="attrs" icon small v-on="on">
                <v-icon>mdi-pencil-outline</v-icon>
              </v-btn>
            </template>
          </create-edit-card-dialog>
          <delete-card-dialog :id="card.id">
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon color="error" small v-bind="attrs" v-on="on">
                <v-icon>mdi-delete-outline</v-icon>
              </v-btn>
            </template>
          </delete-card-dialog>
        </v-card-actions>

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

    <template v-slot:footer>
      <v-col cols="12" sm="6" md="4">
        <create-edit-card-dialog :board-id="boardId">
          <template v-slot:activator="{ on, attrs }">
            <v-hover v-slot="{ hover }">
              <v-card
                outlined
                height="15rem"
                :color="hover ? 'grey lighten-4' : undefined"
                style="cursor: pointer"
                v-bind="attrs"
                v-on="on"
              >
                <v-card-text style="height: 100%">
                  <v-row align="center" justify="center" style="height: 100%">
                    <v-icon left>mdi-plus</v-icon>
                    Create Card
                  </v-row>
                </v-card-text>
              </v-card>
            </v-hover>
          </template>
        </create-edit-card-dialog>
      </v-col>
    </template>
  </draggable>
</template>

<script>
import Draggable from 'vuedraggable'
import CreateEditCardDialog from '~/components/CreateEditCardDialog'
import DeleteCardDialog from '~/components/DeleteCardDialog'
import ActivitiesList from '~/components/ActivitiesList'

export default {
  components: {
    Draggable,
    CreateEditCardDialog,
    DeleteCardDialog,
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
  methods: {
    async onDragCard(event) {
      if (event.moved) {
        const cardIds = this.cards.map((c) => c.id)
        const result = await this.$axios.$put(`cards/reorder`, { cardIds })

        this.$nuxt.$emit('card-moved', result)
      }
    },
  },
}
</script>
