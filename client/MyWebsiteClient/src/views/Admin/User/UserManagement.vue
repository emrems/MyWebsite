<template>
  <div class="admin-layout">
    <!-- Admin Navbar -->
    <header class="admin-navbar">
      <div class="admin-nav-content">
        <div class="admin-nav-title">
          <button @click="goBackToAdmin" class="back-btn">← Admin Paneli</button>
          <h2>Kullanıcı Yönetimi</h2>
        </div>
        <div class="admin-nav-actions">
          <button @click="handleLogout" class="admin-logout-btn">
            Çıkış Yap
          </button>
        </div>
      </div>
    </header>

    <main class="admin-main">
      <div class="user-management-container">
        <!-- Header -->
        <div class="header-section">
          <h1>Kullanıcılar</h1>
          <div class="stats-container">
            <div class="stat-card">
              <h3>Toplam Kullanıcı</h3>
              <p class="stat-number">{{ userCount }}</p>
            </div>
            <div class="stat-card">
              <h3>Admin Sayısı</h3>
              <p class="stat-number admin-count">{{ adminCount }}</p>
            </div>
            <div class="stat-card">
              <h3>Normal Kullanıcı</h3>
              <p class="stat-number user-count">{{ userCount - adminCount }}</p>
            </div>
          </div>
        </div>

        <!-- Loading State -->
        <div v-if="isLoading" class="loading-state">
          <div class="spinner"></div>
          <p>Kullanıcılar yükleniyor...</p>
        </div>

        <!-- Error State -->
        <div v-if="getError && !isLoading" class="error-message">
          {{ getError }}
        </div>

        <!-- Users Table -->
        <div v-if="!isLoading && !getError" class="table-container">
          <div class="table-controls">
            <div class="search-box">
              <input 
                type="text" 
                v-model="searchQuery" 
                placeholder="Kullanıcı ara..."
                class="search-input"
              />
              <span class="search-icon">🔍</span>
            </div>
            
            <div class="filter-buttons">
              <button 
                @click="filterRole = 'all'" 
                :class="{ active: filterRole === 'all' }"
                class="filter-btn"
              >
                Tümü
              </button>
              <button 
                @click="filterRole = 'Admin'" 
                :class="{ active: filterRole === 'Admin' }"
                class="filter-btn admin-filter"
              >
                Adminler
              </button>
              <button 
                @click="filterRole = 'User'" 
                :class="{ active: filterRole === 'User' }"
                class="filter-btn user-filter"
              >
                Kullanıcılar
              </button>
            </div>
          </div>

          <table class="users-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Kullanıcı Adı</th>
                <th>E-posta</th>
                <th>Rol</th>
                <th>Makaleler</th>
                <th>Yorumlar</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="user in filteredUsers" :key="user.id">
                <td>{{ user.id }}</td>
                <td>
                  <div class="user-info">
                    <span class="username">{{ user.username }}</span>
                  </div>
                </td>
                <td>
                  <span class="email">{{ user.email }}</span>
                </td>
                <td>
                  <span :class="['role-badge', user.role.toLowerCase()]">
                    {{ user.role }}
                  </span>
                </td>
                <td>
                  <div class="article-info">
                    <span class="article-count">{{ user.articles?.length || 0 }}</span>
                    <span 
                      class="article-detail" 
                      v-if="user.articles && user.articles.length > 0" 
                      @click="viewUserArticles(user)" 
                      title="Sadece Makaleleri Gör"
                    >
                      📝
                    </span>
                  </div>
                </td>
                <td>
                  <div class="comment-info">
                    <span class="comment-count">{{ user.comments?.length || 0 }}</span>
                    <span 
                      class="comment-detail" 
                      v-if="user.comments && user.comments.length > 0" 
                      @click="viewUserComments(user)" 
                      title="Sadece Yorumları Gör"
                    >
                      💬
                    </span>
                  </div>
                </td>
                <td>
                  <div class="action-buttons">
                    <button @click="viewUserDetails(user)" class="btn-view" title="Tüm Detayları Gör">
                      👁️
                    </button>
                    <button @click="openEditModal(user)" class="btn-edit" title="Düzenle">
                      ✏️
                    </button>
                    <button 
                      @click="confirmDelete(user)" 
                      class="btn-delete" 
                      title="Sil"
                      :disabled="user.id === currentAuthUser.id"
                    >
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
              <tr v-if="filteredUsers.length === 0">
                <td colspan="7" class="empty-state">
                  <span v-if="searchQuery">
                    "{{ searchQuery }}" için kullanıcı bulunamadı.
                  </span>
                  <span v-else>
                    Henüz kullanıcı bulunmamaktadır.
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- User Details Modal (Tüm Detaylar) -->
        <div v-if="showViewModal && viewMode === 'all'" class="modal-overlay" @click.self="showViewModal = false">
          <div class="modal-content modal-large">
            <div class="modal-header">
              <h2>Kullanıcı Detayları - {{ selectedUser?.username }}</h2>
              <button @click="showViewModal = false" class="close-btn">×</button>
            </div>
            
            <div class="user-detail-content">
              <div class="user-info-card">
                <div class="user-avatar">
                  {{ selectedUser?.username?.charAt(0).toUpperCase() || 'U' }}
                </div>
                <div class="user-details">
                  <h3>{{ selectedUser?.username }}</h3>
                  <p class="user-email">{{ selectedUser?.email }}</p>
                  <span :class="['role-badge-large', selectedUser?.role.toLowerCase()]">
                    {{ selectedUser?.role }}
                  </span>
                </div>
              </div>
              
              <div class="user-stats">
                <div class="stat-item">
                  <span class="stat-label">Kullanıcı ID:</span>
                  <span class="stat-value">{{ selectedUser?.id }}</span>
                </div>
                <div class="stat-item">
                  <span class="stat-label">Toplam Makale:</span>
                  <span class="stat-value">{{ selectedUser?.articles?.length || 0 }}</span>
                </div>
                <div class="stat-item">
                  <span class="stat-label">Doğrudan Yorum:</span>
                  <span class="stat-value">{{ selectedUser?.comments?.length || 0 }}</span>
                </div>
                <div class="stat-item">
                  <span class="stat-label">Toplam Beğeni:</span>
                  <span class="stat-value">{{ userTotalLikes(selectedUser?.id) }}</span>
                </div>
              </div>
              
              <!-- User Articles Section -->
              <div v-if="selectedUser?.articles && selectedUser.articles.length > 0" class="user-sections">
                <div class="section-header">
                  <h4>Makaleler ({{ selectedUser.articles.length }})</h4>
                  <button @click="toggleArticles" class="toggle-btn">
                    {{ showArticles ? '▲ Gizle' : '▼ Göster' }}
                  </button>
                </div>
                
                <div v-if="showArticles" class="articles-list">
                  <div v-for="article in selectedUser.articles" :key="article.id" class="article-item">
                    <div class="article-header">
                      <h5>{{ article.title }}</h5>
                      <div class="article-meta">
                        <span class="article-date">{{ formatDate(article.createdDate) }}</span>
                        <span class="article-likes">❤️ {{ article.articleLikeCount || 0 }}</span>
                        <span class="article-comments-count">💬 {{ article.comments?.length || 0 }}</span>
                      </div>
                    </div>
                    
                    <!-- Article Content -->
                    <div v-if="article.content" class="article-content">
                      <p class="content-preview">{{ truncateText(article.content, 200) }}</p>
                    </div>
                    
                    <!-- Article Comments -->
                    <div v-if="article.comments && article.comments.length > 0" class="article-comments">
                      <h6>Makaledeki Yorumlar ({{ article.comments.length }}):</h6>
                      <div class="comments-list">
                        <div v-for="comment in article.comments.slice(0, 3)" :key="comment.id" class="comment-item">
                          <div class="comment-content">{{ truncateText(comment.content, 100) }}</div>
                          <div class="comment-meta">
                            <span class="comment-date">{{ formatDate(comment.createdDate) }}</span>
                          </div>
                        </div>
                        <div v-if="article.comments.length > 3" class="more-comments">
                          + {{ article.comments.length - 3 }} daha...
                        </div>
                      </div>
                    </div>
                    
                    <!-- Article Media -->
                    <div v-if="article.mediaFiles && article.mediaFiles.length > 0" class="article-media">
                      <h6>Medya Dosyaları ({{ article.mediaFiles.length }}):</h6>
                      <div class="media-thumbnails">
                        <div v-for="media in article.mediaFiles.slice(0, 3)" :key="media.id" class="media-thumb">
                          <img v-if="media.url" :src="getMediaUrl(media.url)" :alt="'Media ' + media.id" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              
              <!-- User Direct Comments Section -->
              <div v-if="selectedUser?.comments && selectedUser.comments.length > 0" class="user-sections">
                <div class="section-header">
                  <h4>Doğrudan Yorumlar ({{ selectedUser.comments.length }})</h4>
                </div>
                
                <div class="user-comments-list">
                  <div v-for="comment in selectedUser.comments.slice(0, 5)" :key="comment.id" class="user-comment-item">
                    <div class="user-comment-content">
                      <strong>Makale:</strong> {{ comment.articleTitle || 'Bilinmeyen Makale' }}
                    </div>
                    <div class="user-comment-text">{{ truncateText(comment.content, 150) }}</div>
                    <div class="user-comment-meta">
                      <span class="user-comment-date">{{ formatDate(comment.createdDate) }}</span>
                    </div>
                  </div>
                  <div v-if="selectedUser.comments.length > 5" class="more-user-comments">
                    + {{ selectedUser.comments.length - 5 }} daha yorum...
                  </div>
                </div>
              </div>
              
              <div v-if="(!selectedUser?.articles || selectedUser.articles.length === 0) && (!selectedUser?.comments || selectedUser.comments.length === 0)" class="no-data">
                <p>Bu kullanıcının henüz makalesi veya yorumu bulunmamaktadır.</p>
              </div>
            </div>
          </div>
        </div>

        <!-- User Articles Only Modal -->
        <div v-if="showViewModal && viewMode === 'articles'" class="modal-overlay" @click.self="showViewModal = false">
          <div class="modal-content modal-large">
            <div class="modal-header">
              <h2>{{ selectedUser?.username }} - Makaleler ({{ selectedUser?.articles?.length || 0 }})</h2>
              <div class="modal-header-actions">
                <button @click="viewUserDetails(selectedUser)" class="btn-secondary" title="Tüm Detayları Gör">
                  Tüm Detaylar
                </button>
                <button @click="showViewModal = false" class="close-btn">×</button>
              </div>
            </div>
            
            <div class="user-detail-content">
              <!-- User Info -->
              <div class="user-info-card">
                <div class="user-avatar">
                  {{ selectedUser?.username?.charAt(0).toUpperCase() || 'U' }}
                </div>
                <div class="user-details">
                  <h3>{{ selectedUser?.username }}</h3>
                  <p class="user-email">{{ selectedUser?.email }}</p>
                  <span :class="['role-badge-large', selectedUser?.role.toLowerCase()]">
                    {{ selectedUser?.role }}
                  </span>
                </div>
              </div>
              
              <!-- Articles Stats -->
              <div class="user-stats">
                <div class="stat-item">
                  <span class="stat-label">Toplam Makale:</span>
                  <span class="stat-value">{{ selectedUser?.articles?.length || 0 }}</span>
                </div>
                <div class="stat-item">
                  <span class="stat-label">Toplam Makale Beğenisi:</span>
                  <span class="stat-value">{{ userTotalLikes(selectedUser?.id) }}</span>
                </div>
                <div class="stat-item">
                  <span class="stat-label">Toplam Makale Yorumu:</span>
                  <span class="stat-value">{{ userArticlesCommentsCount(selectedUser?.id) }}</span>
                </div>
              </div>
              
              <!-- User Articles Section -->
              <div v-if="selectedUser?.articles && selectedUser.articles.length > 0" class="user-sections">
                <div class="section-header">
                  <h4>Tüm Makaleler</h4>
                </div>
                
                <div class="articles-list">
                  <div v-for="article in selectedUser.articles" :key="article.id" class="article-item">
                    <div class="article-header">
                      <h5>{{ article.title }}</h5>
                      <div class="article-meta">
                        <span class="article-date">{{ formatDate(article.createdDate) }}</span>
                        <span class="article-likes">❤️ {{ article.articleLikeCount || 0 }}</span>
                        <span class="article-comments-count">💬 {{ article.comments?.length || 0 }}</span>
                      </div>
                    </div>
                    
                    <!-- Article Content -->
                    <div v-if="article.content" class="article-content">
                      <p class="content-preview">{{ truncateText(article.content, 300) }}</p>
                    </div>
                    
                    <!-- Article Comments -->
                    <div v-if="article.comments && article.comments.length > 0" class="article-comments">
                      <h6>Bu Makaledeki Yorumlar ({{ article.comments.length }}):</h6>
                      <div class="comments-list">
                        <div v-for="comment in article.comments" :key="comment.id" class="comment-item">
                          <div class="comment-content">{{ comment.content }}</div>
                          <div class="comment-meta">
                            <span class="comment-date">{{ formatDate(comment.createdDate) }}</span>
                          </div>
                        </div>
                      </div>
                    </div>
                    
                    <!-- Article Media -->
                    <div v-if="article.mediaFiles && article.mediaFiles.length > 0" class="article-media">
                      <h6>Medya Dosyaları ({{ article.mediaFiles.length }}):</h6>
                      <div class="media-thumbnails">
                        <div v-for="media in article.mediaFiles" :key="media.id" class="media-thumb">
                          <img v-if="media.url" :src="getMediaUrl(media.url)" :alt="'Media ' + media.id" />
                        </div>
                      </div>
                    </div>
                    
                    <div v-if="!article.comments || article.comments.length === 0" class="no-comments">
                      <p>Bu makalede henüz yorum bulunmamaktadır.</p>
                    </div>
                  </div>
                </div>
              </div>
              
              <div v-else class="no-data">
                <p>Bu kullanıcının henüz makalesi bulunmamaktadır.</p>
              </div>
            </div>
          </div>
        </div>

        <!-- User Comments Only Modal -->
        <div v-if="showViewModal && viewMode === 'comments'" class="modal-overlay" @click.self="showViewModal = false">
          <div class="modal-content modal-large">
            <div class="modal-header">
              <h2>{{ selectedUser?.username }} - Yorumlar ({{ selectedUser?.comments?.length || 0 }})</h2>
              <div class="modal-header-actions">
                <button @click="viewUserDetails(selectedUser)" class="btn-secondary" title="Tüm Detayları Gör">
                  Tüm Detaylar
                </button>
                <button @click="showViewModal = false" class="close-btn">×</button>
              </div>
            </div>
            
            <div class="user-detail-content">
              <!-- User Info -->
              <div class="user-info-card">
                <div class="user-avatar">
                  {{ selectedUser?.username?.charAt(0).toUpperCase() || 'U' }}
                </div>
                <div class="user-details">
                  <h3>{{ selectedUser?.username }}</h3>
                  <p class="user-email">{{ selectedUser?.email }}</p>
                  <span :class="['role-badge-large', selectedUser?.role.toLowerCase()]">
                    {{ selectedUser?.role }}
                  </span>
                </div>
              </div>
              
              <!-- Comments Stats -->
              <div class="user-stats">
                <div class="stat-item">
                  <span class="stat-label">Toplam Yorum:</span>
                  <span class="stat-value">{{ selectedUser?.comments?.length || 0 }}</span>
                </div>
              </div>
              
              <!-- User Comments Section -->
              <div v-if="selectedUser?.comments && selectedUser.comments.length > 0" class="user-sections">
                <div class="section-header">
                  <h4>Tüm Yorumlar</h4>
                </div>
                
                <div class="user-comments-list-full">
                  <div v-for="comment in selectedUser.comments" :key="comment.id" class="user-comment-item-full">
                    <div class="comment-header">
                      <div class="comment-article">
                        <strong>Makale:</strong> {{ comment.articleTitle || 'Bilinmeyen Makale' }}
                      </div>
                      <div class="comment-date">
                        {{ formatDate(comment.createdDate) }}
                      </div>
                    </div>
                    <div class="comment-content-full">
                      {{ comment.content }}
                    </div>
                    <div class="comment-actions">
                      <span class="comment-id">Yorum ID: {{ comment.id || 'Geçici' }}</span>
                    </div>
                  </div>
                </div>
              </div>
              
              <div v-else class="no-data">
                <p>Bu kullanıcının henüz yorumu bulunmamaktadır.</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Edit User Modal -->
        <div v-if="showEditModal" class="modal-overlay" @click.self="showEditModal = false">
          <div class="modal-content modal-small">
            <div class="modal-header">
              <h2>Kullanıcı Düzenle</h2>
              <button @click="showEditModal = false" class="close-btn">×</button>
            </div>
            
            <form @submit.prevent="handleUpdate" class="user-form">
              <div class="form-group">
                <label for="username">Kullanıcı Adı</label>
                <input
                  type="text"
                  id="username"
                  v-model="editForm.username"
                  required
                  placeholder="Kullanıcı adı"
                />
              </div>

              <div class="form-group">
                <label for="email">E-posta</label>
                <input
                  type="email"
                  id="email"
                  v-model="editForm.email"
                  required
                  placeholder="E-posta adresi"
                />
              </div>

              <div class="modal-actions">
                <button type="button" @click="showEditModal = false" class="btn-secondary">
                  İptal
                </button>
                <button type="submit" class="btn-primary" :disabled="isLoading">
                  {{ isLoading ? 'Güncelleniyor...' : 'Güncelle' }}
                </button>
              </div>
            </form>
          </div>
        </div>

        <!-- Delete Confirmation Modal -->
        <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
          <div class="modal-content modal-small">
            <div class="modal-header">
              <h2>Kullanıcı Sil</h2>
              <button @click="showDeleteModal = false" class="close-btn">×</button>
            </div>
            
            <div class="modal-body">
              <p>
                <strong>{{ userToDelete?.username }}</strong> kullanıcısını silmek istediğinizden emin misiniz?
              </p>
              <p class="warning-text" v-if="userToDelete?.id === currentAuthUser.id">
                ⚠️ Kendi hesabınızı silemezsiniz!
              </p>
              <p class="warning-text" v-else-if="(userToDelete?.articles?.length || 0) > 0">
                ⚠️ Bu kullanıcının {{ userToDelete?.articles?.length || 0 }} makalesi ve 
                {{ userToDelete?.comments?.length || 0 }} yorumu silinecektir!
              </p>
              <p class="warning-text" v-else>
                Bu işlem geri alınamaz!
              </p>
            </div>

            <div class="modal-actions">
              <button @click="showDeleteModal = false" class="btn-secondary">
                İptal
              </button>
              <button 
                @click="handleDelete" 
                class="btn-danger" 
                :disabled="isLoading || userToDelete?.id === currentAuthUser.id"
              >
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
  name: 'UserManagement',
  data() {
    return {
      showViewModal: false,
      showEditModal: false,
      showDeleteModal: false,
      selectedUser: null,
      userToDelete: null,
      editForm: {
        username: '',
        email: ''
      },
      searchQuery: '',
      filterRole: 'all',
      showArticles: false,
      viewMode: 'all', // 'all', 'articles', 'comments'
      toast: {
        show: false,
        message: '',
        type: 'success'
      }
    };
  },
  computed: {
    ...mapGetters('auth', ['currentUser']),
    ...mapGetters('users', [
      'allUsers', 
      'isLoading', 
      'getError', 
      'userCount', 
      'adminCount',
      'userTotalLikes',
      'userArticlesCommentsCount'
    ]),
    
    currentAuthUser() {
      return this.currentUser;
    },
    
    filteredUsers() {
      let users = this.allUsers;
      
      // Rol filtreleme
      if (this.filterRole !== 'all') {
        users = users.filter(user => user.role === this.filterRole);
      }
      
      // Arama filtreleme
      if (this.searchQuery) {
        const query = this.searchQuery.toLowerCase();
        users = users.filter(user => 
          user.username.toLowerCase().includes(query) ||
          user.email.toLowerCase().includes(query)
        );
      }
      
      return users;
    }
  },
  methods: {
    ...mapActions('auth', ['logout']),
    ...mapActions('users', ['fetchUsers', 'updateUser', 'deleteUser']),

    handleLogout() {
      this.logout();
      this.$router.push('/login');
    },

    goBackToAdmin() {
      this.$router.push('/admin');
    },

    viewUserDetails(user) {
      this.selectedUser = user;
      this.showArticles = false;
      this.viewMode = 'all';
      this.showViewModal = true;
    },

    toggleArticles() {
      this.showArticles = !this.showArticles;
    },

    viewUserArticles(user) {
      this.selectedUser = user;
      this.showArticles = true;
      this.viewMode = 'articles';
      this.showViewModal = true;
    },

    viewUserComments(user) {
      this.selectedUser = user;
      this.showArticles = false;
      this.viewMode = 'comments';
      this.showViewModal = true;
    },

    openEditModal(user) {
      this.selectedUser = user;
      this.editForm = {
        username: user.username,
        email: user.email
      };
      this.showEditModal = true;
    },

    async handleUpdate() {
      try {
        const result = await this.updateUser({
          userId: this.selectedUser.id,
          userData: {
            Id: this.selectedUser.id, // Backend Id bekliyor
            UserName: this.editForm.username,
            UserEmail: this.editForm.email
          }
        });

        if (result.success) {
          this.showToast(result.message, 'success');
          this.showEditModal = false;
          await this.fetchUsers(); // Listeyi yenile
        } else {
          this.showToast(result.message || 'Güncelleme başarısız oldu.', 'error');
        }
      } catch (error) {
        console.error('Update error:', error);
        this.showToast('Bir hata oluştu.', 'error');
      }
    },

    confirmDelete(user) {
      if (user.id === this.currentAuthUser.id) {
        this.showToast('Kendi hesabınızı silemezsiniz!', 'warning');
        return;
      }
      
      this.userToDelete = user;
      this.showDeleteModal = true;
    },

    async handleDelete() {
      try {
        const result = await this.deleteUser(this.userToDelete.id);
        
        if (result.success) {
          this.showToast(result.message, 'success');
          this.showDeleteModal = false;
          this.userToDelete = null;
          await this.fetchUsers(); // Listeyi yenile
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
      if (!url) return '';
      // URL zaten tam ise değiştirme
      if (url.startsWith('http')) return url;
      return `https://localhost:7239${url}`;
    },

    truncateText(text, length) {
      if (!text) return '';
      return text.length > length ? text.substring(0, length) + '...' : text;
    }
  },
  mounted() {
    // Sayfa yüklendiğinde kullanıcıları çek
    this.fetchUsers();
    
    // Admin yetkisi kontrolü
    if (this.currentUser.role !== 'Admin') {
      this.$router.push('/');
    }
  }
};
</script>

<style scoped>
/* Admin Layout Styles */
.admin-layout {
  min-height: 100vh;
  background: #f8f9fa;
}

/* Admin Navbar Styles */
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
  font-size: 1.5rem;
  font-weight: 600;
}

