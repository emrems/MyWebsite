<template>
  <section id="projects" class="section projects-section">
    <h2 class="section-heading">Son Projelerim</h2>
    
    <div v-if="isLoading" class="loading-state">
      <div class="loader"></div>
      <p>Projeler yükleniyor...</p>
    </div>
    
    <div v-else-if="getError" class="error-state">
      {{ getError }}
    </div>
    
    <div v-else class="projects-container">
      <div 
        v-for="(project, index) in allProjects" 
        :key="project.id" 
        class="project-item"
        :class="{ 'reverse': index % 2 !== 0 }"
      >
        <div class="project-content">
          <div class="project-number">0{{ index + 1 }}</div>
          <h3 class="project-title">{{ project.title }}</h3>
          <p class="project-description">{{ project.description }}</p>
          
          <div class="project-tech-stack">
            <div class="tech-label">Teknolojiler</div>
            <div class="tech-list">
              <span 
                v-for="tech in project.technologies.split(',').map(t => t.trim())" 
                :key="tech" 
                class="tech-badge"
              >
                {{ tech }}
              </span>
            </div>
          </div>
          
          <div class="project-links">
            <a :href="project.githubUrl" target="_blank" class="link-item github-link">
              <span class="link-icon">→</span>
              <span>GitHub'da Görüntüle</span>
            </a>
            <a :href="project.liveDemoUrl" target="_blank" class="link-item demo-link">
              <span class="link-icon">→</span>
              <span>Canlı Demo</span>
            </a>
          </div>
        </div>
        
        <div class="project-visual">
          <div class="visual-placeholder">
            <div class="placeholder-grid">
              <div class="grid-item"></div>
              <div class="grid-item"></div>
              <div class="grid-item"></div>
              <div class="grid-item"></div>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <div class="all-projects-section">
      <div class="divider"></div>
      <router-link to="/Project" class="view-all-link">
        <span>Tüm Projeleri Keşfet</span>
        <span class="arrow">→</span>
      </router-link>
    </div>
  </section>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'RecentProjects',
  computed: {
    ...mapGetters('projects', ['allProjects', 'isLoading', 'getError'])
  },
  methods: {
    ...mapActions('projects', ['fetchProjects'])
  },
  created() {
    this.fetchProjects();
  }
};
</script>

<style scoped>
.projects-section {
  background: white;
  padding: 4rem 2rem;
}

.section-heading {
  font-size: 2.25rem;
  font-weight: 800;
  color: #1a1a2e;
  text-align: center;
  margin-bottom: 3rem;
  position: relative;
  letter-spacing: -0.02em;
}

.section-heading::after {
  content: '';
  position: absolute;
  bottom: -15px;
  left: 50%;
  transform: translateX(-50%);
  width: 60px;
  height: 5px;
  background: linear-gradient(90deg, #007bff, #00d4ff);
  border-radius: 3px;
}

/* Loading State */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 300px;
  gap: 1rem;
}

.loader {
  width: 50px;
  height: 50px;
  border: 4px solid #e0e0e0;
  border-top-color: #007bff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.error-state {
  text-align: center;
  font-size: 1.2rem;
  color: #dc3545;
  padding: 3rem;
  background-color: #ffe0e6;
  border-radius: 1rem;
  margin: 2rem auto;
  max-width: 600px;
}

/* Projects Container */
.projects-container {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 3rem;
}

.project-item {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2.5rem;
  align-items: center;
  padding: 2.5rem;
  background: white;
  border-radius: 2rem;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.06);
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
}

.project-item::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 4px;
  height: 100%;
  background: linear-gradient(180deg, #007bff, #00d4ff);
  transform: scaleY(0);
  transition: transform 0.4s ease;
}

.project-item:hover {
  transform: translateY(-8px);
  box-shadow: 0 20px 60px rgba(0, 123, 255, 0.15);
}

.project-item:hover::before {
  transform: scaleY(1);
}

.project-item.reverse {
  grid-template-columns: 1fr 1fr;
}

.project-item.reverse .project-content {
  order: 2;
}

.project-item.reverse .project-visual {
  order: 1;
}

/* Project Content */
.project-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.project-number {
  font-size: 3rem;
  font-weight: 900;
  color: #f0f0f0;
  line-height: 1;
  letter-spacing: -0.05em;
}

.project-title {
  font-size: 1.6rem;
  font-weight: 700;
  color: #1a1a2e;
  margin-top: -1.5rem;
  line-height: 1.2;
}

