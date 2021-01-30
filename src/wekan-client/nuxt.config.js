import path from 'path'
import fs from 'fs'

export default {
  // Global page headers (https://go.nuxtjs.dev/config-head)
  head: {
    titleTemplate: '%s - wekan-client',
    title: 'wekan-client',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }],
  },

  // Global CSS (https://go.nuxtjs.dev/config-css)
  css: [],

  // Plugins to run before rendering page (https://go.nuxtjs.dev/config-plugins)
  plugins: ['~/plugins/axiosErrorHandler.js'],

  // Auto import components (https://go.nuxtjs.dev/config-components)
  components: true,

  // Modules for dev and build (recommended) (https://go.nuxtjs.dev/config-modules)
  buildModules: [
    // https://go.nuxtjs.dev/eslint
    '@nuxtjs/eslint-module',
    // https://go.nuxtjs.dev/vuetify
    '@nuxtjs/vuetify',
    '@nuxtjs/dotenv',
  ],

  // Modules (https://go.nuxtjs.dev/config-modules)
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios',
    '@nuxtjs/auth',
  ],

  // Vuetify module configuration (https://go.nuxtjs.dev/config-vuetify)
  vuetify: {
    customVariables: ['~/assets/variables.scss'],
    theme: {},
  },

  // Build Configuration (https://go.nuxtjs.dev/config-build)
  build: {},

  axios: {
    baseURL: 'https://localhost:44360',
  },

  router: {
    middleware: ['auth'],
  },

  auth: {
    strategies: {
      auth0: {
        domain: process.env.AUTHENTICATION_DOMAIN,
        client_id: process.env.AUTHENTICATION_CLIENT_ID,
        audience: process.env.AUTHENTICATION_AUDIENCE,
      },
    },
  },

  server:
    process.env.NODE_ENV !== 'production'
      ? {
          https: {
            key: fs.readFileSync(path.resolve(__dirname, 'server.key')),
            cert: fs.readFileSync(path.resolve(__dirname, 'server.crt')),
          },
        }
      : {},
}
