import { v4 as uuidv4 } from 'uuid'

export const state = () => ({
  items: [],
})

export const mutations = {
  add(state, snackbar) {
    state.items.push(snackbar)
  },
  remove(state, snackbar) {
    state.items.splice(state.items.indexOf(snackbar), 1)
  },
}

export const actions = {
  addError({ commit }, { message }) {
    const snackbar = {
      id: uuidv4(),
      message,
    }

    commit('add', snackbar)
  },
  removeItem({ commit, state }, { id }) {
    const item = state.items.find((i) => i.id === id)

    commit('remove', item)
  },
}
