<template>
  <section class="about-section">
    <div class="container">
      
      <div class="profile-wrapper">
        <div class="profile-image-container">
          <div class="image-frame">
            <img :src="profileImage" :alt="name" />
          </div>
          <div class="decoration-circle"></div>
        </div>

        <div class="profile-content">
          <div class="intro-badge">Merhaba, Ben</div>
          <h1 class="name">{{ name }}</h1>
          <h2 class="job-title">{{ jobTitle }}</h2>
          <p class="location"><i class="fas fa-map-marker-alt"></i> {{ location }}</p>
          <p class="description">{{ description }}</p>

          <div class="social-actions">
            <a 
              v-for="social in socialLinks" 
              :key="social.platform"
              :href="social.url" 
              class="social-btn"
              target="_blank"
            >
              <span class="social-icon">
                <i v-if="social.platform === 'GitHub'" class="fab fa-github"></i>
                <i v-else-if="social.platform === 'LinkedIn'" class="fab fa-linkedin-in"></i>
                <i v-else-if="social.platform === 'Email'" class="fas fa-envelope"></i>
                <i v-else class="fas fa-link"></i>
              </span>
              {{ social.platform }}
            </a>
          </div>
        </div>
      </div>

      <div class="stats-grid">
        <div class="stat-card" v-for="stat in stats" :key="stat.label">
          <div class="stat-value">{{ stat.value }}</div>
          <div class="stat-label">{{ stat.label }}</div>
        </div>
      </div>

      <div class="skills-preview">
        <div class="section-header">
          <h3>Teknik Yetenekler</h3>
          <div class="header-line"></div>
        </div>
        
        <div v-if="loadingSkills" class="loading-skeleton">
          <div class="skeleton-bar"></div>
        </div>
        
        <div v-else class="skills-tags">
          <button 
            v-for="skill in skills" 
            :key="skill.id" 
            class="skill-tag interactive"
            @click="openSkillModal(skill)"
          >
            {{ skill.name }}
            <i class="fas fa-chevron-right tag-icon"></i>
          </button>
        </div>
      </div>
    </div>

    <transition name="modal-fade">
      <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
        <div class="modal-content">
          <button class="close-btn" @click="closeModal">
            <i class="fas fa-times"></i>
          </button>

          <div class="modal-header">
            <span class="id-badge">#{{ selectedSkill.id }}</span>
            <h2 class="skill-title">{{ selectedSkill.name }}</h2>
          </div>

          <div class="modal-body">
            <div class="visual-stat">
              <div class="circular-progress" :style="circularStyle">
                <div class="inner-circle">
                  <span class="percentage">%{{ selectedSkill.level }}</span>
                  <span class="label">Yetkinlik</span>
                </div>
              </div>
            </div>

            <div class="info-content">
              <h3>Teknoloji Hakkında</h3>
              <p class="modal-description">
                {{ selectedSkill.name }} teknolojisini projelerimde aktif olarak kullanıyorum. 
                </p>

              <div class="level-bar-container">
                <div class="bar-labels">
                  <span>Başlangıç</span>
                  <span>Uzman</span>
                </div>
                <div class="bar-bg">
                  <div class="bar-fill" :style="{ width: selectedSkill.level + '%' }"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </transition>

  </section>
</template>

<script>
import ApiService from "@/services/ApiService";
import emreImage from "@/Image/emre.jpg"; 

