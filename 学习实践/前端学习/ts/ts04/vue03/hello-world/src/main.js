import Vue from "vue";
import router from './router/router.js'
import App from "./App.vue";
import Header from "./components/Header";
import Footer from "./components/Footer";

Vue.config.productionTip = true;

//Vue.use(Header);
Vue.component(Header.name, Header);
Vue.component(Footer.name, Footer);

new Vue({
  render: h => h(App)
}).$mount("#app");

//错误的
{
  /* <template>
  <App />
</template>;
new Vue({
  el: "#app",
  components: { App }
  //template: "<App />"
}); */
}
