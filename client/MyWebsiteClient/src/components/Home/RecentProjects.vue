<template>
  <section id="projects" class="section projects-section">
    <h2 class="section-heading">Son Projelerim</h2>
    <div v-if="isLoading" class="loading-state">
      Projeler yükleniyor...
    </div>
    <div v-else-if="getError" class="error-state">
      {{ getError }}
    </div>
    <div v-else class="projects-grid">
      <div v-for="project in allProjects" :key="project.id" class="project-card">
        <h3 class="project-title">{{ project.title }}</h3>
        <p class="project-description">{{ project.description }}</p>
        <div class="project-tech-tags">
          <span v-for="tech in project.technologies.split(',').map(t => t.trim())" :key="tech" class="tech-tag primary-tag">
            {{ tech }}
          </span>
        </div>
        <a :href="project.githubUrl" target="_blank" class="project-link-btn">GitHub</a>
        <a :href="project.liveDemoUrl" target="_blank" class="project-link-btn">Canlı Demo</a>
      </div>
    </div>
    <div class="all-projects-button-container">
      <router-link to="/Project" class="btn btn-dark">
        Tüm Projeler
      </router-link>
    </div>
  </section>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'RecentProjects',
  computed: {
    // Vuex 'projects' modülünden verileri getirir
    ...mapGetters('projects', ['allProjects', 'isLoading', 'getError'])
  },
  methods: {
    // Vuex 'projects' modülünden action'ları çağırır
    ...mapActions('projects', ['fetchProjects'])
  },
  created() {
    // Bileşen oluşturulduğunda projeleri çekme action'ını çalıştırır
    this.fetchProjects();
  }
};
</script>

<style scoped>
/* Projeler bölümüne ait tüm CSS kodlarını buraya taşıyın */
.projects-section {
  background-color: #f8f8f8;
}

.projects-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2rem;
}

.project-card {
  background-color: white;
  border-radius: 1rem;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.08);
  padding: 2rem;
  text-align: center;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.project-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.12);
}

.project-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.75rem;
}

.project-description {
  color: #666;
  margin-bottom: 1.5rem;
  flex-grow: 1;
}

.project-tech-tags {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.tech-tag {
  padding: 0.3rem 0.8rem;
  border-radius: 9999px;
  font-size: 0.8rem;
  font-weight: 500;
}

.primary-tag {
  background-color: #e0f2ff;
  color: #007bff;
}

.secondary-tag {
  background-color: #ffe0e6;
  color: #dc3545;
}

.gray-tag {
  background-color: #e9ecef;
  color: #6c757d;
}

.project-link-btn {
  display: inline-block;
  background-color: #007bff;
  color: white;
  padding: 0.6rem 1.2rem;
  border-radius: 9999px;
  text-decoration: none;
  font-weight: 500;
  transition: background-color 0.3s ease, transform 0.3s ease;
  margin-top: 1rem;
}

.project-link-btn:hover {
  background-color: #0056b3;
  transform: translateY(-2px);
}

.all-projects-button-container {
  text-align: center;
  margin-top: 3rem;
}

.loading-state, .error-state {
  text-align: center;
  font-size: 1.2em;
  color: #555;
  margin-top: 50px;
}

.error-state {
  color: #d9534f;
}

</style>