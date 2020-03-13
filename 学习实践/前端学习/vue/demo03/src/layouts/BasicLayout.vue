<template>
  <a-layout id="basic_layout">
    <a-layout-sider :trigger="null" collapsible v-model="collapsed">
      <div class="logo" style="color:#fff;text-align:center;font-size:14px;">
        企鹅团队协作系统
      </div>
      <a-menu
        theme="dark"
        mode="inline"
        :defaultSelectedKeys="['1']"
        :inlineCollapsed="collapsed"
      >
        <template v-for="item in list">
          <a-menu-item v-if="!item.children" :key="item.key">
            <a-icon type="pie-chart" />
            <span>{{ item.title }}</span>
          </a-menu-item>
          <a-sub-menu v-else :key="item.key">
            <span slot="title"><a-icon type="user" />{{ item.title }}</span>
            <template v-for="item2 in item.children">
              <a-menu-item :key="item2.key">
                <a-icon type="pie-chart" />
                <span>{{ item2.title }}</span>
              </a-menu-item>
            </template>
          </a-sub-menu>
        </template>
      </a-menu>
    </a-layout-sider>
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
const data = [
  {
    key: "1",
    title: "首页"
  },
  {
    key: "11",
    title: "产品部"
  }
];

export default {
  data() {
    return {
      collapsed: false,
      list: data
    };
  },
  components: {
    //Layout
  },
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
  height: 32px;
  background: rgba(255, 255, 255, 0.2);
  margin: 16px;
}
</style>
