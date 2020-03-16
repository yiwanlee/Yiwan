<template>
  <a-layout-sider
    :class="['sider', theme, fixSiderbar ? 'ant-fixed-sidemenu' : null]"
    width="256px"
    :collapsible="collapsible"
    v-model="collapsed"
    :trigger="null"
  >
    <div class="logo" style="color:#fff;text-align:center;font-size:14px;">
      <img alt="Vue logo" src="@/assets/logo.png" />
      <span v-show="!collapsed">企鹅团队协作系统</span>
    </div>
    <a-menu :theme="theme" :mode="mode">
      <a-menu-item key="1">
        <router-link to="/about">
          <a-icon type="user" />
          <span class="nav-text">仪表概览</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="2">
        <router-link to="/demo1/9527">
          <a-icon type="video-camera" />
          <span class="nav-text">产品部</span>
        </router-link>
      </a-menu-item>
      <a-menu-item key="3">
        <a-icon type="upload" />
        <span class="nav-text">设计部</span>
      </a-menu-item>
      <a-menu-item key="4">
        <a-icon type="team" />
        <span class="nav-text">采购部</span>
      </a-menu-item>
    </a-menu>
  </a-layout-sider>
</template>

<script>
export default {
  name: "SideMenu",
  props: {
    mode: {
      type: String,
      required: false,
      default: "inline"
    },
    theme: {
      type: String,
      required: false,
      default: "dark"
    },
    collapsible: {
      type: Boolean,
      required: false,
      default: false
    },
    collapsed: {
      type: Boolean,
      required: false,
      default: false
    },
    menus: {
      type: Array,
      required: true
    }
  }
};
</script>

<style scoped>
.sider .logo img {
  height: 32px;
  padding: 0 12px 0 24px;
}
</style>
