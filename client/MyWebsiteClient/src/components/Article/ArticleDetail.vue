<template>
  <div class="article-detail-page-wrapper">
    <!-- Geri Dön Butonu -->
    <button class="back-to-articles-btn" @click="$router.push('/articles')">
      <i class="fas fa-chevron-left"></i> Tüm Makaleler
    </button>

    <!-- Yükleniyor Durumu -->
    <div v-if="loading" class="status-section loading-spinner">
      <div class="spinner"></div>
      <p>Makale yükleniyor...</p>
    </div>

    <!-- Hata Durumu -->
    <div v-else-if="error" class="status-section error-message-box">
      <i class="fas fa-exclamation-triangle"></i>
      <p>{{ error }}</p>
      <RouterLink to="/articles" class="btn btn-primary">Blog Sayfasına Geri Dön</RouterLink>
    </div>

    <!-- Makale İçeriği -->
    <article v-else-if="article" class="article-main-layout">
      <ArticleHero :article="article" :readingTime="calculateReadingTime(article.content)" />

      <div class="article-content-wrapper">
        <div class="article-body-content" v-html="article.content"></div>

        <!-- Meta Bilgiler ve Aksiyonlar -->
        <div class="article-meta-info">
          <div class="tags-section">
            <span class="meta-label">Etiketler:</span>
            <span class="tag-pill" v-for="tag in article.tags" :key="tag">{{ tag }}</span>
            <span class="tag-pill">Genel</span>
          </div>

          <div class="actions-section">
            <span class="meta-label">Paylaş:</span>
            <button class="action-icon-btn" @click="isShareModalVisible = true">
              <i class="fas fa-share-alt"></i>
            </button>
            <button class="action-icon-btn" @click="likeArticle">
              <i class="fas fa-heart"></i>
              <span>{{ article.articleLikeCount || 0 }}</span>
            </button>
          </div>
        </div>

        <!-- Navigasyon -->
        <ArticleNavigation :previousArticle="previousArticle" :nextArticle="nextArticle" />
      </div>
    </article>

    <!-- Share Modal -->
    <ShareModal
      :isVisible="isShareModalVisible"
      :shareText="article ? `Bu makaleyi oku: ${article.title}` : 'İlginç bir makale!'"
      :articleId="article?.id"
      @close="isShareModalVisible = false"
      @comment-submitted="onCommentSubmitted"
    />
  </div>
</template>

<script>
import ApiService from "@/services/ApiService";
import ArticleHero from "@/components/Article/ArticleHero.vue";
import ArticleNavigation from "@/components/Article/ArticleNavigation.vue";
import ShareModal from "@/components/ShareModal.vue";

export default {
  name: "ArticleDetail",
  components: { ArticleHero, ArticleNavigation, ShareModal },
  data() {
    return {
      article: null,
      previousArticle: null,
      nextArticle: null,
      loading: true,
      error: null,
      isShareModalVisible: false,
    };
  },
  async created() {
    await this.fetchArticleBySlug(this.$route.params.slug);
  },
  mounted() {
    window.scrollTo({ top: 0, behavior: "smooth" });
  },
  watch: {
    "$route.params.slug": {
      immediate: true,
      async handler(newSlug) {
        await this.fetchArticleBySlug(newSlug);
        window.scrollTo({ top: 0, behavior: "smooth" });
      },
    },
  },
  methods: {
    onCommentSubmitted(commentData) {
      console.log("Yorum gönderildi:", commentData);
    },

    async fetchArticleBySlug(slug) {
      this.loading = true;
      this.error = null;
      this.article = null;
      this.previousArticle = null;
      this.nextArticle = null;

      const result = await ApiService.fetch(`article/slug/${slug}`);
      if (result.success) {
        this.article = result.data;
        this.simulateNavigationArticles();
      } else {
        this.error = result.message || "Makale bulunamadı veya yüklenirken bir hata oluştu.";
      }
      this.loading = false;
    },

    calculateReadingTime(content) {
      if (!content) return 1;
      const wordsPerMinute = 200;
      const textLength = content.split(/\s+/).length;
      return Math.ceil(textLength / wordsPerMinute);
    },

    simulateNavigationArticles() {
      this.previousArticle = { slug: "onceki-makale-ornek", title: "Önceki Makale Başlığı" };
      this.nextArticle = { slug: "sonraki-makale-ornek", title: "Sonraki Makale Başlığı" };
    },

    async likeArticle() {
      try {
        const result = await ApiService.create("ArticleLike/createArticleLike", {
          articleId: this.article.id,
        });

        if (result.success) {
          const data = result.data;
          console.log(result); 

          // DB’den gelen değerleri kullanma
          this.article.articleLikeCount = data.likes || 0;
          this.article.isLiked = data.isliked || false;

          this.$notify.show({
            type: "success",
            title: "Başarılı",
            message: result.message,
            duration: 3000,
          });
        } else {
          this.$notify.show({
            type: "error",
            title: "Hata",
            message: result.message || "Beğeni işlemi başarısız.",
            duration: 4000,
          });
        }
      } catch (error) {
        this.$notify.show({
          type: "error",
          title: "Hata",
          message: "Bir hata oluştu. Lütfen tekrar deneyin.",
          duration: 4000,
        });
      }
    },
  },
};
</script>

