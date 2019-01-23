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

            <card title="Lightweight" icon="arrange-bring-to-front">No other internal dependency</card>
        </div>
    </section>
</template>

<script type="ts">
import Card from '~/components/Card'
import { Component } from 'vue-class-component'

@Component({ components: { Card } })
export default class Home extends Vue {
    mounted() {
        this.$axios.get('http://www.baidu.com')
        console.log('shit')
    }

    // methods
    async getData() {
        let pageNumber = Number(this.$route.query.pageNumber)
        if (pageNumber) {
            this.pageNumber = pageNumber
        }
        let res = await api.get('Blogs', { page: pageNumber })
        this.list = res.data
    }
}
</script>
