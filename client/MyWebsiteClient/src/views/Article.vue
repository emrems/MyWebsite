<template>
  <div class="articles-container">
    <div class="page-header">
      <h1>Blog Yazıları</h1>
      <p class="page-subtitle">Teknoloji, tasarım ve yazılım dünyasından en güncel içerikler</p>
    </div>
    
    <div v-if="loading" class="status-message loading-spinner">
      <div class="spinner"></div>
      <p>Yazılar yükleniyor...</p>
    </div>
    
    <div v-if="error" class="status-message error-message">
      <i class="icon-error"></i>
      <p>{{ error }}</p>
      <button @click="fetchArticles" class="retry-button">Tekrar Dene</button>
    </div>
    
    <div v-else-if="articles.length > 0" class="articles-grid">
      <div v-for="article in articles" :key="article.id" class="article-card">
        <div class="article-image" :style="{ backgroundImage: 'url(https://picsum.photos/400/200?random=' + article.id + ')' }">
          <div class="article-category">Teknoloji</div>
        </div>
        <div class="article-card-content">
          <h2>{{ article.title }}</h2>
          <p class="article-date">
            <i class="icon-calendar"></i>
            {{ formatDate(article.createdDate) }}
          </p>
          <p class="article-content">{{ truncateContent(article.content, 120) }}</p>
          <div class="article-footer">
            <RouterLink :to="`/articles/${article.slug}`" class="read-more-link">
              Devamını Oku
              <i class="icon-arrow"></i>
            </RouterLink>
            <div class="reading-time">
              <i class="icon-clock"></i>
              {{ calculateReadingTime(article.content) }} dk okuma
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <div v-else class="status-message empty-state">
      <i class="icon-empty"></i>
      <p>Henüz makale bulunmamaktadır.</p>
    </div>
  </div>
</template>

<script>
import ApiService from '@/services/ApiService';

export default {
  data() {
    return {
      articles: [],
      loading: true,
      error: null
    };
  },
  created() {
    this.fetchArticles();
  },
  methods: {
    async fetchArticles() {
      try {
        const response = await ApiService.get('article/all');
        this.articles = response.data;
      } catch (exception) {
        console.error('API isteği başarısız oldu:', exception);
        this.error = 'Makaleler yüklenirken bir hata oluştu.';
      } finally {
        this.loading = false;
      }
    },
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
  }
};
</script>

<style scoped>
.articles-container {
  max-width: 1200px;
  margin: 2rem auto;
  padding: 0 1rem;
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
}

.empty-state .icon-empty {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.articles-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 2rem;
}

.article-card {
  background-color: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
}

.article-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 25px rgba(0, 0, 0, 0.15);
}

.article-image {
  height: 200px;
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

.article-card-content {
  padding: 1.5rem;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
}

.article-card h2 {
  font-size: 1.4rem;
  color: #2c3e50;
  margin-bottom: 0.8rem;
  line-height: 1.4;
  font-weight: 600;
}

.article-date {
  font-size: 0.85rem;
  color: #7f8c8d;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
}

.article-date .icon-calendar {
  margin-right: 0.5rem;
  font-size: 0.9rem;
}

.article-content {
  font-size: 1rem;
  color: #34495e;
  line-height: 1.6;
  margin-bottom: 1.5rem;
  flex-grow: 1;
}

.article-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.read-more-link {
  display: inline-flex;
  align-items: center;
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
  transform: translateX(5px);
}

.read-more-link .icon-arrow {
  margin-left: 0.5rem;
  transition: transform 0.3s ease;
}

.read-more-link:hover .icon-arrow {
  transform: translateX(3px);
}

.reading-time {
  font-size: 0.8rem;
  color: #95a5a6;
  display: flex;
  align-items: center;
}

.reading-time .icon-clock {
  margin-right: 0.3rem;
}

/* İkonlar için base stil (gerçek uygulamada font-icon veya SVG kullanılmalı) */
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

@media (max-width: 768px) {
  .articles-grid {
    grid-template-columns: 1fr;
  }
  
  .page-header h1 {
    font-size: 2rem;
  }
  
  .article-footer {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
}
</style>