<template>
  <div>
    <p>这是个人中心</p>
    <router-link to="/UCenter/UInfo">用户信息</router-link>
    <router-view></router-view>
  </div>
</template>
 
<script>
export default {
 
}
</script>
 
<style>
  p{
    background: #d6e9c6;
  }
</style>