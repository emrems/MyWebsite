<template>
  <div class="article-detail-container">
    <div v-if="loading" class="status-message loading-spinner">
      <div class="spinner"></div>
      <p>Makale yükleniyor...</p>
    </div>
    
    <div v-if="error" class="status-message error-message">
      <i class="icon-error"></i>
      <p>{{ error }}</p>
      <RouterLink to="/articles" class="back-button">← Blog Sayfasına Dön</RouterLink>
    </div>

    <div v-if="article" class="article-detail">
      <div class="article-hero">
        <div class="article-hero-image" :style="{ backgroundImage: 'url(https://picsum.photos/1200/600?random=' + article.id + ')' }"></div>
        <div class="article-hero-content">
          <div class="article-breadcrumb">
            <RouterLink to="/">Ana Sayfa</RouterLink> 
            <span class="breadcrumb-separator">/</span>
            <RouterLink to="/articles">Blog</RouterLink>
            <span class="breadcrumb-separator">/</span>
            <span>{{ article.title }}</span>
          </div>
          
          <h1 class="article-title">{{ article.title }}</h1>
          
          <div class="article-meta">
            <div class="meta-item">
              <i class="icon-calendar"></i>
              <span>{{ formatDate(article.createdDate) }}</span>
            </div>
            <div class="meta-item">
              <i class="icon-clock"></i>
              <span>{{ calculateReadingTime(article.content) }} dk okuma</span>
            </div>
            <div class="meta-item">
              <i class="icon-user"></i>
              <span>Yazar: Admin</span>
            </div>
          </div>
        </div>
      </div>
      
      <div class="article-body">
        <div class="article-content" v-html="article.content"></div>
        
        <div class="article-tags">
          <span class="tag-label">Etiketler:</span>
          <span class="tag">Teknoloji</span>
          <span class="tag">Yazılım</span>
          <span class="tag">Tasarım</span>
        </div>
        
        <div class="article-actions">
          <button class="action-button">
            <i class="icon-like"></i> Beğen
          </button>
          <button class="action-button">
            <i class="icon-share"></i> Paylaş
          </button>
          <button class="action-button">
            <i class="icon-comment"></i> Yorum Yap
          </button>
        </div>
        
        <div class="article-navigation">
          <RouterLink v-if="previousArticle" :to="`/articles/${previousArticle.slug}`" class="nav-link prev-article">
            <i class="icon-arrow-left"></i>
            <div class="nav-content">
              <span>Önceki Yazı</span>
              <p>{{ previousArticle.title }}</p>
            </div>
          </RouterLink>
          
          <RouterLink to="/articles" class="all-articles-link">
            <i class="icon-grid"></i> Tüm Yazılar
          </RouterLink>
          
          <RouterLink v-if="nextArticle" :to="`/articles/${nextArticle.slug}`" class="nav-link next-article">
            <div class="nav-content">
              <span>Sonraki Yazı</span>
              <p>{{ nextArticle.title }}</p>
            </div>
            <i class="icon-arrow-right"></i>
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from '@/services/ApiService';

export default {
  data() {
    return {
      article: null,
      previousArticle: null,
      nextArticle: null,
      loading: true,
      error: null
    };
  },
  
  created() {
    const slug = this.$route.params.slug;
    this.fetchArticleBySlug(slug);
  },
  
  watch: {
    '$route.params.slug'(newSlug) {
      this.fetchArticleBySlug(newSlug);
    }
  },
  
  methods: {
    async fetchArticleBySlug(slug) {
      this.loading = true;
      this.error = null;
      this.article = null;

      const result = await ApiService.fetch(`article/slug/${slug}`);
      
      if (result.success) {
        this.article = result.data;
        this.simulateNavigationArticles();
      } else {
        this.error = result.message || 'Makale bulunamadı veya yüklenirken bir hata oluştu.';
      }
      
      this.loading = false;
    },
    
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('tr-TR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    },
    
    calculateReadingTime(content) {
      const wordsPerMinute = 200;
      const wordCount = content.split(' ').length;
      return Math.ceil(wordCount / wordsPerMinute);
    },
    
    simulateNavigationArticles() {
      this.previousArticle = {
        slug: 'previous-article-slug',
        title: 'Önceki Makale Başlığı'
      };
      
      this.nextArticle = {
        slug: 'next-article-slug',
        title: 'Sonraki Makale Başlığı'
      };
    }
  }
};
</script>

<style scoped>
.article-detail-container {
  max-width: 100%;
}

.status-message {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 50vh;
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
  padding: 3rem;
  border-radius: 8px;
  max-width: 600px;
  margin: 2rem auto;
}

.error-message .icon-error {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.back-button {
  margin-top: 1.5rem;
  color: #e74c3c;
  text-decoration: none;
  font-weight: 500;
  display: inline-flex;
  align-items: center;
}

.back-button:hover {
  text-decoration: underline;
}

.article-detail {
  background: white;
}

.article-hero {
  position: relative;
  margin-bottom: 2rem;
}

.article-hero-image {
  height: 400px;
  background-size: cover;
  background-position: center;
  position: relative;
}

.article-hero-image:after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 50%;
  background: linear-gradient(to bottom, transparent 0%, rgba(0,0,0,0.7) 100%);
}

.article-hero-content {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  padding: 2rem;
  color: white;
  z-index: 2;
}

.article-breadcrumb {
  margin-bottom: 1rem;
  font-size: 0.9rem;
}

.article-breadcrumb a {
  color: rgba(255, 255, 255, 0.8);
  text-decoration: none;
}

.article-breadcrumb a:hover {
  color: white;
  text-decoration: underline;
}

.breadcrumb-separator {
  margin: 0 0.5rem;
  color: rgba(255, 255, 255, 0.6);
}

.article-title {
  font-size: 2.8rem;
  font-weight: 700;
  line-height: 1.2;
  margin-bottom: 1.5rem;
  text-shadow: 0 2px 4px rgba(0,0,0,0.5);
}

.article-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  margin-bottom: 1rem;
}

