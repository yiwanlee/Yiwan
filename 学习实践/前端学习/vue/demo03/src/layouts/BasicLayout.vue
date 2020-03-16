<template>
  <a-layout id="basic_layout">
    <SideMenu
      theme="dark"
      mode="inline"
      :collapsed="collapsed"
      :collapsible="true"
      @menuSelect="menuSelect"
    />
    <a-layout>
      <a-layout-header style="background: #fff; padding: 0">
        <a-icon
          class="trigger"
          :type="collapsed ? 'menu-unfold' : 'menu-fold'"
          @click="() => (collapsed = !collapsed)"
        />
        <div style="float:right;">超级管理员</div>
      </a-layout-header>
      <a-layout-content
        :style="{
          margin: '24px 16px',
          padding: '24px',
          background: '#fff',
          minHeight: '280px'
        }"
      >
        <router-view />
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script>
import SideMenu from "../components/SideMenu";
export default {
  data() {
    return {
      collapsed: false
    };
  },
  components: { SideMenu },
  mounted() {
    this.$axios
      .get("index.html", {
        params: {}
      })
      .then(function(response) {
        console.log(response);
      })
      .catch(function(error) {
        console.log(error);
      });
  }
};
</script>

<style>
#app,
#basic_layout {
  height: 100%;
}
#basic_layout .trigger {
  font-size: 18px;
  line-height: 64px;
  padding: 0 24px;
  cursor: pointer;
  transition: color 0.3s;
}

#basic_layout .trigger:hover {
  color: #1890ff;
}

#basic_layout .logo {
  height: 64px;
  background: #002140;
  position: relative;
  height: 64px;
  overflow: hidden;
  line-height: 64px;
  background: #002140;
  -webkit-transition: all 0.3s;
  transition: all 0.3s;
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  font-size: 20px !important;
  color: #fff;
  font-weight: 600;
}
</style>