.back-btn {
  background: rgba(255,255,255,0.1);
  color: white;
  border: 1px solid rgba(255,255,255,0.2);
  padding: 0.5rem 1rem;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s;
  font-size: 0.9rem;
  font-weight: 500;
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
  font-size: 0.9rem;
  transition: background 0.3s;
}

.admin-logout-btn:hover {
  background: #c0392b;
}

/* Main Content Styles */
.admin-main {
  padding: 2rem;
  max-width: 1600px;
  margin: 0 auto;
}

.user-management-container {
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

/* Header Section Styles */
.header-section {
  margin-bottom: 2rem;
}

.header-section h1 {
  color: #2c3e50;
  margin: 0 0 1.5rem 0;
  font-size: 2rem;
  font-weight: 700;
}

.stats-container {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.stat-card {
  flex: 1;
  min-width: 200px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  border-radius: 8px;
  text-align: center;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.stat-card h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1rem;
  opacity: 0.9;
  font-weight: 500;
}

.stat-number {
  font-size: 2.5rem;
  font-weight: bold;
  margin: 0;
}

.admin-count {
  color: #f39c12;
}

.user-count {
  color: #2ecc71;
}

/* Loading State Styles */
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

.loading-state p {
  color: #6c757d;
  font-size: 1rem;
}

/* Error State Styles */
.error-message {
  background: #fee;
  color: #c33;
  padding: 1rem;
  border-radius: 5px;
  text-align: center;
  font-weight: 500;
  border: 1px solid #f5c6cb;
}

/* Table Controls Styles */
.table-controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.search-box {
  position: relative;
  flex: 1;
  max-width: 300px;
}

.search-input {
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 2.5rem;
  border: 1px solid #ced4da;
  border-radius: 25px;
  font-size: 0.9rem;
  transition: all 0.3s;
  background: #f8f9fa;
}

.search-input:focus {
  outline: none;
  border-color: #667eea;
  background: white;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.search-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6c757d;
  font-size: 1rem;
}

.filter-buttons {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.filter-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #dee2e6;
  background: white;
  border-radius: 20px;
  cursor: pointer;
  transition: all 0.3s;
  font-weight: 600;
  font-size: 0.85rem;
  color: #495057;
}

.filter-btn.active {
  background: #667eea;
  color: white;
  border-color: #667eea;
}

.filter-btn:hover:not(.active) {
  background: #f8f9fa;
}

.admin-filter.active {
  background: #f39c12;
  border-color: #f39c12;
}

.user-filter.active {
  background: #2ecc71;
  border-color: #2ecc71;
}

/* Table Container Styles */
.table-container {
  overflow-x: auto;
  border: 1px solid #dee2e6;
  border-radius: 8px;
  background: white;
}

.users-table {
  width: 100%;
  border-collapse: collapse;
  margin: 0;
}

.users-table thead {
  background: #f8f9fa;
  border-bottom: 2px solid #dee2e6;
}

.users-table th,
.users-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #dee2e6;
  font-size: 0.9rem;
}

.users-table th {
  font-weight: 600;
  color: #495057;
  white-space: nowrap;
}

.users-table tbody tr {
  transition: background-color 0.2s;
}

.users-table tbody tr:hover {
  background: #f8f9fa;
}

.users-table tbody tr:last-child td {
  border-bottom: none;
}

/* User Info Cell Styles */
.user-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.username {
  font-weight: 500;
  color: #2c3e50;
  font-size: 0.95rem;
}

.email {
  color: #6c757d;
  font-family: monospace;
  font-size: 0.85rem;
}

/* Role Badge Styles */
.role-badge {
  display: inline-block;
  padding: 0.25rem 0.75rem;
  border-radius: 15px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.role-badge.admin {
  background: #fff3cd;
  color: #856404;
  border: 1px solid #ffeaa7;
}

.role-badge.user {
  background: #d1ecf1;
  color: #0c5460;
  border: 1px solid #bee5eb;
}

/* Article and Comment Info Styles */
.article-info,
.comment-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.article-count,
.comment-count {
  font-weight: 600;
  color: #2c3e50;
  min-width: 20px;
  text-align: center;
  font-size: 0.95rem;
}

.article-detail,
.comment-detail {
  cursor: pointer;
  font-size: 1.1rem;
  transition: transform 0.3s;
  display: inline-block;
}

.article-detail:hover,
.comment-detail:hover {
  transform: scale(1.2);
}

/* Action Buttons Styles */
.action-buttons {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-start;
}

.btn-view,
.btn-edit,
.btn-role,
.btn-delete {
  padding: 0.5rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.3s;
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-view {
  background: #3498db;
  color: white;
}

.btn-view:hover {
  background: #2980b9;
  transform: scale(1.1);
}

.btn-edit {
  background: #f39c12;
  color: white;
}

.btn-edit:hover {
  background: #e67e22;
  transform: scale(1.1);
}

.btn-role {
  background: #9b59b6;
  color: white;
}

.btn-role:hover {
  background: #8e44ad;
  transform: scale(1.1);
}

.btn-delete {
  background: #e74c3c;
  color: white;
}

.btn-delete:hover:not(:disabled) {
  background: #c0392b;
  transform: scale(1.1);
}

.btn-delete:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none !important;
}

/* Empty State Styles */
.empty-state {
  text-align: center;
  color: #6c757d;
  padding: 3rem !important;
  font-style: italic;
  font-size: 1rem;
}

/* Modal Overlay Styles */
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

/* Modal Content Styles */
.modal-content {
  background: white;
  border-radius: 10px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
  margin: auto;
}

.modal-small {
  max-width: 500px;
}

.modal-large {
  max-width: 900px;
}

.modal-medium {
  max-width: 600px;
}

/* Modal Header Styles */
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
  font-size: 1.5rem;
  font-weight: 600;
}

.close-btn {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: #6c757d;
  line-height: 1;
  transition: color 0.3s;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 5px;
}

.close-btn:hover {
  color: #2c3e50;
  background: #f8f9fa;
}

/* Modal Header Actions Styles */
.modal-header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

/* User Detail Content Styles */
.user-detail-content {
  padding: 1.5rem;
}

/* User Info Card Styles */
.user-info-card {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid #e9ecef;
}

.user-avatar {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: bold;
  flex-shrink: 0;
}

.user-details h3 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.5rem;
  font-weight: 600;
}

