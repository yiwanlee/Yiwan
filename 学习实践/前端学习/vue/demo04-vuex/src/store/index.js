import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    count: 1
  },
  getters: {
    StateCount(state) {
      return state.count + 1;
    }
  },
  mutations: {
    jia(state, num) {
      state.count = state.count + num;
    },
    jian(state, num) {
      state.count = state.count - num;
    }
  },
  actions: {
    jiafa(context, num) {
      context.commit("jia", num)
    },
    jianfa(context, num) {
      context.commit("jian", num)
    }
  },
  modules: {}
});