.project-description {
  font-size: 1rem;
  color: #666;
  line-height: 1.6;
}

/* Tech Stack */
.project-tech-stack {
  margin-top: 0.75rem;
}

.tech-label {
  font-size: 0.8rem;
  font-weight: 600;
  color: #888;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  margin-bottom: 0.5rem;
}

.tech-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.tech-badge {
  padding: 0.35rem 0.9rem;
  background: linear-gradient(135deg, #e0f2ff, #f0f9ff);
  color: #007bff;
  border-radius: 50px;
  font-size: 0.8rem;
  font-weight: 600;
  border: 1px solid rgba(0, 123, 255, 0.2);
  transition: all 0.3s ease;
}

.tech-badge:hover {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  transform: translateY(-2px);
}

/* Project Links */
.project-links {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.75rem;
}

.link-item {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.65rem 1.25rem;
  background: #1a1a2e;
  color: white;
  text-decoration: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.85rem;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.link-item::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.5s ease;
}

.link-item:hover::before {
  left: 100%;
}

.link-item:hover {
  background: #007bff;
  transform: translateX(5px);
}

.link-icon {
  font-size: 1.2rem;
  transition: transform 0.3s ease;
}

.link-item:hover .link-icon {
  transform: translateX(5px);
}

.github-link {
  background: #24292e;
}

.github-link:hover {
  background: #1a1d21;
}

/* Project Visual */
.project-visual {
  position: relative;
  height: 300px;
}

.visual-placeholder {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 1.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 20px 40px rgba(102, 126, 234, 0.3);
  transition: all 0.4s ease;
}

.project-item:hover .visual-placeholder {
  transform: scale(1.05) rotate(2deg);
  box-shadow: 0 30px 60px rgba(102, 126, 234, 0.4);
}

.placeholder-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.75rem;
  padding: 1.5rem;
}

.grid-item {
  width: 70px;
  height: 70px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 1rem;
  backdrop-filter: blur(10px);
  animation: float 3s ease-in-out infinite;
}

.grid-item:nth-child(2) {
  animation-delay: 0.5s;
}

.grid-item:nth-child(3) {
  animation-delay: 1s;
}

.grid-item:nth-child(4) {
  animation-delay: 1.5s;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

/* All Projects Section */
.all-projects-section {
  margin-top: 3rem;
  text-align: center;
}

.divider {
  width: 100px;
  height: 2px;
  background: linear-gradient(90deg, transparent, #007bff, transparent);
  margin: 0 auto 1.5rem;
}

.view-all-link {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 2rem;
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  text-decoration: none;
  border-radius: 50px;
  font-weight: 700;
  font-size: 1rem;
  transition: all 0.4s ease;
  box-shadow: 0 10px 30px rgba(0, 123, 255, 0.3);
  position: relative;
  overflow: hidden;
}

.view-all-link::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  transition: width 0.6s ease, height 0.6s ease;
}

.view-all-link:hover::before {
  width: 300px;
  height: 300px;
}

.view-all-link:hover {
  transform: translateY(-3px);
  box-shadow: 0 15px 40px rgba(0, 123, 255, 0.4);
}

.arrow {
  font-size: 1.5rem;
  transition: transform 0.3s ease;
}

.view-all-link:hover .arrow {
  transform: translateX(10px);
}

/* Responsive */
@media (max-width: 968px) {
  .project-item,
  .project-item.reverse {
    grid-template-columns: 1fr;
    gap: 2rem;
  }
  
  .project-item.reverse .project-content,
  .project-item.reverse .project-visual {
    order: 0;
  }
  
  .project-visual {
    height: 250px;
  }
  
  .section-heading {
    font-size: 2.5rem;
  }
  
  .project-title {
    font-size: 1.75rem;
  }
  
  .project-links {
    flex-direction: column;
  }
  
  .link-item {
    justify-content: center;
  }
}

@media (max-width: 640px) {
  .projects-section {
    padding: 3rem 1rem;
  }
  
  .project-item {
    padding: 2rem 1.5rem;
    gap: 1.5rem;
  }
  
  .project-number {
    font-size: 3rem;
  }
  
  .project-title {
    font-size: 1.5rem;
    margin-top: -1.5rem;
  }
  
  .project-description {
    font-size: 1rem;
  }
  
  .view-all-link {
    padding: 1rem 2rem;
    font-size: 1rem;
  }
}
</style>