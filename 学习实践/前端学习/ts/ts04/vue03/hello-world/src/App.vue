<template>
  <div id="app">
    <app-header />
    <router-link to="/UCenter">个人中心</router-link>
    <router-view></router-view>
    <Users />
    <app-footer />
  </div>
</template>

<script>
//import HelloWorld from "./components/HelloWorld.vue";
//import router from "vue-router";
import Users from "./components/Users";

export default {
  name: "App",
  data() {
    return {
      title: "这是我的第一个vue脚手架项目"
    };
  },
  components: { Users }
};
</script>

<style>
h1 {
  color: red;
}
</style>
