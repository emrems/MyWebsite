<template>
  <div class="project-card">
    <!-- Başlık -->
    <h2 class="project-title">{{ project.title }}</h2>

    <!-- Kategori -->
    <div class="project-category">
      <span class="badge">
        <i class="fas fa-folder-open"></i> {{ project.categoryName }}
      </span>
    </div>

    <!-- Açıklama -->
    <p class="project-description">{{ project.description }}</p>

    <!-- Teknolojiler -->
    <div class="project-technologies">
      <strong>Teknolojiler:</strong>
      <div class="tech-list">
        <span
          v-for="(tech, index) in project.technologies.split(',')"
          :key="index"
          class="tech-badge"
        >
          <i class="fas fa-code"></i> {{ tech.trim() }}
        </span>
      </div>
    </div>

    <!-- Linkler -->
    <div class="project-links">
      <a
        :href="project.githubUrl"
        target="_blank"
        class="link-button github-link"
      >
        <i class="fab fa-github"></i> GitHub
      </a>
      <a
        :href="project.liveDemoUrl"
        target="_blank"
        class="link-button demo-link"
      >
        <i class="fas fa-arrow-up-right-from-square"></i> Canlı Demo
      </a>
    </div>
  </div>
</template>

<script>
export default {
  name: "ProjectCard",
  props: {
    project: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style scoped>
.project-card {
  background: linear-gradient(135deg, #ffffff, #f8faff);
  border: 1px solid #e6e9ef;
  border-radius: 12px;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.08);
  padding: 25px;
  width: 100%;
  max-width: 360px;
  transition: transform 0.25s ease, box-shadow 0.25s ease;
  display: flex;
  flex-direction: column;
}

.project-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.12);
}

.project-title {
  color: #1e293b;
  margin-top: 0;
  font-size: 1.5em;
  font-weight: 600;
  border-bottom: 2px solid #3b82f6;
  padding-bottom: 10px;
}

.project-category {
  margin-top: 12px;
}

.badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: #e0f2fe;
  color: #0369a1;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 0.85em;
  font-weight: 500;
}

.project-description {
  color: #475569;
  margin-top: 16px;
  line-height: 1.6;
}

.project-technologies {
  margin-top: 16px;
}

.tech-list {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-top: 6px;
}

.tech-badge {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  background: #f1f5f9;
  color: #334155;
  padding: 6px 10px;
  border-radius: 8px;
  font-size: 0.8em;
  transition: background 0.2s;
}

.tech-badge:hover {
  background: #e2e8f0;
}

.project-links {
  margin-top: auto;
  padding-top: 20px;
  display: flex;
  justify-content: space-between;
  gap: 12px;
}

.link-button {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 10px 15px;
  border-radius: 8px;
  text-decoration: none;
  color: #fff;
  flex-grow: 1;
  font-weight: 500;
  transition: transform 0.2s, background-color 0.3s ease;
}

.link-button:hover {
  transform: translateY(-2px);
}

.github-link {
  background-color: #24292f; /* GitHub resmi rengi */
}

.github-link:hover {
  background-color: #000;
}

.demo-link {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
}

.demo-link:hover {
  background: linear-gradient(135deg, #2563eb, #1d4ed8);
}
</style>
