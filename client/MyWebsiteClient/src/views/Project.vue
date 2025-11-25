<template>
  <div class="main-container">
    <div class="content-wrapper">

      <!-- Header -->
      <header class="page-header">
        <h1 class="page-title">Projelerim</h1>
        <p class="page-subtitle">
          Geliştirdiğim uygulamalar, açık kaynak kodlar ve deneyimler.
        </p>

        <!-- Filter + Search -->
        <div class="filter-bar">
          
          <!-- Categories -->
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

          <!-- Search -->
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

      <!-- Loading -->
      <LoadingSpinner
        v-if="isLoading"
        text="Projeler yükleniyor..."
      />

      <!-- Error -->
      <ErrorMessage 
        v-else-if="getError"
        :message="getError"
        :showRetry="true"
        @retry="fetchProjects"
      />

      <!-- Projects List -->
      <div v-else class="projects-card">

        <transition-group
          name="fade-list"
          tag="div"
          class="projects-grid"
        >
          <ProjectRow
            v-for="project in filteredProjects"
            :key="project.id"
            :project="project"
          />
        </transition-group>
 
        <div v-if="filteredProjects.length === 0" class="empty-state">
          📁 Kriterlere uygun proje bulunamadı.
        </div>

      </div>

    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import ErrorMessage from "@/components/common/ErrorMessage.vue";
import LoadingSpinner from "@/components/common/LoadingSpinner.vue";
import ProjectRow from "@/components/Project/ProjectCard.vue";

export default {
  components: {
    ProjectRow,
    ErrorMessage,
    LoadingSpinner
  },

  data() {
    return {
      searchQuery: "",
      selectedCategory: "Tümü",
    };
  },

  computed: {
    ...mapGetters("projects", ["allProjects", "isLoading", "getError"]),

    uniqueCategories() {
      const categories = ["Tümü"];
      this.allProjects?.forEach(p => {
        if (p.categoryName && !categories.includes(p.categoryName)) {
          categories.push(p.categoryName);
        }
      });
      return categories;
    },

    filteredProjects() {
      if (!this.allProjects) return [];

      const q = this.searchQuery.toLowerCase();

      return this.allProjects.filter(project => {
        const matchesCategory =
          this.selectedCategory === "Tümü" ||
          project.categoryName === this.selectedCategory;

        const matchesSearch =
          project.title.toLowerCase().includes(q) ||
          project.description.toLowerCase().includes(q);

        return matchesCategory && matchesSearch;
      });
    },
  },

  methods: {
    ...mapActions("projects", ["fetchProjects"]),
  },

  created() {
    this.fetchProjects();
  },
};
</script>

<style scoped>
.main-container {
  min-height: 100vh;
  background-color: #f8f9fa;
  padding: 100px 20px;
  font-family: Inter, sans-serif;
}

.content-wrapper {
  max-width: 900px;
  margin: auto;
}

.page-header {
  text-align: center;
  margin-bottom: 40px;
}

.page-title {
  font-size: 2.4rem;
  font-weight: 800;
  color: #1e293b;
}

.page-subtitle {
  color: #64748b;
  margin-bottom: 20px;
}

/* Filter Bar */
.filter-bar {
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 15px;
  margin-top: 20px;
}

.category-tabs {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.tab-btn {
  padding: 8px 16px;
  border-radius: 20px;
  border: none;
  background: transparent;
  color: #64748b;
  cursor: pointer;
  font-weight: 600;
  transition: 0.25s;
}

.tab-btn:hover {
  background: #e2e8f0;
  color: #1e293b;
}

.tab-btn.active {
  background: #1e293b;
  color: #fff;
}

.search-box {
  position: relative;
  width: 250px;
}

.search-box i {
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  position: absolute;
  color: #94a3b8;
}

.search-box input {
  width: 100%;
  padding: 10px 12px 10px 35px;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  transition: border-color 0.2s;
}

.search-box input:focus {
  border-color: #3b82f6;
}

.projects-card {
  background: #fff;
  border-radius: 16px;
  padding: 20px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.04);
}

.projects-grid {
  display: grid;
  gap: 2rem;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
}

/* Empty */
.empty-state {
  padding: 50px;
  text-align: center;
  color: #94a3b8;
  font-size: 1.1rem;
}

/* Animations */
.fade-list-enter-active,
.fade-list-leave-active {
  transition: 0.3s ease;
}

.fade-list-enter {
  opacity: 0;
  transform: translateY(10px);
}

.fade-list-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

@media (max-width: 768px) {
  .filter-bar { flex-direction: column; }
  .search-box { width: 100%; }
}
</style>
