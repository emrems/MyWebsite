<template>
  <div class="article-detail-page-wrapper">
    <!-- Geri Dön -->
    <button class="back-to-articles-btn" @click="$router.push('/articles')">
      <i class="fas fa-chevron-left"></i> Tüm Makaleler
    </button>

    <!-- Loading -->
    <LoadingSpinner v-if="isLoading" text="Makale yükleniyor..." />

    <!-- Error -->
    <ErrorMessage v-else-if="getError" :message="getError" :showRetry="true" @retry="retryLoad" />

    <!-- Article Content -->
    <article v-else-if="article" class="article-main-layout">
      <ArticleHero :article="article" :readingTime="calculateReadingTime(article.content)" />

      <div class="article-content-wrapper">
        <!-- Article Body Content -->
        <div class="article-body-content" v-html="article.content"></div>

        <!-- Makaleye ait tüm resimler -->
        <div v-if="articleImages.length > 0" class="article-gallery">
          <h3>Görseller</h3>
          <div class="gallery-grid">
            <div
              v-for="image in articleImages"
              :key="image.id"
              class="gallery-item"
              @click="openLightbox(image)"
            >
              <img :src="image.url" :alt="article.title" />
            </div>
          </div>
        </div>

        <!-- Makaleye ait videolar (varsa) -->
        <div v-if="articleVideos.length > 0" class="article-videos">
          <h3>Videolar</h3>
          <div class="videos-grid">
            <div v-for="video in articleVideos" :key="video.id" class="video-item">
              <video controls :src="video.url"></video>
            </div>
          </div>
        </div>

        <!-- Meta Info -->
        <div class="article-meta-info">
          <div class="tags-section">
            <span class="meta-label">Etiketler:</span>
            <span class="tag-pill" v-for="tag in article.tags" :key="tag">{{ tag }}</span>
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

        <!-- Yorum Listesi -->
        <CommentList :comments="article.comments" />

        <!-- Yorum Ekleme Formu -->
        <div class="comment-form">
          <h3>Yorum Yap</h3>

          <textarea
            v-model="newComment"
            placeholder="Yorumunuzu yazın..."
            class="comment-input"
          ></textarea>

          <button
            class="comment-submit-btn"
            :disabled="isCommentSending || !newComment.trim()"
            @click="submitComment"
          >
            <span v-if="!isCommentSending">Gönder</span>
            <span v-else>Gönderiliyor...</span>
          </button>
        </div>

        <ArticleNavigation :previousArticle="previousArticle" :nextArticle="nextArticle" />
      </div>
    </article>

    <!-- Share Modal -->
    <ShareModal
      :isVisible="isShareModalVisible"
      :shareText="article ? `Bu makaleyi oku: ${article.title}` : ''"
      :articleId="article?.id"
      @close="isShareModalVisible = false"
    />

    <!-- Lightbox Modal (resmi büyütmek için) -->
    <div v-if="lightboxImage" class="lightbox-overlay" @click="closeLightbox">
      <div class="lightbox-content" @click.stop>
        <button class="lightbox-close" @click="closeLightbox">&times;</button>
        <img :src="lightboxImage.url" :alt="article.title" />
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import ArticleHero from "@/components/Article/ArticleHero.vue";
import ArticleNavigation from "@/components/Article/ArticleNavigation.vue";
import CommentList from "@/components/Article/CommentList.vue";
import ShareModal from "@/components/ShareModal.vue";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";
import ApiService from "@/services/ApiService";

