<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <span>全局属性 [ {{ count }} ]</span>
    <span>计算属性Getter [ {{ this.$store.getters.StateCount }} ]</span>
    <button style="width:100px;" @click="jia1">+1</button>
    <button style="width:100px;" @click="jian1">-1</button>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "HelloWorld",
  props: {
    msg: String
  },
  computed: {
    ...mapState({
      count: state => state.count
    })
  },
  methods: {
    jia1() {
      this.$store.dispatch("jiafa", 1);
    },
    jian1() {
      this.$store.dispatch("jianfa", 1);
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="less">
.hello {
  display: flex;
  flex-direction: column;
  align-items: center;
}
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