.user-email {
  margin: 0 0 1rem 0;
  color: #6c757d;
  font-size: 1rem;
}

.role-badge-large {
  display: inline-block;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.85rem;
}

.role-badge-large.admin {
  background: #fff3cd;
  color: #856404;
  border: 1px solid #ffeaa7;
}

.role-badge-large.user {
  background: #d1ecf1;
  color: #0c5460;
  border: 1px solid #bee5eb;
}

/* User Stats Styles */
.user-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.stat-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.stat-label {
  color: #6c757d;
  font-size: 0.9rem;
  font-weight: 500;
}

.stat-value {
  color: #2c3e50;
  font-weight: 600;
  font-size: 1.1rem;
}

/* User Sections Styles */
.user-sections {
  margin-top: 2rem;
  padding-top: 1rem;
  border-top: 1px solid #e9ecef;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.section-header h4 {
  margin: 0;
  color: #2c3e50;
  font-size: 1.2rem;
  font-weight: 600;
}

.toggle-btn {
  background: #667eea;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 5px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background 0.3s;
  font-weight: 500;
}

.toggle-btn:hover {
  background: #5a6fd8;
}

/* Articles List Styles */
.articles-list {
  max-height: 500px;
  overflow-y: auto;
  margin-top: 1rem;
}

.article-item {
  background: #f8f9fa;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
  border-left: 4px solid #667eea;
  border: 1px solid #e9ecef;
}