export default {
  name: 'AboutSection',
  data() {
    return {
      // Profil Verileri
      name: "Emre Almamış",
      jobTitle: "Yazılım Geliştirici",
      location: "Kocaeli, Türkiye",
      profileImage: emreImage,
      description: "Bilgisayar Mühendisliği alanında yeni mezun, modern web teknolojileri ve yazılım geliştirme konularında tutkulu bir geliştiriciyim. Karmaşık problemleri basit ve ölçeklenebilir çözümlere dönüştürmeyi seviyorum.",
      
      // Yetenek Verileri
      skills: [],            
      loadingSkills: true,    
      
      // Modal Yönetimi
      showModal: false,
      selectedSkill: null,

      // İstatistikler
      stats: [
        { value: "2024", label: "Mezuniyet" },
        { value: "15+", label: "Proje" },
        { value: "5+", label: "Core Tech" },
        { value: "∞", label: "Öğrenme" }
      ],
      socialLinks: [
        { platform: "GitHub", url: "https://github.com/kullaniciadi" },
        { platform: "LinkedIn", url: "https://linkedin.com/in/kullaniciadi" },
        { platform: "Email", url: "mailto:email@domain.com" }
      ]
    }
  },
  computed: {
    // Modal açıldığında dairesel grafik rengini hesaplar
    circularStyle() {
      if (!this.selectedSkill) return {};
      const deg = (this.selectedSkill.level * 3.6); // 100% = 360deg
      return {
        background: `conic-gradient(#3b82f6 ${deg}deg, #e2e8f0 0deg)`
      };
    }
  },
  async created() {
    await this.loadSkills();
  },
  methods: {
    async loadSkills() {
      // Tüm yetenekleri çekiyoruz
      const result = await ApiService.fetch("Skills");
      if (result.success) {
        this.skills = result.data;
      }
      this.loadingSkills = false;
    },

    // Modal Açma Fonksiyonu
    async openSkillModal(skill) {
      this.selectedSkill = skill;
      this.showModal = true;
      
      // Eğer listedeki veri eksikse ve detay için API'ye gitmek gerekiyorsa:
      // const detail = await ApiService.fetch(`skills/getById/${skill.id}`);
      // if(detail.success) this.selectedSkill = detail.data;
    },

    // Modal Kapama Fonksiyonu
    closeModal() {
      this.showModal = false;
      setTimeout(() => {
        this.selectedSkill = null;
      }, 300); // Animasyon bitince datayı temizle
    }
  }
}
</script>

