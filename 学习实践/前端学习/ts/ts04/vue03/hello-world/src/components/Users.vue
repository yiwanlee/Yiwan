<template>
  <div class="users">
    <h1>我的Users组件</h1>
    <!--遍历数组-->
    <ul>
      <!-- 添加点击事件 -->
      <li
        v-for="(user, index) in users"
        v-bind:key="index"
        @click="user.show = !user.show"
      >
        <h2>{{ user.name }}</h2>
        <h3 v-show="user.show">{{ user.position }}</h3>
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: "Users",
  data() {
    return {
      users: [
        { name: "李雷111", position: "web开发", show: false },
        { name: "李雷222", position: "web开发", show: false },
        { name: "李雷333", position: "web开发", show: false },
        { name: "李雷444", position: "web开发", show: false },
        { name: "李雷555", position: "web开发", show: false },
        { name: "李雷666", position: "web开发", show: false }
      ]
    };
  }
};
</script>

<style scoped>
h1 {
  color: orange;
}

.users {
  width: 100%;
  max-width: 1000px;
  margin: 20px auto;
  padding: 0 20px;
  box-sizing: border-box;
}

.users ul {
  display: flex;
  flex-wrap: wrap;
  list-style-type: none;
  padding: 0px;
}

.users ul li {
  flex-grow: 1;
  flex-basis: 200px;
  text-align: center;
  padding: 30px;
  border: 1px solid gray;
  margin: 10px;
}
</style>