.article-header {
  margin-bottom: 1rem;
}

.article-header h5 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.1rem;
  font-weight: 600;
}

.article-meta {
  display: flex;
  gap: 1rem;
  font-size: 0.875rem;
  color: #6c757d;
  flex-wrap: wrap;
  align-items: center;
}

.article-date {
  font-size: 0.85rem;
}

.article-likes,
.article-comments-count {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  font-size: 0.85rem;
}

/* Article Content Styles */
.article-content {
  margin: 1rem 0;
  padding: 0.5rem;
  background: white;
  border-radius: 4px;
  border-left: 3px solid #e9ecef;
}

.content-preview {
  margin: 0;
  color: #495057;
  font-size: 0.9rem;
  line-height: 1.5;
}

/* Article Comments Styles */
.article-comments {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px dashed #dee2e6;
}

.article-comments h6 {
  margin: 0 0 0.5rem 0;
  color: #495057;
  font-size: 0.9rem;
  font-weight: 600;
}

.comments-list {
  background: white;
  border-radius: 6px;
  padding: 0.75rem;
  border: 1px solid #e9ecef;
}

.comment-item {
  padding: 0.5rem;
  margin-bottom: 0.5rem;
  background: #f8f9fa;
  border-radius: 4px;
  border: 1px solid #dee2e6;
}

