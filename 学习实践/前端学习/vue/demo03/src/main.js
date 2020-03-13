import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

import axios from "axios";

//axios.defaults.baseURL = "https://api.qebb.cn/";
Vue.prototype.$axios = axios;

import "./core/lazy_use"; // ant-design-vue 按需加载

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
