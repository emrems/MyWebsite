import { createRouter, createWebHistory } from "vue-router";
import Home from "@/views/Home.vue";
import About from "@/views/About.vue";
import Contact from "@/views/Contact.vue";
import Project from "@/views/Project.vue";
import Article from "@/views/Article.vue";
import ArticleDetail from "@/components/Article/ArticleDetail.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
    },
    {
      path: "/about",
      name: "about",
      component: About,
    },
    {
      path: "/contact",
      name: "contact",
      component: Contact,
    },
    {
      path: "/project",
      name: "project",
      component: Project,
    },
    {
      path: "/articles",
      name: "article",
      component: Article,
    },
    {
      path: "/articles/:slug",
      name: "Article-Detail",
      component: ArticleDetail,
    }
  ],
});

export default router;