.comment-content {
  color: #495057;
  font-size: 0.9rem;
  margin-bottom: 0.25rem;
  line-height: 1.4;
}

.comment-meta {
  font-size: 0.8rem;
  color: #6c757d;
  text-align: right;
}

.more-comments {
  text-align: center;
  padding: 0.5rem;
  color: #6c757d;
  font-style: italic;
  font-size: 0.875rem;
}

.no-comments {
  text-align: center;
  padding: 1rem;
  color: #6c757d;
  font-style: italic;
  background: #f8f9fa;
  border-radius: 4px;
  margin-top: 1rem;
  font-size: 0.9rem;
}

/* Article Media Styles */
.article-media {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px dashed #dee2e6;
}

.article-media h6 {
  margin: 0 0 0.5rem 0;
  color: #495057;
  font-size: 0.9rem;
  font-weight: 600;
}

.media-thumbnails {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.media-thumb {
  width: 80px;
  height: 80px;
  border-radius: 4px;
  overflow: hidden;
  border: 1px solid #dee2e6;
  background: #f8f9fa;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.media-thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* User Comments List Styles */
.user-comments-list {
  max-height: 300px;
  overflow-y: auto;
  margin-top: 1rem;
}

.user-comment-item {
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 6px;
  padding: 1rem;
  margin-bottom: 0.75rem;
}

.user-comment-content {
  color: #2c3e50;
  font-weight: 500;
  margin-bottom: 0.5rem;
  font-size: 0.95rem;
}

.user-comment-text {
  color: #495057;
  font-size: 0.9rem;
  line-height: 1.5;
  margin-bottom: 0.5rem;
}

.user-comment-meta {
  font-size: 0.8rem;
  color: #6c757d;
  text-align: right;
}

.more-user-comments {
  text-align: center;
  padding: 1rem;
  color: #6c757d;
  font-style: italic;
  background: #f8f9fa;
  border-radius: 6px;
  margin-top: 0.5rem;
  font-size: 0.9rem;
}

/* Full Comments List Styles */
.user-comments-list-full {
  max-height: 500px;
  overflow-y: auto;
  margin-top: 1rem;
}

.user-comment-item-full {
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  padding: 1.5rem;
  margin-bottom: 1rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
}

.comment-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e9ecef;
}

.comment-article {
  color: #2c3e50;
  font-weight: 600;
  font-size: 1rem;
}

.comment-date {
  color: #6c757d;
  font-size: 0.875rem;
}

.comment-content-full {
  color: #495057;
  font-size: 1rem;
  line-height: 1.6;
  margin-bottom: 1rem;
  white-space: pre-wrap;
}

.comment-actions {
  display: flex;
  justify-content: flex-end;
  font-size: 0.875rem;
  color: #6c757d;
}

.comment-id {
  background: #f8f9fa;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-family: monospace;
  font-size: 0.8rem;
}

/* No Data Styles */
.no-data {
  text-align: center;
  padding: 3rem;
  color: #6c757d;
  font-style: italic;
  background: #f8f9fa;
  border-radius: 8px;
  margin-top: 1rem;
  font-size: 1rem;
}

/* User Form Styles */
.user-form {
  padding: 1.5rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #495057;
  font-weight: 600;
  font-size: 0.9rem;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ced4da;
  border-radius: 5px;
  font-size: 1rem;
  transition: border-color 0.3s;
  box-sizing: border-box;
  background: white;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-select {
  background-color: white;
  cursor: pointer;
}

/* Modal Actions Styles */
.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding: 1.5rem;
  border-top: 1px solid #dee2e6;
  background: #f8f9fa;
  border-radius: 0 0 10px 10px;
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
  font-size: 0.9rem;
  min-width: 100px;
}

.btn-secondary:hover {
  background: #5a6268;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s;
  font-size: 0.9rem;
  min-width: 100px;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none !important;
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
  font-size: 0.9rem;
  min-width: 100px;
}

.btn-danger:hover:not(:disabled) {
  background: #c0392b;
}

.btn-danger:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Modal Body Styles */
.modal-body {
  padding: 1.5rem;
}

.warning-text {
  color: #e74c3c;
  font-weight: 600;
  margin-top: 1rem;
  font-size: 0.95rem;
}

.modal-body p {
  color: #495057;
  line-height: 1.5;
  margin-bottom: 0.5rem;
  font-size: 0.95rem;
}

/* Toast Notification Styles */
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
  font-size: 0.9rem;
  max-width: 400px;
}

