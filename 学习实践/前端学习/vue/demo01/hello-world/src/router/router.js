import Vue from "vue";
import Router from "vue-router";
import Index from "../components/Index";
import UCenter from "../components/UCenter";
import UInfo from "../components/UInfo";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      component: Index
    },
    {
      path: "/UCenter",
      component: UCenter,
      children: [
        {
          path: "UInfo",
          component: UInfo
        }
      ]
    }
  ]
});
