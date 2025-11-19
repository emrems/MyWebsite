<template>
  <div class="article-navigation">
    <RouterLink
      v-if="previousArticle"
      :to="`/articles/${previousArticle.slug}`"
      class="nav-link prev-article"
    >
      <i class="icon-arrow-left"></i>
      <div class="nav-content">
        <span>Önceki Yazı</span>
        <p>{{ previousArticle.title }}</p>
      </div>
    </RouterLink>

    <RouterLink to="/articles" class="all-articles-link">
      <i class="icon-grid"></i> Tüm Yazılar
    </RouterLink>

    <RouterLink
      v-if="nextArticle"
      :to="`/articles/${nextArticle.slug}`"
      class="nav-link next-article"
    >
      <div class="nav-content">
        <span>Sonraki Yazı</span>
        <p>{{ nextArticle.title }}</p>
      </div>
      <i class="icon-arrow-right"></i>
    </RouterLink>
  </div>
</template>

<script>
export default {
  name: 'ArticleNavigation',
  
  props: {
    previousArticle: {
      type: Object,
      default: null
    },
    nextArticle: {
      type: Object,
      default: null
    }
  }
};
</script>

<style scoped>
.article-navigation {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 2rem;
  gap: 1rem;
}

.nav-link {
  display: flex;
  align-items: center;
  padding: 1rem;
  border: 1px solid #ecf0f1;
  border-radius: 8px;
  text-decoration: none;
  color: #2c3e50;
  transition: all 0.3s;
  max-width: 300px;
}

.nav-link:hover {
  border-color: #3498db;
  background: #f8f9fa;
  transform: translateY(-3px);
}

.prev-article {
  text-align: left;
}

.next-article {
  text-align: right;
}

.nav-content span {
  font-size: 0.8rem;
  color: #7f8c8d;
  display: block;
  margin-bottom: 0.3rem;
}

.nav-content p {
  font-weight: 500;
  margin: 0;
  line-height: 1.3;
}

.all-articles-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.8rem 1.5rem;
  background: #3498db;
  color: white;
  border-radius: 6px;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s;
}

.all-articles-link:hover {
  background: #2980b9;
  transform: translateY(-3px);
}

/* İkonlar */
[class^="icon-"] {
  display: inline-block;
}

.icon-arrow-left:before {
  content: "←";
}
.icon-arrow-right:before {
  content: "→";
}
.icon-grid:before {
  content: "≡";
}

@media (max-width: 768px) {
  .article-navigation {
    flex-direction: column;
  }

  .nav-link,
  .all-articles-link {
    max-width: 100%;
    width: 100%;
  }
}
</style>