.toast.success {
  background: #27ae60;
}

.toast.error {
  background: #e74c3c;
}

.toast.warning {
  background: #f39c12;
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

/* Responsive Styles */
@media (max-width: 768px) {
  .admin-navbar {
    padding: 1rem;
  }
  
  .admin-nav-content {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }
  
  .admin-nav-title {
    justify-content: space-between;
    width: 100%;
  }
  
  .admin-nav-actions {
    width: 100%;
  }
  
  .admin-logout-btn {
    width: 100%;
  }
  
  .admin-main {
    padding: 1rem;
  }
  
  .user-management-container {
    padding: 1rem;
  }
  
  .header-section h1 {
    font-size: 1.5rem;
    margin-bottom: 1rem;
  }
  
  .stat-card {
    min-width: 100%;
  }
  
  .table-controls {
    flex-direction: column;
    align-items: stretch;
  }
  
  .search-box {
    max-width: 100%;
  }
  
  .filter-buttons {
    justify-content: center;
    width: 100%;
  }
  
  .users-table {
    font-size: 0.75rem;
  }
  
  .users-table th,
  .users-table td {
    padding: 0.75rem 0.5rem;
  }
  
  .action-buttons {
    flex-direction: column;
    gap: 0.25rem;
  }
  
  .btn-view,
  .btn-edit,
  .btn-role,
  .btn-delete {
    width: 100%;
    text-align: center;
    height: 32px;
    width: 32px;
    font-size: 0.9rem;
  }
  
  .modal-actions {
    flex-direction: column;
  }
  
  .btn-secondary,
  .btn-primary,
  .btn-danger {
    width: 100%;
    text-align: center;
  }
  
  .toast {
    right: 1rem;
    left: 1rem;
    bottom: 1rem;
    max-width: calc(100% - 2rem);
  }
  
  .modal-content {
    margin: 0.5rem;
  }
  
  .modal-header {
    padding: 1rem;
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }
  
  .modal-header h2 {
    font-size: 1.3rem;
  }
  
  .modal-header-actions {
    width: 100%;
    justify-content: space-between;
  }
  
  .user-info-card {
    flex-direction: column;
    text-align: center;
    gap: 1rem;
  }
  
  .user-avatar {
    width: 60px;
    height: 60px;
    font-size: 1.5rem;
  }
  
  .user-details h3 {
    font-size: 1.3rem;
  }
  
  .user-stats {
    grid-template-columns: 1fr;
  }
  
  .article-item {
    padding: 0.75rem;
  }
  
  .article-meta {
    flex-direction: column;
    gap: 0.25rem;
    align-items: flex-start;
  }
  
  .media-thumb {
    width: 60px;
    height: 60px;
  }
  
  .user-comment-item,
  .user-comment-item-full {
    padding: 0.75rem;
  }
  
  .comment-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.5rem;
  }
  
  .comment-content-full {
    font-size: 0.9rem;
  }
  
  .user-form {
    padding: 1rem;
  }
  
  .form-group input,
  .form-group select {
    padding: 0.6rem;
  }
}

