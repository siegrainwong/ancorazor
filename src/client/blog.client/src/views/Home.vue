<template>
  <div class="home">
    <div class="l_body">
      <div class="container clearfix">
        <div class="l_main">
          <section class="post-list">
            <div v-for="i in list" :key="i.id" class="post-wrapper">
              <article class="post">
                <section class="meta">
                  <h2 class="title">
                    <router-link :to="'/content/' + i.id">{{ i.title }}</router-link>
                  </h2>
                  <time>{{i.createTime}}</time>
                  <div class="cats">
                    <a href="javascript:void(0)">{{i.author}}</a>
                  </div>
                </section>
                <section class="article typo">
                  <article v-html="i.content"></article>
                  <div class="readmore">
                    <a href="/dotnet/asp.net core???????????/">查看更多</a>
                  </div>
                  <div class="full-width auto-padding tags">
                    <a href="javascript:void(0);">{{i.category}}</a>
                  </div>
                </section>
              </article>
            </div>
          </section>

          <nav id="page-nav">
            <router-link
              :to="'/?page=' + (pageNumber > 1 ? pageNumber - 1 : 1)"
              class="prev"
              rel="prev"
            >{{ 1 >= pageNumber ? "" : "Previous" }}</router-link>
            <router-link
              :to="'/?page=' + (pageNumber >= total ? total : page + 1)"
              class="next"
              rel="next"
            >{{(pageNumber >= total ? "" : "Next")}}</router-link>
          </nav>
        </div>
        <aside class="l_side">
          <section class="m_widget categories">
            <div class="header">标签</div>
            <div class="content">
              <ul class="entry">
                <li>
                  <a class="flat-box" href="javascript:void(0);">
                    <div class="name">博客</div>
                    <div class="badget">11</div>
                  </a>
                </li>

                <li>
                  <a class="flat-box" href="javascript:void(0);">
                    <div class="name">随笔</div>
                    <div class="badget">10</div>
                  </a>
                </li>
              </ul>
            </div>
          </section>
        </aside>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import HelloWorld from "@/components/HelloWorld.vue"; // @ is an alias to /src
import api from "@/common/network/api.request";

@Component({
  components: {
    HelloWorld
  }
})
export default class Home extends Vue {
  shit: string = "";
  pageNumber: number = 1;
  total: number = 1;
  isShow: boolean = true;
  list: Array<any> = [];

  // lifecycle hook
  mounted() {
    this.getData();
  }

  // methods
  async getData() {
    let pageNumber = Number(this.$route.query.pageNumber);
    if (pageNumber) {
      this.pageNumber = pageNumber;
    }
    let res = await api.get("Blogs", { page: pageNumber });
    this.list = res.data;
  }
}
</script>
