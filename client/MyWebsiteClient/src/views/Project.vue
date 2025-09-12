<template>
  <div class="main-container">
    <h1 class="page-title">Tüm Projelerim</h1> <div v-if="isLoading" class="loading-state">
      Projeler yükleniyor...
    </div>

    <div v-else-if="getError" class="error-state">
      {{ getError }}
    </div>

    <div v-else class="projects-container">
      <ProjectCard
        v-for="project in allProjects"
        :key="project.id"
        :project="project"
      />
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ProjectCard from "@/components/Project/ProjectCard.vue";

export default {
  components: { ProjectCard },
  
  computed: {
    ...mapGetters('projects', ['allProjects', 'isLoading', 'getError'])
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
  font-family: Arial, sans-serif;
  padding: 20px;
  /* Sayfa içeriğinin en üstte yapışmasını engellemek için genel bir üst boşluk */
  padding-top: 80px; /* Navigasyon çubuğu yüksekliğine göre ayarlanabilir */
  min-height: 100vh; /* Minimum ekran yüksekliği */
  background-color: #f8f8f8; /* Arka plan rengi */
}

.page-title {
  text-align: center;
  font-size: 2.8em; /* Daha büyük ve belirgin başlık */
  color: #2c3e50;
  margin-bottom: 40px; /* Başlık ile kartlar arasına boşluk */
  font-weight: 700;
  position: relative;
}

.page-title::after {
  content: '';
  position: absolute;
  bottom: -10px;
  left: 50%;
  transform: translateX(-50%);
  width: 100px;
  height: 4px;
  background-color: #007bff;
  border-radius: 2px;
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

.projects-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 30px; /* Kartlar arası boşluğu biraz artırdık */
  justify-items: center;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto; /* Ortalamak için */
  /* Kartların kendilerine özgü bir üst boşluğa ihtiyacı varsa */
  padding-top: 20px; 
}

/* Küçük Ekranlar İçin Medya Sorguları */
@media (max-width: 768px) {
  .main-container {
    padding-top: 60px; /* Daha küçük ekranlarda üst boşluğu azaltabiliriz */
  }
  .page-title {
    font-size: 2.2em;
    margin-bottom: 30px;
  }
  .projects-container {
    gap: 20px;
  }
}
</style>