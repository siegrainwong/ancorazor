<template>
  <section class="section">
    <div class="columns is-mobile">
      <card title="Free" icon="github-circle">
        Open source on
        <a href="https://github.com/buefy/buefy">GitHub</a>
      </card>

      <card title="Responsive" icon="cellphone-link">
        <b class="has-text-grey">Every</b> component is responsive
      </card>
    </div>
    <div class="columns is-mobile">
      <card title="Modern" icon="alert-decagram">
        Built with
        <a href="https://vuejs.org/">Vue.js</a> and
        <a href="http://bulma.io/">Bulma</a>
      </card>

      <card title="Lightweight" icon="arrange-bring-to-front">
        No other internal dependency
        <button @click="increment">{{ counter }}</button>
        <label>{{fetched}}</label>
      </card>
    </div>
  </section>
</template>

<script lang="ts">
import Card from '../components/Card'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { mapState } from 'vuex'

@Component({ components: { Card } })
export default class Home extends Vue {
  fetched: string = ''

  // fetch({ store }) {
  //   store.commit('counter')
  // }
  mounted() {
    this.$cookies.set('token', 'fuckthiscookie', {
      path: '/',
      maxAge: 60 * 60 * 24 * 7
    })
    console.log('shit')
  }

  // async getData() {
  //   let pageNumber = Number(this.$route.query.pageNumber)
  //   if (pageNumber) {
  //     this.pageNumber = pageNumber
  //   }
  //   let res = await api.get('Blogs', { page: pageNumber })
  //   this.list = res.data
  // }

  get counter() {
    return this.$store.state.counter
  }

  increment() {
    console.log(this.$store)
    this.$store.commit('increment')
  }
}
</script>