.meta-item {
  display: flex;
  align-items: center;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.9);
}

.meta-item i {
  margin-right: 0.5rem;
  font-size: 1rem;
}

.article-body {
  max-width: 800px;
  margin: 0 auto;
  padding: 0 1.5rem 3rem;
}

.article-content {
  font-size: 1.1rem;
  line-height: 1.8;
  color: #2c3e50;
  margin-bottom: 2rem;
}

/* API'den gelen HTML içeriğinin stilini düzenler */
.article-content >>> p {
  margin-bottom: 1.5rem;
}

.article-content >>> h2,
.article-content >>> h3 {
  margin: 2rem 0 1rem;
  color: #2c3e50;
  font-weight: 600;
}

.article-content >>> h2 {
  font-size: 1.8rem;
  border-bottom: 2px solid #ecf0f1;
  padding-bottom: 0.5rem;
}

.article-content >>> h3 {
  font-size: 1.5rem;
}

.article-content >>> img {
  max-width: 100%;
  height: auto;
  border-radius: 8px;
  margin: 1.5rem 0;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.article-content >>> blockquote {
  border-left: 4px solid #3498db;
  padding-left: 1.5rem;
  margin: 1.5rem 0;
  font-style: italic;
  color: #7f8c8d;
  background: #f8f9fa;
  padding: 1.5rem;
  border-radius: 0 8px 8px 0;
}

.article-content >>> code {
  background: #f8f9fa;
  padding: 0.2rem 0.4rem;
  border-radius: 4px;
  font-family: 'Courier New', monospace;
}

.article-content >>> pre {
  background: #2d2d2d;
  color: #f8f9fa;
  padding: 1.5rem;
  border-radius: 8px;
  overflow-x: auto;
  margin: 1.5rem 0;
}

.article-content >>> ul, 
.article-content >>> ol {
  margin: 1rem 0;
  padding-left: 1.5rem;
}

.article-content >>> li {
  margin-bottom: 0.5rem;
}

.article-tags {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 2rem;
  padding: 1.5rem 0;
  border-top: 1px solid #ecf0f1;
  border-bottom: 1px solid #ecf0f1;
}

.tag-label {
  font-weight: 600;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.tag {
  background: #ecf0f1;
  padding: 0.3rem 0.8rem;
  border-radius: 20px;
  font-size: 0.85rem;
  color: #7f8c8d;
  transition: all 0.3s;
}

.tag:hover {
  background: #3498db;
  color: white;
  cursor: pointer;
}

.article-actions {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-bottom: 3rem;
}

.action-button {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.7rem 1.5rem;
  background: white;
  border: 1px solid #ddd;
  border-radius: 6px;
  color: #7f8c8d;
  cursor: pointer;
  transition: all 0.3s;
}

.action-button:hover {
  background: #3498db;
  color: white;
  border-color: #3498db;
}

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

/* İkonlar için base stil */
[class^="icon-"] {
  display: inline-block;
}

.icon-calendar:before { content: "📅"; }
.icon-clock:before { content: "🕒"; }
.icon-user:before { content: "👤"; }
.icon-error:before { content: "❗"; }
.icon-like:before { content: "👍"; }
.icon-share:before { content: "📤"; }
.icon-comment:before { content: "💬"; }
.icon-arrow-left:before { content: "←"; }
.icon-arrow-right:before { content: "→"; }
.icon-grid:before { content: "≡"; }

@media (max-width: 768px) {
  .article-hero-image {
    height: 300px;
  }
  
  .article-title {
    font-size: 2rem;
  }
  
  .article-meta {
    flex-direction: column;
    gap: 0.8rem;
  }
  
  .article-body {
    padding: 0 1rem 2rem;
  }
  
  .article-navigation {
    flex-direction: column;
  }
  
  .nav-link, .all-articles-link {
    max-width: 100%;
    width: 100%;
  }
  
  .article-actions {
    flex-wrap: wrap;
  }
}
</style>