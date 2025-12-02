<template>
  <div class="admin-layout">
    <!-- Admin Navbar -->
    <header class="admin-navbar">
      <div class="admin-nav-content">
        <div class="admin-nav-title">
          <button @click="goBackToAdmin" class="back-btn">← Geri</button>
          <h2>Makale Yönetimi</h2>
        </div>
        <div class="admin-nav-actions">
          <button @click="handleLogout" class="admin-logout-btn">
            Çıkış Yap
          </button>
        </div>
      </div>
    </header>

    <main class="admin-main">
      <div class="article-management-container">
        <!-- Header with Add Button -->
        <div class="header-section">
          <h1>Makaleler</h1>
          <button @click="openCreateModal" class="btn-primary">
            <span class="icon">+</span> Yeni Makale Ekle
          </button>
        </div>

        <!-- Loading State -->
        <div v-if="isLoading" class="loading-state">
          <div class="spinner"></div>
          <p>Yükleniyor...</p>
        </div>

        <!-- Error State -->
        <div v-if="getError && !isLoading" class="error-message">
          {{ getError }}
        </div>

        <!-- Articles Table -->
        <div v-if="!isLoading && !getError" class="table-container">
          <table class="articles-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Başlık</th>
                <th>Slug</th>
                <th>Yazar</th>
                <th>Beğeni</th>
                <th>Yorum</th>
                <th>Tarih</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="article in allArticles" :key="article.id">
                <td>{{ article.id }}</td>
                <td>
                  <div class="article-title">
                    {{ truncateText(article.title, 40) }}
                  </div>
                </td>
                <td>
                  <span class="slug-badge">{{ article.slug || '-' }}</span>
                </td>
                <td>{{ article.authorName || 'Bilinmiyor' }}</td>
                <td>
                  <span class="stat-badge likes">
                    ❤️ {{ article.articleLikeCount }}
                  </span>
                </td>
                <td>
                  <span class="stat-badge comments">
                    💬 {{ article.comments?.length || 0 }}
                  </span>
                </td>
                <td>{{ formatDate(article.createdDate) }}</td>
                <td>
                  <div class="action-buttons">
                    <button @click="viewArticle(article)" class="btn-view" title="Görüntüle">
                      👁️
                    </button>
                    <button @click="openEditModal(article)" class="btn-edit" title="Düzenle">
                      ✏️
                    </button>
                    <button @click="confirmDelete(article)" class="btn-delete" title="Sil">
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
              <tr v-if="allArticles.length === 0">
                <td colspan="8" class="empty-state">
                  Henüz makale bulunmamaktadır.
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Create/Edit Modal -->
        <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
          <div class="modal-content modal-xlarge">
            <div class="modal-header">
              <h2>{{ isEditMode ? 'Makale Düzenle' : 'Yeni Makale Ekle' }}</h2>
              <button @click="closeModal" class="close-btn">×</button>
            </div>
            
            <form @submit.prevent="handleSubmit" class="article-form">
              <div class="form-group">
                <label for="title">Başlık *</label>
                <input
                  type="text"
                  id="title"
                  v-model="formData.title"
                  required
                  placeholder="Makale başlığı"
                />
              </div>

              <div class="form-group full-width">
                <label for="content">İçerik *</label>
                <div class="content-editor-container">
                  <textarea
                    id="content"
                    v-model="formData.content"
                    required
                    rows="25"
                    placeholder="Makale içeriğini buraya yazın..."
                    class="content-textarea"
                  ></textarea>
                  <div class="content-stats">
                    <span>{{ formData.content.length }} karakter</span>
                    <span>{{ formData.content.split(' ').filter(w => w.length > 0).length }} kelime</span>
                  </div>
                </div>
              </div>

              <div class="form-row">
                <div class="form-group">
                  <label for="slug">Slug {{ isEditMode ? '' : '*' }}</label>
                  <input
                    type="text"
                    id="slug"
                    v-model="formData.slug"
                    :required="!isEditMode"
                    placeholder="makale-url-slug"
                  />
                  <small>URL'de kullanılacak benzersiz tanımlayıcı</small>
                </div>

                <div class="form-group" v-if="!isEditMode">
                  <label for="categoryId">Kategori</label>
                  <select
                    id="categoryId"
                    v-model.number="formData.categoryId"
                    class="form-select"
                  >
                    <option :value="null">Kategori seçin (opsiyonel)</option>
                    <option 
                      v-for="category in categoriesForSelect" 
                      :key="category.value" 
                      :value="category.value"
                    >
                      {{ category.label }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="modal-actions">
                <button type="button" @click="closeModal" class="btn-secondary">
                  İptal
                </button>
                <button type="submit" class="btn-primary" :disabled="isLoading">
                  {{ isLoading ? 'Kaydediliyor...' : (isEditMode ? 'Güncelle' : 'Oluştur') }}
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- View Modal -->
        <div v-if="showViewModal" class="modal-overlay" @click.self="showViewModal = false">
          <div class="modal-content modal-xlarge">
            <div class="modal-header">
              <h2>{{ viewingArticle?.title }}</h2>
              <button @click="showViewModal = false" class="close-btn">×</button>
            </div>
            
            <div class="article-view-content">
              <div class="article-meta">
                <p><strong>Yazar:</strong> {{ viewingArticle?.authorName || 'Bilinmiyor' }}</p>
                <p><strong>Slug:</strong> {{ viewingArticle?.slug }}</p>
                <p><strong>Tarih:</strong> {{ formatDate(viewingArticle?.createdDate) }}</p>
                <p><strong>Beğeni:</strong> {{ viewingArticle?.articleLikeCount }}</p>
                <p><strong>Yorum Sayısı:</strong> {{ viewingArticle?.comments?.length || 0 }}</p>
              </div>
              
              <div class="article-content">
                <h3>İçerik:</h3>
                <div class="content-text-large">{{ viewingArticle?.content }}</div>
              </div>

              <div v-if="viewingArticle?.comments?.length > 0" class="article-comments">
                <h3>Yorumlar ({{ viewingArticle.comments.length }}):</h3>
                <div class="comment-list">
                  <div v-for="comment in viewingArticle.comments" :key="comment.id" class="comment-item">
                    <div class="comment-header">
                      <strong>{{ comment.userName }}</strong>
                      <span class="comment-date">{{ formatDate(comment.createdDate) }}</span>
                    </div>
                    <p class="comment-content">{{ comment.content }}</p>
                  </div>
                </div>
              </div>

              <div v-if="viewingArticle?.mediaFiles?.length > 0" class="article-media">
                <h3>Medya Dosyaları ({{ viewingArticle.mediaFiles.length }}):</h3>
                <div class="media-grid">
                  <div v-for="media in viewingArticle.mediaFiles" :key="media.id" class="media-item">
                    <img :src="getMediaUrl(media.url)" :alt="'Media ' + media.id" />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Delete Confirmation Modal -->
        <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
          <div class="modal-content modal-small">
            <div class="modal-header">
              <h2>Makale Sil</h2>
              <button @click="showDeleteModal = false" class="close-btn">×</button>
            </div>
            
            <div class="modal-body">
              <p>
                <strong>{{ articleToDelete?.title }}</strong> makalesini silmek istediğinizden emin misiniz?
              </p>
              <p class="warning-text">Bu işlem geri alınamaz ve tüm yorumlar silinecektir!</p>
            </div>

            <div class="modal-actions">
              <button @click="showDeleteModal = false" class="btn-secondary">
                İptal
              </button>
              <button @click="handleDelete" class="btn-danger" :disabled="isLoading">
                {{ isLoading ? 'Siliniyor...' : 'Evet, Sil' }}
              </button>
            </div>
          </div>
        </div>

        <!-- Success/Error Toast -->
        <div v-if="toast.show" :class="['toast', toast.type]">
          {{ toast.message }}
        </div>
      </div>
    </main>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'ArticleManagement',
  data() {
    return {
      showModal: false,
      showViewModal: false,
      showDeleteModal: false,
      isEditMode: false,
      articleToDelete: null,
      viewingArticle: null,
      formData: {
        title: '',
        content: '',
        slug: '',
        categoryId: null
      },
      currentArticleId: null,
      toast: {
        show: false,
        message: '',
        type: 'success'
      }
    };
  },
  computed: {
    ...mapGetters('auth', ['currentUser']),
    ...mapGetters('articles', ['allArticles', 'isLoading', 'getError']),
    ...mapGetters('categories', { categoriesForSelect: 'categoriesForSelect' })
  },
  methods: {
    ...mapActions('auth', ['logout']),
    ...mapActions('articles', ['fetchArticles', 'createArticle', 'updateArticle', 'deleteArticle']),
    ...mapActions('categories', { fetchCategories: 'fetchCategories' }),

    handleLogout() {
      this.logout();
      this.$router.push('/login');
    },

    goBackToAdmin() {
      this.$router.push('/admin');
    },

    openCreateModal() {
      this.isEditMode = false;
      this.resetForm();
      this.showModal = true;
    },

    openEditModal(article) {
      this.isEditMode = true;
      this.currentArticleId = article.id;
      this.formData = {
        title: article.title,
        content: article.content,
        slug: article.slug || '',
        categoryId: null // Update'te categoryId kullanılmıyor
      };
      this.showModal = true;
    },

    viewArticle(article) {
      this.viewingArticle = article;
      this.showViewModal = true;
    },

    closeModal() {
      this.showModal = false;
      this.resetForm();
    },

    resetForm() {
      this.formData = {
        title: '',
        content: '',
        slug: '',
        categoryId: null
      };
      this.currentArticleId = null;
    },

    async handleSubmit() {
      try {
        let result;
        
        if (this.isEditMode) {
          // Update işlemi - sadece title ve content
          const updateData = {
            id: this.currentArticleId,
            title: this.formData.title,
            content: this.formData.content
          };
          
          result = await this.updateArticle({
            articleId: this.currentArticleId,
            articleData: updateData
          });
        } else {
          // Create işlemi
          result = await this.createArticle(this.formData);
        }

        if (result.success) {
          this.showToast(result.message, 'success');
          this.closeModal();
          await this.fetchArticles();
        } else {
          this.showToast(result.message || 'İşlem başarısız oldu.', 'error');
        }
      } catch (error) {
        console.error('Form submit error:', error);
        this.showToast('Bir hata oluştu.', 'error');
      }
    },

    confirmDelete(article) {
      this.articleToDelete = article;
      this.showDeleteModal = true;
    },

    async handleDelete() {
      try {
        const result = await this.deleteArticle(this.articleToDelete.id);
        
        if (result.success) {
          this.showToast(result.message, 'success');
          this.showDeleteModal = false;
          this.articleToDelete = null;
          await this.fetchArticles();
        } else {
          this.showToast(result.message || 'Silme işlemi başarısız oldu.', 'error');
        }
      } catch (error) {
        console.error('Delete error:', error);
        this.showToast('Bir hata oluştu.', 'error');
      }
    },

    showToast(message, type = 'success') {
      this.toast = { show: true, message, type };
      setTimeout(() => {
        this.toast.show = false;
      }, 3000);
    },

    truncateText(text, length) {
      if (!text) return '-';
      return text.length > length ? text.substring(0, length) + '...' : text;
    },

    formatDate(dateString) {
      if (!dateString) return '-';
      const date = new Date(dateString);
      return date.toLocaleDateString('tr-TR', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    },

    getMediaUrl(url) {
      // API base URL ile birleştir
      return `https://localhost:7239${url}`;
    }
  },
  mounted() {
    // Sayfa yüklendiğinde makaleleri ve kategorileri çek
    this.fetchArticles();
    this.fetchCategories();
    
    // Admin yetkisi kontrolü
    if (this.currentUser.role !== 'Admin') {
      this.$router.push('/');
    }
  }
};
</script>

<style scoped>
.admin-layout {
  min-height: 100vh;
  background: #f8f9fa;
}

.admin-navbar {
  background: #2c3e50;
  color: white;
  padding: 1rem 2rem;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.admin-nav-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1600px;
  margin: 0 auto;
}

.admin-nav-title {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.admin-nav-title h2 {
  margin: 0;
  color: white;
}

.back-btn {
  background: rgba(255,255,255,0.1);
  color: white;
  border: 1px solid rgba(255,255,255,0.2);
  padding: 0.5rem 1rem;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s;
}

.back-btn:hover {
  background: rgba(255,255,255,0.2);
}

.admin-logout-btn {
  padding: 0.5rem 1.5rem;
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.3s;
}

.admin-logout-btn:hover {
  background: #c0392b;
}

.admin-main {
  padding: 2rem;
}

.article-management-container {
  max-width: 1600px;
  margin: 0 auto;
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.header-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.header-section h1 {
  color: #2c3e50;
  margin: 0;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
}

.btn-primary .icon {
  font-size: 1.2rem;
}

.loading-state {
  text-align: center;
  padding: 3rem;
}

.spinner {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  background: #fee;
  color: #c33;
  padding: 1rem;
  border-radius: 5px;
  text-align: center;
}

.table-container {
  overflow-x: auto;
}

.articles-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

.articles-table thead {
  background: #f8f9fa;
}

.articles-table th,
.articles-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #dee2e6;
}

.articles-table th {
  font-weight: 600;
  color: #495057;
}

.articles-table tbody tr:hover {
  background: #f8f9fa;
}

.article-title {
  font-weight: 500;
}

.slug-badge {
  background: #e9ecef;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.875rem;
  font-family: monospace;
}

.stat-badge {
  display: inline-block;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.875rem;
}

.stat-badge.likes {
  background: #ffe0e0;
  color: #c33;
}

.stat-badge.comments {
  background: #e0f0ff;
  color: #369;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
}

.btn-view,
.btn-edit,
.btn-delete {
  padding: 0.5rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.3s;
}

.btn-view {
  background: #3498db;
}

.btn-view:hover {
  background: #2980b9;
  transform: scale(1.1);
}

.btn-edit {
  background: #f39c12;
}

.btn-edit:hover {
  background: #e67e22;
  transform: scale(1.1);
}

.btn-delete {
  background: #e74c3c;
}

.btn-delete:hover {
  background: #c0392b;
  transform: scale(1.1);
}

.empty-state {
  text-align: center;
  color: #6c757d;
  padding: 3rem !important;
  font-style: italic;
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  padding: 1rem;
  overflow-y: auto;
}

.modal-content {
  background: white;
  border-radius: 10px;
  width: 100%;
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
  margin: auto;
}

.modal-large {
  max-width: 900px;
}

.modal-xlarge {
  max-width: 1100px;
}

.modal-small {
  max-width: 400px;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #dee2e6;
  position: sticky;
  top: 0;
  background: white;
  z-index: 10;
}

.modal-header h2 {
  margin: 0;
  color: #2c3e50;
}

.close-btn {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: #6c757d;
  line-height: 1;
  transition: color 0.3s;
}

.close-btn:hover {
  color: #2c3e50;
}

/* Article Form */
.article-form {
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-group {
  margin-bottom: 0;
}

.form-group.full-width {
  width: 100%;
}

.form-row {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #495057;
  font-weight: 600;
  font-size: 1rem;
}

.form-group input,
.form-group textarea,
.form-group select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ced4da;
  border-radius: 5px;
  font-size: 1rem;
  transition: border-color 0.3s;
  box-sizing: border-box;
  font-family: inherit;
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-group small {
  display: block;
  margin-top: 0.25rem;
  color: #6c757d;
  font-size: 0.875rem;
}

.form-select {
  background-color: white;
  cursor: pointer;
}

/* Content Editor */
.content-editor-container {
  border: 1px solid #ced4da;
  border-radius: 5px;
  overflow: hidden;
  transition: border-color 0.3s;
}

.content-editor-container:focus-within {
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.content-textarea {
  width: 100%;
  min-height: 400px;
  padding: 1rem;
  border: none;
  resize: vertical;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  font-size: 1rem;
  line-height: 1.6;
  background: #fff;
}

.content-textarea:focus {
  outline: none;
}

.content-stats {
  background: #f8f9fa;
  padding: 0.5rem 1rem;
  border-top: 1px solid #dee2e6;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  font-size: 0.875rem;
  color: #6c757d;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding: 1.5rem;
  border-top: 1px solid #dee2e6;
  position: sticky;
  bottom: 0;
  background: white;
}

.btn-secondary {
  background: #6c757d;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.3s;
}

.btn-secondary:hover {
  background: #5a6268;
}

.btn-danger {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.3s;
}

.btn-danger:hover {
  background: #c0392b;
}

.modal-body {
  padding: 1.5rem;
}

.warning-text {
  color: #e74c3c;
  font-weight: 600;
  margin-top: 1rem;
}

/* Article View Content */
.article-view-content {
  padding: 1.5rem;
  max-height: 80vh;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.article-meta {
  background: #f8f9fa;
  padding: 1.5rem;
  border-radius: 8px;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 1rem;
}

.article-meta p {
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.article-meta strong {
  color: #2c3e50;
  font-size: 0.9rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.article-content {
  flex: 1;
}

.article-content h3 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #e9ecef;
}

.content-text-large {
  white-space: pre-wrap;
  line-height: 1.8;
  color: #2c3e50;
  font-size: 1.1rem;
  background: #f8f9fa;
  padding: 2rem;
  border-radius: 8px;
  max-height: 600px;
  overflow-y: auto;
  font-family: 'Georgia', 'Times New Roman', serif;
}

.article-comments {
  border-top: 1px solid #e9ecef;
  padding-top: 2rem;
}

.article-comments h3 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.comment-list {
  max-height: 400px;
  overflow-y: auto;
}

.comment-item {
  background: #f8f9fa;
  padding: 1.5rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  border-left: 4px solid #667eea;
}

.comment-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.comment-header strong {
  color: #2c3e50;
  font-size: 1rem;
}

.comment-date {
  color: #6c757d;
  font-size: 0.875rem;
}

.comment-content {
  margin: 0;
  color: #495057;
  line-height: 1.6;
}

.article-media {
  border-top: 1px solid #e9ecef;
  padding-top: 2rem;
}

.article-media h3 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.media-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 1rem;
}

.media-item {
  border-radius: 8px;
  overflow: hidden;
  border: 1px solid #dee2e6;
}

.media-item img {
  width: 100%;
  height: 150px;
  object-fit: cover;
  display: block;
}

/* Toast Notification */
.toast {
  position: fixed;
  bottom: 2rem;
  right: 2rem;
  padding: 1rem 1.5rem;
  border-radius: 8px;
  color: white;
  font-weight: 600;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
  z-index: 2000;
  animation: slideIn 0.3s ease-out;
}

.toast.success {
  background: #27ae60;
}

.toast.error {
  background: #e74c3c;
}

@keyframes slideIn {
  from {
    transform: translateX(400px);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}

@media (max-width: 1200px) {
  .modal-xlarge {
    max-width: 95%;
  }
  
  .articles-table {
    font-size: 0.875rem;
  }

  .articles-table th,
  .articles-table td {
    padding: 0.75rem 0.5rem;
  }
}

@media (max-width: 768px) {
  .admin-nav-content {
    flex-direction: column;
    gap: 1rem;
  }

  .admin-main {
    padding: 1rem;
  }

  .article-management-container {
    padding: 1rem;
  }

  .header-section {
    flex-direction: column;
    align-items: stretch;
  }

  .articles-table {
    font-size: 0.75rem;
  }

  .form-row {
    flex-direction: column;
  }

  .modal-actions {
    flex-direction: column;
  }

  .toast {
    right: 1rem;
    left: 1rem;
    bottom: 1rem;
  }

  .content-textarea {
    min-height: 300px;
  }

  .content-text-large {
    font-size: 1rem;
    padding: 1rem;
  }

  .article-meta {
    grid-template-columns: 1fr;
    gap: 0.75rem;
  }
}

@media (max-width: 480px) {
  .action-buttons {
    flex-direction: column;
    gap: 0.25rem;
  }

  .btn-view,
  .btn-edit,
  .btn-delete {
    width: 100%;
    text-align: center;
  }

  .content-textarea {
    min-height: 250px;
    font-size: 0.95rem;
  }
}

/* Scrollbar Styling */
.content-textarea::-webkit-scrollbar,
.content-text-large::-webkit-scrollbar,
.comment-list::-webkit-scrollbar {
  width: 8px;
}

.content-textarea::-webkit-scrollbar-track,
.content-text-large::-webkit-scrollbar-track,
.comment-list::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

.content-textarea::-webkit-scrollbar-thumb,
.content-text-large::-webkit-scrollbar-thumb,
.comment-list::-webkit-scrollbar-thumb {
  background: #c1c1c1;
  border-radius: 4px;
}

.content-textarea::-webkit-scrollbar-thumb:hover,
.content-text-large::-webkit-scrollbar-thumb:hover,
.comment-list::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}
/* Media Upload Styles */
.media-upload-section {
  margin-top: 0.5rem;
}

.upload-area {
  border: 2px dashed #ced4da;
  border-radius: 8px;
  padding: 3rem 1rem;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s;
  background: #f8f9fa;
  position: relative;
}

.upload-area:hover,
.upload-area.drag-over {
  border-color: #667eea;
  background: #f0f3ff;
}

.file-input {
  display: none;
}

.upload-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  color: #6c757d;
}

.upload-text {
  font-size: 1.1rem;
  color: #495057;
  margin-bottom: 0.5rem;
  font-weight: 600;
}

.upload-hint {
  font-size: 0.875rem;
  color: #6c757d;
  margin: 0;
}

.selected-files,
.uploaded-medias {
  margin-top: 1.5rem;
}

.selected-files h4,
.uploaded-medias h4 {
  color: #495057;
  margin-bottom: 1rem;
  font-size: 1rem;
}

.file-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  max-height: 200px;
  overflow-y: auto;
}

.file-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem;
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 6px;
  transition: all 0.3s;
}

.file-item:hover {
  border-color: #667eea;
  box-shadow: 0 2px 8px rgba(102, 126, 234, 0.1);
}

.file-preview {
  width: 60px;
  height: 60px;
  flex-shrink: 0;
  border-radius: 4px;
  overflow: hidden;
  background: #f8f9fa;
  display: flex;
  align-items: center;
  justify-content: center;
}

.file-preview img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.video-preview {
  font-size: 1.5rem;
  color: #6c757d;
}

.file-info {
  flex: 1;
  min-width: 0;
}

.file-name {
  margin: 0 0 0.25rem 0;
  color: #495057;
  font-weight: 500;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.file-size {
  margin: 0;
  color: #6c757d;
  font-size: 0.875rem;
}

.remove-file-btn {
  width: 30px;
  height: 30px;
  border: none;
  background: #e74c3c;
  color: white;
  border-radius: 50%;
  cursor: pointer;
  font-size: 1.2rem;
  line-height: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.3s;
  flex-shrink: 0;
}

.remove-file-btn:hover {
  background: #c0392b;
}

.media-grid-preview {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
  gap: 0.75rem;
}

.media-preview-item {
  position: relative;
  border-radius: 6px;
  overflow: hidden;
  border: 1px solid #e9ecef;
  aspect-ratio: 1;
}

.media-preview-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.video-preview-item {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
}

.remove-media-btn {
  position: absolute;
  top: 5px;
  right: 5px;
  width: 25px;
  height: 25px;
  border: none;
  background: rgba(231, 76, 60, 0.9);
  color: white;
  border-radius: 50%;
  cursor: pointer;
  font-size: 1rem;
  line-height: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.3s;
}

.remove-media-btn:hover {
  background: #c0392b;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .upload-area {
    padding: 2rem 1rem;
  }
  
  .upload-icon {
    font-size: 2.5rem;
  }
  
  .upload-text {
    font-size: 1rem;
  }
  
  .media-grid-preview {
    grid-template-columns: repeat(auto-fill, minmax(80px, 1fr));
  }
}

@media (max-width: 480px) {
  .file-item {
    flex-direction: column;
    text-align: center;
    gap: 0.5rem;
  }
  
  .file-info {
    width: 100%;
  }
  
  .media-grid-preview {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>