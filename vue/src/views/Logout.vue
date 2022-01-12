<template>
  <h1>Logout</h1>
</template>

<!-- Note: This page should not really render, but instead redirect you immediately to Home -->

<script>
export default {
  created() {
    this.$store.commit("LOGOUT");
    this.$router.push({name: 'home'});
  }
};
</script>
