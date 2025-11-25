<template>
  <div class="articles-container">
    
    <div class="page-header">
      <h1>Blog Yazıları</h1>
      <p class="page-subtitle">
        Teknoloji, tasarım ve yazılım dünyasından en güncel içerikler
      </p>
    </div>

    <!-- Loading -->
    <LoadingSpinner
      v-if="isLoading"
      text="Yazılar yükleniyor..."
    />

    <!-- Error -->
    <ErrorMessage
      v-else-if="getError"
      :message="getError"
      :showRetry="true"
      @retry="fetchArticles"
    />

    <!-- Articles -->
    <div
      v-else-if="allArticles && allArticles.length"
      class="articles-grid"
    >
      <div
        v-for="article in allArticles"
        :key="article.id"
        class="article-card"
      >
        <div
          class="article-image"
          :style="{ backgroundImage: `url(https://picsum.photos/400/250?random=${article.id})` }"
        >
          <div class="article-category">
            {{ article.category || "Teknoloji" }}
          </div>
        </div>

        <div class="article-card-content">
          <h2>{{ article.title }}</h2>

          <div class="article-meta">
            <span>
              📅 {{ formatDate(article.createdDate) }}
            </span>
            <span>
              🕒 {{ calculateReadingTime(article.content) }} dk okuma
            </span>
          </div>

          <p class="article-snippet">
            {{ truncateContent(article.content, 140) }}
          </p>

          <RouterLink
            :to="`/articles/${article.slug}`"
            class="read-more"
          >
            Devamını Oku →
          </RouterLink>
        </div>
      </div>
    </div>

    <!-- Empty -->
    <div v-else class="empty-state">
      📝 Henüz makale bulunmamaktadır.
    </div>

  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";

export default {
  components: { ErrorMessage, LoadingSpinner },

  computed: {
    ...mapGetters("articles", ["allArticles", "isLoading", "getError"]),
  },

  methods: {
    ...mapActions("articles", ["fetchArticles"]),

    formatDate(dateString) {
      return new Date(dateString).toLocaleDateString("tr-TR", {
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    },

    truncateContent(content, length) {
      return content.length > length
        ? content.substring(0, length) + "..."
        : content;
    },

    calculateReadingTime(content) {
      const wpm = 200;
      const words = content.split(" ").length;
      return Math.ceil(words / wpm);
    },
  },

  created() {
    this.fetchArticles();
  },
};
</script>

<style scoped>
.articles-container {
  max-width: 1200px;
  margin: auto;
  padding: 70px 1rem 0;
  font-family: Inter, sans-serif;
}

.page-header {
  text-align: center;
  margin-bottom: 2.5rem;
}

.page-header h1 {
  font-size: 2.4rem;
  font-weight: 700;
  color: #2c3e50;
}

.page-subtitle {
  color: #7f8c8d;
}

/* Grid */
.articles-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 2rem;
}

/* Card */
.article-card {
  background: #fff;
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 8px 18px rgba(0, 0, 0, 0.07);
  transition: 0.25s;
}

.article-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 14px 26px rgba(0, 0, 0, 0.1);
}

.article-image {
  height: 220px;
  background-size: cover;
  background-position: center;
  position: relative;
}

.article-category {
  position: absolute;
  top: 14px;
  left: 14px;
  background: #3498db;
  padding: 4px 10px;
  font-size: 0.8rem;
  color: white;
  border-radius: 20px;
}

.article-card-content {
  padding: 1.3rem;
}

.article-meta {
  display: flex;
  justify-content: space-between;
  color: #7f8c8d;
  font-size: 0.9rem;
  margin-bottom: 1rem;
}

.article-snippet {
  color: #34495e;
  margin-bottom: 1rem;
}

.read-more {
  background: #3498db;
  padding: 8px 14px;
  color: white;
  border-radius: 6px;
  display: inline-flex;
  align-items: center;
  text-decoration: none;
  transition: 0.2s;
}

.read-more:hover {
  background: #2980b9;
}

/* Empty */
.empty-state {
  text-align: center;
  padding: 3rem;
  font-size: 1.2rem;
  color: #95a5a6;
}
</style>
