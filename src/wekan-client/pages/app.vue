<template>
  <sidebar-layout>
    <template v-slot:sidebar>
      <app-sidebar></app-sidebar>
    </template>
  </sidebar-layout>
</template>

<script>
import SidebarLayout from '~/components/layouts/SidebarLayout'
import AppSidebar from '~/components/AppSidebar'

export default {
  components: {
    SidebarLayout,
    AppSidebar,
  },
}
</script>
