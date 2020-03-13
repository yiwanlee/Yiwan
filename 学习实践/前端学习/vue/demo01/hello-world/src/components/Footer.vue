<template>
  <footer class="app-footer">
    <p>{{ copyright }}</p>
  </footer>
</template>

<script>
export default {
  name: "app-footer",
  data() {
    return {
      copyright: "企鹅爸爸 版权所有"
    };
  }
};
</script>

<style scoped>
footer {
  background-color: lightblue;
  padding: 10px;
}
p {
  color: #fff;
  text-align: center;
}
</style>