export default {
  name: "ArticleDetail",
  components: {
    ArticleHero,
    ArticleNavigation,
    CommentList,
    ShareModal,
    ErrorMessage,
    LoadingSpinner,
  },

  data() {
    return {
      isShareModalVisible: false,
      previousArticle: null,
      nextArticle: null,
      newComment: "",
      isCommentSending: false,
      lightboxImage: null, // Lightbox için
    };
  },

  computed: {
    ...mapGetters("articles", ["articleDetail", "isLoading", "getError"]),

    article() {
      return this.articleDetail;
    },

    // Sadece Image tipindeki medyaları filtrele
    articleImages() {
      if (!this.article || !this.article.mediaFiles) return [];
      return this.article.mediaFiles
        .filter((m) => m.type === 0)
        .map((m) => ({
          ...m,
          url: m.url.startsWith("http") ? m.url : `${import.meta.env.VITE_API_BASE_URL}${m.url}`,
        }));
    },

    // Sadece Video tipindeki medyaları filtrele
    articleVideos() {
      if (!this.article || !this.article.mediaFiles) return [];
      return this.article.mediaFiles.filter((m) => m.type === 1); // 1 = Video
    },
  },

  created() {
    this.loadArticle();
  },

  watch: {
    "$route.params.slug"() {
      this.loadArticle();
      window.scrollTo({ top: 0, behavior: "smooth" });
    },
  },

  methods: {
    ...mapActions("articles", ["fetchArticleBySlug"]),
    ...mapActions("comments", ["createComment"]),

    loadArticle() {
      this.fetchArticleBySlug(this.$route.params.slug);
      this.simulateNavigationArticles();
    },

    retryLoad() {
      this.loadArticle();
    },

    calculateReadingTime(content) {
      if (!content) return 1;
      const wordsPerMinute = 200;
      const textLength = content.split(/\s+/).length;
      return Math.ceil(textLength / wordsPerMinute);
    },

    simulateNavigationArticles() {
      this.previousArticle = { slug: "onceki-makale", title: "Önceki Makale" };
      this.nextArticle = { slug: "sonraki-makale", title: "Sonraki Makale" };
    },

    async likeArticle() {
      const result = await ApiService.create("ArticleLike/createArticleLike", {
        articleId: this.article.id,
      });

      if (result.success) {
        this.article.articleLikeCount = result.data.likes;
        this.article.isLiked = result.data.isliked;

        this.$notify.show({
          type: "success",
          title: "Başarılı",
          message: result.message,
        });
      } else {
        this.$notify.show({
          type: "error",
          title: "Hata",
          message: result.message,
        });
      }
    },

    async submitComment() {
      if (!this.newComment.trim()) return;

      this.isCommentSending = true;

      const response = await this.createComment({
        articleId: this.article.id,
        content: this.newComment,
      });

      if (response.success) {
        this.newComment = "";
        await this.fetchArticleBySlug(this.$route.params.slug);

        this.$notify.show({
          type: "success",
          title: "Yorum Gönderildi",
          message: response.message,
        });
      } else {
        this.$notify.show({
          type: "error",
          title: "Hata",
          message: response.message,
        });

        this.newComment = "";
      }

      this.isCommentSending = false;
    },

    // Lightbox fonksiyonları
    openLightbox(image) {
      this.lightboxImage = image;
    },

    closeLightbox() {
      this.lightboxImage = null;
    },
  },
};
</script>

<style scoped>
.comment-form {
  margin-top: 40px;
  padding: 25px;
  background: #f8fafc;
  border-radius: 12px;
}

.comment-form h3 {
  margin-bottom: 15px;
  font-weight: 700;
}

.comment-input {
  width: 100%;
  min-height: 120px;
  padding: 12px;
  border-radius: 10px;
  border: 1px solid #cbd5e1;
  resize: vertical;
}

.comment-submit-btn {
  margin-top: 15px;
  padding: 10px 20px;
  background: #3b82f6;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: 0.2s ease;
}

.comment-submit-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.comment-submit-btn:hover:not(:disabled) {
  background: #2563eb;
}

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

/* Gallery Styles */
.article-gallery {
  margin: 40px 0;
}

.article-gallery h3 {
  margin-bottom: 20px;
  font-size: 1.5rem;
  font-weight: 700;
  color: #1e293b;
}

.gallery-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 15px;
}

.gallery-item {
  position: relative;
  cursor: pointer;
  overflow: hidden;
  border-radius: 12px;
  aspect-ratio: 16/9;
  background: #f1f5f9;
}

.gallery-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.gallery-item:hover img {
  transform: scale(1.05);
}

/* Video Styles */
.article-videos {
  margin: 40px 0;
}

.article-videos h3 {
  margin-bottom: 20px;
  font-size: 1.5rem;
  font-weight: 700;
  color: #1e293b;
}

.videos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 20px;
}

.video-item video {
  width: 100%;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

/* Lightbox Styles */
.lightbox-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.9);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  cursor: pointer;
}

.lightbox-content {
  position: relative;
  max-width: 90%;
  max-height: 90%;
  cursor: default;
}

.lightbox-content img {
  max-width: 100%;
  max-height: 90vh;
  border-radius: 8px;
  box-shadow: 0 10px 50px rgba(0, 0, 0, 0.5);
}

.lightbox-close {
  position: absolute;
  top: -40px;
  right: 0;
  background: none;
  border: none;
  color: white;
  font-size: 3rem;
  cursor: pointer;
  line-height: 1;
  transition: transform 0.2s ease;
}

.lightbox-close:hover {
  transform: scale(1.2);
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
  .gallery-grid {
    grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
    gap: 10px;
  }
  .videos-grid {
    grid-template-columns: 1fr;
  }
  .lightbox-close {
    top: -50px;
    font-size: 2.5rem;
  }
}
</style>
