<template>
  <div class="admin-layout">
    <!-- Admin Navbar -->
    <header class="admin-navbar">
      <div class="admin-nav-content">
        <div class="admin-nav-title">
          <button @click="goBackToAdmin" class="back-btn">← Geri</button>
          <h2>Proje Yönetimi</h2>
        </div>
        <div class="admin-nav-actions">
          <button @click="handleLogout" class="admin-logout-btn">
            Çıkış Yap
          </button>
        </div>
      </div>
    </header>

    <main class="admin-main">
      <div class="project-management-container">
        <!-- Header with Add Button -->
        <div class="header-section">
          <h1>Projeler</h1>
          <button @click="openCreateModal" class="btn-primary">
            <span class="icon">+</span> Yeni Proje Ekle
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

        <!-- Projects Table -->
        <div v-if="!isLoading && !getError" class="table-container">
          <table class="projects-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Başlık</th>
                <th>Teknolojiler</th>
                <th>Kategori</th>
                <th>GitHub</th>
                <th>Demo</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="project in allProjects" :key="project.id">
                <td>{{ project.id }}</td>
                <td>{{ project.title }}</td>
                <td>{{ project.technologies }}</td>
                <td>{{ project.categoryId }}</td>
                <td>
                  <a v-if="project.githubUrl" :href="project.githubUrl" target="_blank" class="link-icon">
                    🔗
                  </a>
                  <span v-else>-</span>
                </td>
                <td>
                  <a v-if="project.liveDemoUrl" :href="project.liveDemoUrl" target="_blank" class="link-icon">
                    🔗
                  </a>
                  <span v-else>-</span>
                </td>
                <td>
                  <div class="action-buttons">
                    <button @click="openEditModal(project)" class="btn-edit" title="Düzenle">
                      ✏️
                    </button>
                    <button @click="confirmDelete(project)" class="btn-delete" title="Sil">
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
              <tr v-if="allProjects.length === 0">
                <td colspan="7" class="empty-state">
                  Henüz proje bulunmamaktadır.
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Create/Edit Modal -->
        <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
          <div class="modal-content">
            <div class="modal-header">
              <h2>{{ isEditMode ? 'Proje Düzenle' : 'Yeni Proje Ekle' }}</h2>
              <button @click="closeModal" class="close-btn">×</button>
            </div>
            
            <form @submit.prevent="handleSubmit" class="project-form">
              <div class="form-group">
                <label for="title">Başlık *</label>
                <input
                  type="text"
                  id="title"
                  v-model="formData.title"
                  required
                  placeholder="Proje başlığı"
                />
              </div>

              <div class="form-group">
                <label for="description">Açıklama *</label>
                <textarea
                  id="description"
                  v-model="formData.description"
                  required
                  rows="4"
                  placeholder="Proje açıklaması"
                ></textarea>
              </div>

              <div class="form-group">
                <label for="technologies">Teknolojiler *</label>
                <input
                  type="text"
                  id="technologies"
                  v-model="formData.technologies"
                  required
                  placeholder="React, Node.js, MongoDB"
                />
              </div>

              <div class="form-group" v-if="!isEditMode">
                <label for="categoryId">Kategori *</label>
                <select
                  id="categoryId"
                  v-model.number="formData.categoryId"
                  required
                  class="form-select"
                >
                  <option value="" disabled>Kategori seçin</option>
                  <option 
                    v-for="category in categoriesForSelect" 
                    :key="category.value" 
                    :value="category.value"
                  >
                    {{ category.label }}
                  </option>
                </select>
              </div>

              <div class="form-group">
                <label for="githubUrl">GitHub URL</label>
                <input
                  type="url"
                  id="githubUrl"
                  v-model="formData.githubUrl"
                  placeholder="https://github.com/..."
                />
              </div>

              <div class="form-group">
                <label for="liveDemoUrl">Demo URL</label>
                <input
                  type="url"
                  id="liveDemoUrl"
                  v-model="formData.liveDemoUrl"
                  placeholder="https://demo.example.com"
                />
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

        <!-- Delete Confirmation Modal -->
        <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
          <div class="modal-content modal-small">
            <div class="modal-header">
              <h2>Proje Sil</h2>
              <button @click="showDeleteModal = false" class="close-btn">×</button>
            </div>
            
            <div class="modal-body">
              <p>
                <strong>{{ projectToDelete?.title }}</strong> projesini silmek istediğinizden emin misiniz?
              </p>
              <p class="warning-text">Bu işlem geri alınamaz!</p>
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
  name: 'ProjectManagement',
  data() {
    return {
      showModal: false,
      showDeleteModal: false,
      isEditMode: false,
      projectToDelete: null,
      formData: {
        title: '',
        description: '',
        technologies: '',
        githubUrl: '',
        liveDemoUrl: '',
        categoryId: null
      },
      currentProjectId: null,
      toast: {
        show: false,
        message: '',
        type: 'success'
      }
    };
  },
  computed: {
    ...mapGetters('auth', ['currentUser']),
    ...mapGetters('projects', ['allProjects', 'isLoading', 'getError']),
    ...mapGetters('categories', { categories: 'allCategories', categoriesForSelect: 'categoriesForSelect' })
  },
  methods: {
    ...mapActions('auth', ['logout']),
    ...mapActions('projects', ['fetchProjects', 'createProject', 'updateProject', 'deleteProject']),
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

    openEditModal(project) {
      this.isEditMode = true;
      this.currentProjectId = project.id;
      this.formData = {
        title: project.title,
        description: project.description,
        technologies: project.technologies,
        githubUrl: project.githubUrl || '',
        liveDemoUrl: project.liveDemoUrl || '',
        categoryId: project.categoryId
      };
      this.showModal = true;
    },

    closeModal() {
      this.showModal = false;
      this.resetForm();
    },

    resetForm() {
      this.formData = {
        title: '',
        description: '',
        technologies: '',
        githubUrl: '',
        liveDemoUrl: '',
        categoryId: null
      };
      this.currentProjectId = null;
    },

    async handleSubmit() {
      try {
        let result;
        
        if (this.isEditMode) {
          // Update işlemi - categoryId gönderme
          const updateData = {
            title: this.formData.title,
            description: this.formData.description,
            technologies: this.formData.technologies,
            githubUrl: this.formData.githubUrl,
            liveDemoUrl: this.formData.liveDemoUrl
          };
          
          result = await this.updateProject({
            projectId: this.currentProjectId,
            projectData: updateData
          });
        } else {
          // Create işlemi - tüm veriler
          result = await this.createProject(this.formData);
        }

        if (result.success) {
          this.showToast(result.message, 'success');
          this.closeModal();
          await this.fetchProjects();
        } else {
          this.showToast(result.message || 'İşlem başarısız oldu.', 'error');
        }
      } catch (error) {
        console.error('Form submit error:', error);
        this.showToast('Bir hata oluştu.', 'error');
      }
    },

    confirmDelete(project) {
      this.projectToDelete = project;
      this.showDeleteModal = true;
    },

    async handleDelete() {
      try {
        const result = await this.deleteProject(this.projectToDelete.id);
        
        if (result.success) {
          this.showToast(result.message, 'success');
          this.showDeleteModal = false;
          this.projectToDelete = null;
          await this.fetchProjects();
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
    }
  },
  mounted() {
    // Sayfa yüklendiğinde projeleri ve kategorileri çek
    this.fetchProjects();
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
  max-width: 1400px;
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

.project-management-container {
  max-width: 1400px;
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

.projects-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

.projects-table thead {
  background: #f8f9fa;
}

.projects-table th,
.projects-table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #dee2e6;
}

.projects-table th {
  font-weight: 600;
  color: #495057;
}

.projects-table tbody tr:hover {
  background: #f8f9fa;
}

.link-icon {
  font-size: 1.2rem;
  text-decoration: none;
  cursor: pointer;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
}

.btn-edit,
.btn-delete {
  padding: 0.5rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.3s;
}

.btn-edit {
  background: #3498db;
}

.btn-edit:hover {
  background: #2980b9;
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
}

.modal-content {
  background: white;
  border-radius: 10px;
  width: 100%;
  max-width: 600px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
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

.project-form {
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
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-select {
  background-color: white;
  cursor: pointer;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding: 1.5rem;
  border-top: 1px solid #dee2e6;
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

@media (max-width: 768px) {
  .admin-nav-content {
    flex-direction: column;
    gap: 1rem;
  }

  .admin-main {
    padding: 1rem;
  }

  .project-management-container {
    padding: 1rem;
  }

  .header-section {
    flex-direction: column;
    align-items: stretch;
  }

  .projects-table {
    font-size: 0.875rem;
  }

  .projects-table th,
  .projects-table td {
    padding: 0.5rem;
  }

  .modal-actions {
    flex-direction: column;
  }

  .toast {
    right: 1rem;
    left: 1rem;
    bottom: 1rem;
  }
}
</style>