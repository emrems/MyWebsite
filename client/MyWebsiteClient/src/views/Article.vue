<template>
  <div class="articles-container">
    <div class="page-header">
      <h1>Blog Yazıları</h1>
      <p class="page-subtitle">Teknoloji, tasarım ve yazılım dünyasından en güncel içerikler</p>
    </div>
    
    <!-- Loading State -->
    <div v-if="isLoading" class="status-message loading-spinner">
      <div class="spinner"></div>
      <p>Yazılar yükleniyor...</p>
    </div>
    
    <!-- Error State -->
    <div v-else-if="getError" class="status-message error-message">
      <i class="icon-error"></i>
      <p>{{ getError }}</p>
      <button @click="fetchArticles" class="retry-button">Tekrar Dene</button>
    </div>
    
    <!-- Articles Grid -->
    <div v-else-if="allArticles && allArticles.length > 0" class="articles-grid">
      <div v-for="article in allArticles" :key="article.id" class="article-card">
        <div class="article-image" :style="{ backgroundImage: 'url(https://picsum.photos/400/250?random=' + article.id + ')' }">
          <div class="article-category">{{ article.category || 'Teknoloji' }}</div>
        </div>
        <div class="article-card-content">
          <h2>{{ article.title }}</h2>
          <div class="article-meta">
            <span class="article-date">
              <i class="icon-calendar"></i>
              {{ formatDate(article.createdDate) }}
            </span>
            <span class="reading-time">
              <i class="icon-clock"></i>
              {{ calculateReadingTime(article.content) }} dk okuma
            </span>
          </div>
          <p class="article-snippet">{{ truncateContent(article.content, 140) }}</p>
          <RouterLink :to="`/articles/${article.slug}`" class="read-more-link">
            Devamını Oku <i class="icon-arrow"></i>
          </RouterLink>
        </div>
      </div>
    </div>
    
    <!-- Empty State -->
    <div v-else class="status-message empty-state">
      <i class="icon-empty"></i>
      <p>Henüz makale bulunmamaktadır.</p>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  computed: {
    ...mapGetters('articles', ['allArticles', 'isLoading', 'getError'])
  },
  
  methods: {
    ...mapActions('articles', ['fetchArticles']),

    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('tr-TR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    },

    truncateContent(content, length) {
      return content.length > length ? content.substring(0, length) + '...' : content;
    },

    calculateReadingTime(content) {
      const wordsPerMinute = 200;
      const wordCount = content.split(' ').length;
      return Math.ceil(wordCount / wordsPerMinute);
    }
  },

  created() {
    this.fetchArticles();
  }
};
</script>

<style scoped>
.articles-container {
  max-width: 1200px;
  padding: 80px 1rem 0;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 3rem;
}

.page-header h1 {
  font-size: 2.5rem;
  color: #2c3e50;
  margin-bottom: 0.5rem;
  font-weight: 700;
}

.page-subtitle {
  font-size: 1.1rem;
  color: #7f8c8d;
  max-width: 600px;
  margin: 0 auto;
}

/* Status Messages */
.status-message {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  text-align: center;
}

.loading-spinner {
  color: #3498db;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  color: #e74c3c;
  background: #fdf0ed;
  padding: 2rem;
  border-radius: 8px;
  border-left: 4px solid #e74c3c;
}

.error-message .icon-error {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.retry-button {
  margin-top: 1rem;
  padding: 0.5rem 1.5rem;
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background 0.3s;
}

.retry-button:hover {
  background: #c0392b;
}

.empty-state {
  color: #95a5a6;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.empty-state .icon-empty {
  font-size: 3rem;
  margin-bottom: 1rem;
}

/* Articles Grid */
.articles-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 2rem;
}

/* Article Card */
.article-card {
  background-color: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
}

.article-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 16px 30px rgba(0, 0, 0, 0.1);
}

.article-image {
  height: 250px;
  background-size: cover;
  background-position: center;
  position: relative;
}

.article-category {
  position: absolute;
  top: 15px;
  left: 15px;
  background: #3498db;
  color: white;
  padding: 0.3rem 0.8rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
}

/* Card Content */
.article-card-content {
  padding: 1.5rem;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.article-card-content h2 {
  font-size: 1.4rem;
  color: #2c3e50;
  margin-bottom: 0.8rem;
  line-height: 1.4;
  font-weight: 600;
}

/* Meta Info */
.article-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.85rem;
  color: #7f8c8d;
  margin-bottom: 1rem;
}

.article-meta i {
  margin-right: 0.3rem;
}

/* Snippet */
.article-snippet {
  flex-grow: 1;
  font-size: 1rem;
  color: #34495e;
  line-height: 1.6;
  margin-bottom: 1.2rem;
}

/* Read More Button */
.read-more-link {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0.6rem 1.2rem;
  background-color: #3498db;
  color: white;
  text-decoration: none;
  border-radius: 6px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.read-more-link:hover {
  background-color: #2980b9;
  transform: translateY(-2px);
}

.read-more-link .icon-arrow {
  margin-left: 0.5rem;
  transition: transform 0.3s ease;
}

.read-more-link:hover .icon-arrow {
  transform: translateX(3px);
}

/* İkonlar */
[class^="icon-"] {
  display: inline-block;
  width: 1em;
  height: 1em;
  background-size: cover;
}

.icon-calendar:before { content: "📅"; }
.icon-arrow:before { content: "→"; }
.icon-clock:before { content: "🕒"; }
.icon-error:before { content: "❗"; }
.icon-empty:before { content: "📝"; }

/* Responsive */
@media (max-width: 768px) {
  .articles-grid {
    grid-template-columns: 1fr;
  }
  
  .page-header h1 {
    font-size: 2rem;
  }
  
  .article-meta {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.3rem;
  }
  
  .read-more-link {
    width: 100%;
    justify-content: center;
  }
}
</style>
