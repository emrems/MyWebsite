<template>
  <div class="main-container">
    <div class="content-wrapper">
      
      <header class="page-header">
        <h1 class="page-title">Projelerim</h1>
        <p class="page-subtitle">
          Geliştirdiğim uygulamalar, açık kaynak kodlar ve deneyimler.
        </p>
        
        <div class="filter-bar">
          <div class="category-tabs">
            <button 
              v-for="cat in uniqueCategories" 
              :key="cat"
              :class="['tab-btn', { active: selectedCategory === cat }]"
              @click="selectedCategory = cat"
            >
              {{ cat }}
            </button>
          </div>

          <div class="search-box">
            <i class="fas fa-search"></i>
            <input 
              v-model="searchQuery" 
              type="text" 
              placeholder="Ara..."
            >
          </div>
        </div>
      </header>

      <div v-if="isLoading" class="state-message">
        <div class="spinner"></div>
        <span>Yükleniyor...</span>
      </div>

      <div v-else-if="getError" class="state-message error">
        {{ getError }}
      </div>

      <div v-else class="projects-list-container">
        <transition-group name="fade-list" tag="div">
          <ProjectRow
            v-for="project in filteredProjects"
            :key="project.id"
            :project="project"
          />
        </transition-group>

        <div v-if="filteredProjects.length === 0" class="empty-state">
          <i class="fas fa-folder-open"></i>
          <p>Kriterlere uygun proje bulunamadı.</p>
        </div>
      </div>

    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ProjectRow from "@/components/Project/ProjectCard.vue";

export default {
  components: { ProjectRow },
  
  data() {
    return {
      searchQuery: '',
      selectedCategory: 'Tümü'
    };
  },

  computed: {
    ...mapGetters('projects', ['allProjects', 'isLoading', 'getError']),
    
    uniqueCategories() {
      const categories = ['Tümü'];
      if (this.allProjects) {
        this.allProjects.forEach(p => {
          if (p.categoryName && !categories.includes(p.categoryName)) {
            categories.push(p.categoryName);
          }
        });
      }
      return categories;
    },

    filteredProjects() {
      if (!this.allProjects) return [];

      return this.allProjects.filter(project => {
        const matchesCategory = this.selectedCategory === 'Tümü' || project.categoryName === this.selectedCategory;
        const matchesSearch = project.title.toLowerCase().includes(this.searchQuery.toLowerCase()) || 
                              project.description.toLowerCase().includes(this.searchQuery.toLowerCase());
        return matchesCategory && matchesSearch;
      });
    }
  },

  methods: {
    ...mapActions('projects', ['fetchProjects'])
  },
  
  created() {
    this.fetchProjects();
  },
};
</script>

<style scoped>
.main-container {
  font-family: 'Segoe UI', Inter, sans-serif;
  min-height: 100vh;
  background-color: #f8f9fa; 
  padding: 100px 20px 40px;
}

.content-wrapper {
  max-width: 900px; 
  margin: 0 auto;
}

/* Header Stilleri */
.page-header {
  margin-bottom: 40px;
  text-align: center;
}

.page-title {
  font-size: 2.5rem;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 10px;
}

.page-subtitle {
  color: #64748b;
  margin-bottom: 30px;
}

/* Filtreleme Çubuğu */
.filter-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 20px;
  margin-bottom: 20px;
}

.category-tabs {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
  justify-content: center;
}

.tab-btn {
  border: none;
  background: transparent;
  color: #64748b;
  padding: 8px 16px;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.tab-btn:hover {
  color: #1e293b;
  background: #e2e8f0;
}

.tab-btn.active {
  background: #1e293b;
  color: #ffffff;
  box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}

.search-box {
  position: relative;
  width: 250px;
}

.search-box i {
  position: absolute;
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #94a3b8;
  font-size: 0.9rem;
}

.search-box input {
  width: 100%;
  padding: 10px 10px 10px 35px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  outline: none;
  transition: border-color 0.2s;
  background: white;
}

.search-box input:focus {
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

/* Liste Konteyneri */
.projects-list-container {
  background: #ffffff;
  border-radius: 16px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.04);
  overflow: hidden; /* Köşeleri yuvarlatmak için */
  border: 1px solid rgba(0,0,0,0.02);
}

/* Loading & Empty States */
.state-message {
  text-align: center;
  padding: 50px;
  color: #64748b;
  font-size: 1.1rem;
}

.state-message.error {
  color: #ef4444;
}

.spinner {
  width: 30px;
  height: 30px;
  border: 3px solid #e2e8f0;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto 15px;
}

.empty-state {
  padding: 60px 20px;
  text-align: center;
  color: #94a3b8;
}

.empty-state i {
  font-size: 2rem;
  margin-bottom: 15px;
  display: block;
}

@keyframes spin { to { transform: rotate(360deg); } }

/* Liste Animasyonu (Opsiyonel) */
.fade-list-enter-active, .fade-list-leave-active {
  transition: all 0.3s ease;
}
.fade-list-enter {
  opacity: 0;
  transform: translateY(10px);
}
.fade-list-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

/* Responsive */
@media (max-width: 768px) {
  .filter-bar {
    flex-direction: column;
    align-items: stretch;
  }
  
  .search-box {
    width: 100%;
  }
  
  .page-title {
    font-size: 2rem;
  }
}
</style>