@media (max-width: 480px) {
  .admin-nav-title h2 {
    font-size: 1.2rem;
  }
  
  .back-btn {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .admin-logout-btn {
    padding: 0.4rem 1rem;
    font-size: 0.8rem;
  }
  
  .header-section h1 {
    font-size: 1.3rem;
  }
  
  .stat-number {
    font-size: 2rem;
  }
  
  .stat-card h3 {
    font-size: 0.9rem;
  }
  
  .filter-btn {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .search-input {
    padding: 0.6rem 0.8rem 0.6rem 2.2rem;
    font-size: 0.8rem;
  }
  
  .search-icon {
    left: 0.8rem;
    font-size: 0.9rem;
  }
  
  .username {
    font-size: 0.85rem;
  }
  
  .email {
    font-size: 0.8rem;
  }
  
  .role-badge {
    padding: 0.2rem 0.6rem;
    font-size: 0.7rem;
  }
  
  .article-count,
  .comment-count {
    font-size: 0.85rem;
  }
  
  .article-detail,
  .comment-detail {
    font-size: 1rem;
  }
  
  .btn-view,
  .btn-edit,
  .btn-role,
  .btn-delete {
    height: 30px;
    width: 30px;
    font-size: 0.85rem;
  }
  
  .modal-header h2 {
    font-size: 1.2rem;
  }
  
  .close-btn {
    font-size: 1.8rem;
    width: 36px;
    height: 36px;
  }
  
  .role-badge-large {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .user-details h3 {
    font-size: 1.2rem;
  }
  
  .user-email {
    font-size: 0.9rem;
  }
  
  .stat-label {
    font-size: 0.85rem;
  }
  
  .stat-value {
    font-size: 1rem;
  }
  
  .section-header h4 {
    font-size: 1.1rem;
  }
  
  .toggle-btn {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
  
  .article-header h5 {
    font-size: 1rem;
  }
  
  .content-preview {
    font-size: 0.85rem;
  }
  
  .user-comment-content {
    font-size: 0.9rem;
  }
  
  .user-comment-text {
    font-size: 0.85rem;
  }
  
  .comment-article {
    font-size: 0.9rem;
  }
  
  .comment-date {
    font-size: 0.8rem;
  }
  
  .comment-content-full {
    font-size: 0.85rem;
  }
  
  .toast {
    padding: 0.8rem 1.2rem;
    font-size: 0.85rem;
  }
}
</style>