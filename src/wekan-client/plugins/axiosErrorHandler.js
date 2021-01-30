export default function ({ $axios, store }) {
  $axios.onError(({ response }) => {
    store.dispatch('snackbarQueue/addError', {
      message: response.data.title,
    })

    return Promise.resolve(false)
  })
}
