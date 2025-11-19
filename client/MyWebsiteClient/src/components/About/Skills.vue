<template>
  <section class="skills-section">
    <h2 class="title">Yeteneklerim</h2>

    <div v-if="loading" class="loading">Yükleniyor...</div>

    <div class="skills-grid" v-else>
      <div 
        class="skill-item"
        v-for="skill in skills"
        :key="skill.id"
        @click="goToDetail(skill.id)"
      >
        <span class="skill-name">{{ skill.name }}</span>
        <div class="skill-level">
          <div class="skill-bar" :style="{ width: skill.level + '%' }"></div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import ApiService from "@/services/ApiService";

export default {
  name: "Skills",

  data() {
    return {
      skills: [],
      loading: true
    };
  },

  async created() {
    await this.fetchSkills();
  },

  methods: {
    async fetchSkills() {
      const result = await ApiService.fetch("Skills");

      if (result.success) {
        this.skills = result.data;
      } else {
        console.error("Skill listesi alınamadı:", result.message);
      }

      this.loading = false;
    },

    goToDetail(id) {
      this.$router.push({ name: "SkillDetail", params: { id } });
    }
  }
};
</script>

<style scoped>
.title {
  font-size: 2rem;
  margin-bottom: 20px;
  color: #2c3e50;
  text-align: center;
}

.skills-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 25px;
}

.skill-item {
  background: white;
  padding: 18px;
  border-radius: 8px;
  border: 1px solid #e9ecef;
  cursor: pointer;
  transition: 0.3s;
}

.skill-item:hover {
  transform: translateY(-5px);
}

.skill-name {
  font-weight: 600;
  margin-bottom: 10px;
}

.skill-level {
  height: 6px;
  background: #e9ecef;
  border-radius: 3px;
  overflow: hidden;
}

.skill-bar {
  height: 100%;
  background: #2c3e50;
  transition: width 1s;
}

.loading {
  text-align: center;
  font-size: 1.2rem;
}
</style>
