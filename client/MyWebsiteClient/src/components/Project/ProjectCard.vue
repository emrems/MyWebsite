<template>
  <div class="project-row" @click="goToLiveDemo">
    <div class="row-content">
      <div class="row-header">
        <h3 class="project-title">{{ project.title }}</h3>
        <span class="category-badge">{{ project.categoryName }}</span>
      </div>
      <p class="project-desc">{{ truncatedDescription }}</p>
    </div>

    <div class="row-tech">
      <span 
        v-for="(tech, index) in techArray" 
        :key="index" 
        class="tech-tag"
      >
        #{{ tech }}
      </span>
    </div>

    <div class="row-actions">
      <a 
        v-if="project.githubUrl" 
        :href="project.githubUrl" 
        target="_blank" 
        class="action-icon github"
        @click.stop
        title="GitHub Kodları"
      >
        <i class="fab fa-github"></i>
      </a>
      
      <div class="arrow-circle">
        <i class="fas fa-arrow-right"></i>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ProjectRow",
  props: {
    project: {
      type: Object,
      required: true,
    },
  },
  computed: {
    // Teknolojileri virgülle ayrılmış string'den diziye çevirir
    techArray() {
      if (!this.project.technologies) return [];
      // En fazla 3 tanesini gösterip kalabalığı önleyelim
      return this.project.technologies.split(',').map(t => t.trim()).slice(0, 3);
    },
    // Açıklamayı belli bir karakterden sonra keser
    truncatedDescription() {
      const desc = this.project.description;
      if (desc && desc.length > 90) {
        return desc.substring(0, 90) + '...';
      }
      return desc;
    }
  },
  methods: {
    goToLiveDemo() {
      if (this.project.liveDemoUrl) {
        window.open(this.project.liveDemoUrl, '_blank');
      }
    }
  }
};
</script>

<style scoped>
.project-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 24px 30px;
  background-color: #ffffff;
  border-bottom: 1px solid #f1f5f9;
  cursor: pointer;
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
}

.project-row:last-child {
  border-bottom: none;
}

.project-row:hover {
  background-color: #f8fafc;
  padding-left: 36px;
}

.project-row::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 4px;
  background: #3b82f6;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.project-row:hover::before {
  opacity: 1;
}

.row-content {
  flex: 2;
  padding-right: 20px;
}

.row-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 6px;
}

.project-title {
  font-size: 1.15rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
}

.category-badge {
  font-size: 0.7rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  padding: 4px 10px;
  border-radius: 20px;
  background: #e0f2fe;
  color: #0284c7;
}

.project-desc {
  font-size: 0.9rem;
  color: #64748b;
  margin: 0;
  line-height: 1.5;
}

.row-tech {
  flex: 1;
  display: flex;
  gap: 10px;
  justify-content: flex-start;
}

.tech-tag {
  font-family: 'Courier New', monospace;
  font-size: 0.85rem;
  color: #94a3b8;
}

.row-actions {
  display: flex;
  align-items: center;
  gap: 20px;
}

.action-icon {
  font-size: 1.4rem;
  color: #94a3b8;
  transition: color 0.2s ease;
  z-index: 5; 
}

.action-icon:hover {
  color: #1e293b;
}

.arrow-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #f1f5f9;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #3b82f6;
  transition: all 0.3s ease;
}

.project-row:hover .arrow-circle {
  background: #3b82f6;
  color: white;
  transform: translateX(5px);
}

@media (max-width: 768px) {
  .project-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 15px;
    padding: 20px;
  }

  .project-row:hover {
    padding-left: 20px; 
  }

  .row-content {
    width: 100%;
    padding: 0;
  }

  .row-tech {
    display: none; 
  }

  .row-actions {
    width: 100%;
    justify-content: space-between;
    margin-top: 5px;
  }
  
  .row-header {
    flex-wrap: wrap;
  }
}
</style>