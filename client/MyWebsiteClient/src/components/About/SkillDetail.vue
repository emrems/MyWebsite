<template>
  <section class="skill-detail-section">
    <div class="container">

      <button class="back-btn" @click="$router.back()">← Geri</button>

      <div v-if="loading" class="loading">Yükleniyor...</div>

      <div v-else-if="skill" class="skill-card">
        <h2>{{ skill.name }}</h2>

        <p><strong>ID:</strong> {{ skill.id }}</p>

        <p><strong>Seviye:</strong> %{{ skill.level }}</p>

        <div class="skill-bar-wrapper">
          <div class="skill-bar" :style="{ width: skill.level + '%' }"></div>
        </div>
      </div>

      <div v-else class="error">Skill bulunamadı.</div>

    </div>
  </section>
</template>

<script>
import ApiService from "@/services/ApiService";

export default {
  name: "SkillDetail",

  data() {
    return {
      skill: null,
      loading: true
    };
  },

  async created() {
    await this.fetchSkill();
  },

  methods: {
    async fetchSkill() {
      const id = this.$route.params.id;
      const result = await ApiService.fetch(`Skills/${id}`);

      if (result.success) {
        this.skill = result.data;
      } else {
        console.error("Skill detayı alınamadı:", result.message);
      }

      this.loading = false;
    }
  }
};
</script>

<style scoped>
.container {
  max-width: 700px;
  margin: 0 auto;
  padding: 20px;
}

.back-btn {
  background: #2c3e50;
  color: white;
  padding: 8px 15px;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  margin-bottom: 20px;
}

.skill-card {
  background: white;
  padding: 25px;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.skill-bar-wrapper {
  height: 10px;
  background: #e9ecef;
  border-radius: 5px;
  margin-top: 10px;
  overflow: hidden;
}

.skill-bar {
  height: 100%;
  background: #2c3e50;
  transition: width 1s;
}

.loading, .error {
  text-align: center;
  font-size: 1.2rem;
}
</style>