<style scoped>
.article-detail-page-wrapper {
  padding-top: 80px;
  background-color: #fcfcfc;
  min-height: 100vh;
  font-family: "Inter", sans-serif;
  color: #334155;
}

.back-to-articles-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  background: none;
  border: none;
  color: #64748b;
  font-weight: 600;
  font-size: 0.95rem;
  cursor: pointer;
  padding: 20px 0;
  margin: 0 auto;
  max-width: 860px;
  width: 100%;
  transition: color 0.2s ease;
}
.back-to-articles-btn:hover {
  color: #3b82f6;
}

.status-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 50vh;
  text-align: center;
  padding: 40px;
}
.loading-spinner .spinner {
  width: 50px;
  height: 50px;
  border: 5px solid #e0e7ff;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 20px;
}
.loading-spinner p {
  color: #5a6781;
  font-size: 1.1rem;
}
@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.error-message-box {
  background: #fff0f0;
  color: #ef4444;
  padding: 40px;
  border-radius: 12px;
  max-width: 600px;
  margin: 40px auto;
  box-shadow: 0 5px 20px rgba(239, 68, 68, 0.1);
}
.error-message-box i {
  font-size: 3rem;
  margin-bottom: 20px;
}
.error-message-box p {
  font-size: 1.2rem;
  margin-bottom: 25px;
}
.error-message-box .btn-primary {
  background: #ef4444;
  color: white;
  padding: 12px 25px;
  border-radius: 8px;
  font-weight: 600;
  text-decoration: none;
  transition: background 0.3s ease;
}
.error-message-box .btn-primary:hover {
  background: #dc2626;
}

.article-main-layout {
  padding-bottom: 80px;
}

.article-content-wrapper {
  max-width: 800px;
  margin: -80px auto 0 auto;
  background: white;
  padding: 60px;
  border-radius: 16px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.08);
  position: relative;
  z-index: 10;
}

.article-body-content {
  line-height: 1.75;
  font-size: 1.05rem;
  color: #475569;
  font-family: "Merriweather", serif;
}
.article-body-content :deep(p) {
  margin-bottom: 1.5rem;
}
.article-body-content :deep(h2),
.article-body-content :deep(h3),
.article-body-content :deep(h4) {
  font-family: "Inter", sans-serif;
  color: #1e293b;
  margin: 2.5rem 0 1rem;
  font-weight: 700;
}
.article-body-content :deep(h2) {
  font-size: 2rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e2e8f0;
}
.article-body-content :deep(h3) {
  font-size: 1.6rem;
}
.article-body-content :deep(img) {
  max-width: 100%;
  height: auto;
  border-radius: 12px;
  margin: 2rem 0;
  display: block;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
}
.article-body-content :deep(blockquote) {
  border-left: 5px solid #3b82f6;
  padding: 1.5rem 2rem;
  margin: 2rem 0;
  background: #f8fafc;
  color: #475569;
  font-style: italic;
  border-radius: 0 8px 8px 0;
}
.article-body-content :deep(pre) {
  background: #2d3748;
  color: #f8fafc;
  padding: 1.5rem;
  border-radius: 8px;
  overflow-x: auto;
  margin: 2rem 0;
  font-family: "Fira Code", monospace;
  font-size: 0.95rem;
  line-height: 1.5;
}
.article-body-content :deep(code) {
  background: rgba(0, 0, 0, 0.05);
  padding: 0.2em 0.4em;
  border-radius: 4px;
  font-family: "Fira Code", monospace;
}
.article-body-content :deep(pre code) {
  background: none;
  padding: 0;
}

.article-meta-info {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  padding: 30px 0;
  border-top: 1px solid #e2e8f0;
  margin-top: 40px;
  gap: 20px;
}

.meta-label {
  font-weight: 700;
  color: #64748b;
  margin-right: 10px;
  font-size: 0.95rem;
}
.tags-section,
.actions-section {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
}
.tag-pill {
  background: #e2e8f0;
  color: #475569;
  padding: 6px 15px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
  transition: background 0.2s ease;
  cursor: pointer;
}
.tag-pill:hover {
  background: #cbd5e1;
}

.action-icon-btn {
  background: #f1f5f9;
  border: none;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #64748b;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
}
.action-icon-btn:hover {
  background: #e2e8f0;
  color: #3b82f6;
  transform: translateY(-2px);
}
.action-icon-btn .fa-heart {
  color: #ef4444;
}
.action-icon-btn span {
  font-size: 0.85rem;
  margin-left: 5px;
  color: #475569;
  font-weight: 600;
}

@media (max-width: 768px) {
  .article-content-wrapper {
    padding: 30px;
    margin-top: -60px;
    border-radius: 8px;
  }
  .back-to-articles-btn {
    padding: 15px 20px;
    font-size: 0.9rem;
  }
  .article-body-content {
    font-size: 1rem;
  }
  .article-body-content :deep(h2) {
    font-size: 1.6rem;
  }
  .article-body-content :deep(h3) {
    font-size: 1.3rem;
  }
  .article-meta-info {
    flex-direction: column;
    align-items: flex-start;
    gap: 25px;
  }
  .tags-section,
  .actions-section {
    width: 100%;
    justify-content: flex-start;
  }
}
</style>