<style scoped>
/* --- MEVCUT STİLLER (Değişmedi) --- */
.about-section {
  padding: 80px 0;
  background: linear-gradient(to bottom, #f8f9fa, #ffffff);
  color: #334155;
  min-height: 100vh;
}
.container { max-width: 1100px; margin: 0 auto; padding: 0 24px; }
.profile-wrapper { display: flex; align-items: center; gap: 60px; margin-bottom: 80px; }
.profile-image-container { position: relative; flex-shrink: 0; }
.image-frame img { width: 280px; height: 280px; object-fit: cover; border-radius: 20px; box-shadow: 20px 20px 60px rgba(0,0,0,0.1), -20px -20px 60px rgba(255,255,255,0.8); position: relative; z-index: 2; }
.decoration-circle { position: absolute; width: 100%; height: 100%; top: 20px; left: 20px; border: 3px solid #3b82f6; border-radius: 20px; z-index: 1; opacity: 0.3; }
.profile-content { flex: 1; }
.intro-badge { display: inline-block; background: #e0f2fe; color: #0284c7; padding: 6px 14px; border-radius: 30px; font-weight: 600; font-size: 0.9rem; margin-bottom: 15px; }
.name { font-size: 3rem; font-weight: 800; color: #0f172a; margin: 0 0 5px; letter-spacing: -1px; }
.job-title { font-size: 1.5rem; color: #64748b; font-weight: 500; margin-bottom: 10px; }
.location { color: #94a3b8; margin-bottom: 25px; font-size: 0.95rem; }
.description { font-size: 1.1rem; line-height: 1.8; color: #475569; margin-bottom: 30px; }
.social-actions { display: flex; gap: 15px; flex-wrap: wrap; }
.social-btn { display: inline-flex; align-items: center; gap: 8px; padding: 10px 20px; background: white; border: 1px solid #e2e8f0; border-radius: 12px; color: #1e293b; text-decoration: none; font-weight: 600; transition: all 0.3s ease; box-shadow: 0 2px 4px rgba(0,0,0,0.05); }
.social-btn:hover { transform: translateY(-3px); border-color: #3b82f6; color: #3b82f6; box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1); }
.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 30px; margin-bottom: 80px; }
.stat-card { background: white; padding: 30px; border-radius: 16px; text-align: center; border: 1px solid #f1f5f9; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05); transition: transform 0.3s; }
.stat-card:hover { transform: translateY(-5px); }
.stat-value { font-size: 2.5rem; font-weight: 800; color: #3b82f6; margin-bottom: 5px; }
.stat-label { color: #64748b; font-weight: 500; text-transform: uppercase; font-size: 0.85rem; letter-spacing: 1px; }

/* --- YETENEK LİSTESİ (Güncellendi) --- */
.skills-preview { text-align: center; }
.section-header h3 { font-size: 1.8rem; color: #0f172a; margin-bottom: 10px; }
.header-line { width: 60px; height: 4px; background: #3b82f6; margin: 0 auto 40px; border-radius: 2px; }
.skills-tags { display: flex; flex-wrap: wrap; justify-content: center; gap: 15px; }

/* Yeni Tıklanabilir Tag Stili */
.skill-tag.interactive {
  background: white;
  color: #334155;
  padding: 12px 24px;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.2s ease;
  display: inline-flex;
  align-items: center;
  gap: 8px;
}

.skill-tag.interactive:hover {
  background: #3b82f6;
  color: white;
  border-color: #3b82f6;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(59, 130, 246, 0.3);
}

.tag-icon { font-size: 0.8em; opacity: 0.7; transition: transform 0.2s; }
.skill-tag.interactive:hover .tag-icon { transform: translateX(3px); }

/* --- MODAL STİLLERİ (Yeni Eklendi) --- */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(15, 23, 42, 0.6); /* Koyu ve şeffaf arka plan */
  backdrop-filter: blur(5px); /* Arkadaki içeriği bulanıklaştır */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  padding: 20px;
}

.modal-content {
  background: white;
  width: 100%;
  max-width: 700px;
  border-radius: 24px;
  padding: 40px;
  position: relative;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
}

.close-btn {
  position: absolute;
  top: 20px;
  right: 20px;
  background: #f1f5f9;
  border: none;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  cursor: pointer;
  color: #64748b;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-btn:hover { background: #e2e8f0; color: #ef4444; }

.modal-header { text-align: center; margin-bottom: 30px; border-bottom: 1px solid #f1f5f9; padding-bottom: 20px; }
.id-badge { background: #f1f5f9; color: #94a3b8; padding: 4px 12px; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
.skill-title { margin-top: 10px; font-size: 2rem; color: #0f172a; }

.modal-body { display: flex; gap: 40px; align-items: center; }

/* Modal İçi Grafikler (Dairesel) */
.visual-stat { flex-shrink: 0; }
.circular-progress {
  width: 140px; height: 140px; border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  /* Background JS'den dinamik geliyor */
}
.inner-circle { width: 115px; height: 115px; background: white; border-radius: 50%; display: flex; flex-direction: column; align-items: center; justify-content: center; box-shadow: inset 0 0 10px rgba(0,0,0,0.05); }
.percentage { font-size: 1.8rem; font-weight: 800; color: #3b82f6; }
.label { font-size: 0.75rem; color: #94a3b8; text-transform: uppercase; }

/* Modal İçi Bilgiler */
.info-content { flex: 1; }
.info-content h3 { font-size: 1.1rem; color: #334155; margin-bottom: 10px; }
.modal-description { color: #64748b; line-height: 1.6; margin-bottom: 25px; font-size: 0.95rem; }

/* Progress Bar */
.level-bar-container { margin-top: 10px; }
.bar-labels { display: flex; justify-content: space-between; font-size: 0.8rem; color: #94a3b8; margin-bottom: 6px; font-weight: 600; }
.bar-bg { height: 8px; background: #f1f5f9; border-radius: 4px; overflow: hidden; }
.bar-fill { height: 100%; background: linear-gradient(90deg, #3b82f6, #2563eb); border-radius: 4px; transition: width 1s ease; }

/* Animasyonlar */
.modal-fade-enter-active, .modal-fade-leave-active { transition: opacity 0.3s ease; }
.modal-fade-enter-from, .modal-fade-leave-to { opacity: 0; }
.modal-fade-enter-active .modal-content { animation: slide-up 0.3s ease-out; }
.modal-fade-leave-active .modal-content { animation: slide-up 0.3s ease-in reverse; }

@keyframes slide-up {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

/* Responsive Modal */
@media (max-width: 900px) {
  .profile-wrapper { flex-direction: column; text-align: center; gap: 40px; }
  .modal-body { flex-direction: column; text-align: center; gap: 20px; }
  .modal-content { padding: 25px; }
}
</style>