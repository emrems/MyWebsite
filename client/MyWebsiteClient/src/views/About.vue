<template>
  <section class="about-section">
    <div class="container">
      <!-- Başlık -->
      <div class="section-header">
        <h2 class="section-title">Hakkımda</h2>
        <div class="title-underline"></div>
      </div>

      <!-- Profil ve Ana Bilgiler -->
      <div class="profile-section">
        <div class="profile-image">
          <img :src="profileImage" :alt="name" />
        </div>
        <div class="profile-info">
          <h3 class="name">{{ name }}</h3>
          <p class="title">{{ jobTitle }}</p>
          <p class="location">{{ location }}</p>
        </div>
      </div>

      <!-- Açıklama -->
      <div class="description-section">
        <p>{{ description }}</p>
      </div>

      <!-- Yetenekler -->
      <div class="skills-section">
        <h4>Teknik Yetenekler</h4>
        <div class="skills-grid">
          <div 
            v-for="skill in skills" 
            :key="skill.name" 
            class="skill-item"
          >
            <span class="skill-name">{{ skill.name }}</span>
            <div class="skill-level">
              <div 
                class="skill-bar" 
                :style="{ width: skill.level + '%' }"
              ></div>
            </div>
          </div>
        </div>
      </div>

      <!-- İstatistikler -->
      <div class="stats-section">
        <div class="stat-item" v-for="stat in stats" :key="stat.label">
          <div class="stat-number">{{ stat.value }}</div>
          <div class="stat-label">{{ stat.label }}</div>
        </div>
      </div>

      <!-- İletişim -->
      <div class="contact-section">
        <h4>İletişim</h4>
        <div class="contact-links">
          <a 
            v-for="social in socialLinks" 
            :key="social.platform"
            :href="social.url" 
            class="contact-link"
            target="_blank"
            rel="noopener noreferrer"
          >
            {{ social.platform }}
          </a>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: 'AboutSection',
  data() {
    return {
      name: "Emre Almamış",
      jobTitle: "Yazılım Geliştirici",
      location: "Kocaeli, Türkiye",
      profileImage: "/path/to/your/photo.jpg",
      description: "Bilgisayar Mühendisliği alanında yeni mezun, modern web teknolojileri ve yazılım geliştirme konularında tutkulu bir geliştiriciyim. Vue.js, React ve Node.js gibi teknolojilerle projeler geliştiriyor, sürekli öğrenmeye ve kendimi geliştirmeye odaklanıyorum. Yaratıcı çözümler üretmek ve kullanıcı deneyimini ön planda tutan uygulamalar geliştirmek benim için büyük bir motivasyon kaynağı.",
      
      skills: [
        { name: "Vue.js" },
        { name: "JavaScript" },
        { name: "HTML/CSS" },
        { name: "React" },
        { name: "Node.js" },
        { name: "Python" },
        { name: "Git" },
        { name: "SQL" }
      ],

      stats: [
        { value: "2024", label: "Mezuniyet Yılı" },
        { value: "15+", label: "Tamamlanan Proje" },
        { value: "5+", label: "Teknoloji" },
        { value: "∞", label: "Öğrenme İsteği" }
      ],

      socialLinks: [
        { platform: "GitHub", url: "https://github.com/kullaniciadi" },
        { platform: "LinkedIn", url: "https://linkedin.com/in/kullaniciadi" },
        { platform: "Email", url: "mailto:email@domain.com" },
        { platform: "Portfolio", url: "https://yourportfolio.com" }
      ]
    }
  }
}
</script>

<style scoped>
.about-section {
  padding: 60px 0;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.section-header {
  text-align: center;
  margin-bottom: 50px;
}

.section-title {
  font-size: 2.5rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 15px;
}

.title-underline {
  width: 60px;
  height: 3px;
  background-color: #2c3e50;
  margin: 0 auto;
}

.profile-section {
  display: flex;
  align-items: center;
  margin-bottom: 40px;
  padding-bottom: 40px;
  border-bottom: 1px solid #e9ecef;
}

.profile-image {
  margin-right: 40px;
}

.profile-image img {
  width: 150px;
  height: 150px;
  border-radius: 10px;
  object-fit: cover;
  border: 2px solid #e9ecef;
}

.name {
  font-size: 2rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 10px;
}

.title {
  font-size: 1.2rem;
  color: #6c757d;
  margin-bottom: 8px;
}

.location {
  font-size: 1rem;
  color: #6c757d;
}

.description-section {
  margin-bottom: 50px;
}

.description-section p {
  font-size: 1.1rem;
  line-height: 1.7;
  color: #495057;
  text-align: justify;
  max-width: none;
}

.skills-section {
  margin-bottom: 50px;
}

.skills-section h4 {
  font-size: 1.5rem;
  color: #2c3e50;
  margin-bottom: 30px;
  font-weight: 600;
}

.skills-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 25px;
}

.skill-item {
  padding: 20px;
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 8px;
}

.skill-name {
  font-weight: 500;
  font-size: 1rem;
  color: #2c3e50;
  display: block;
  margin-bottom: 12px;
}

.skill-level {
  height: 6px;
  background-color: #e9ecef;
  border-radius: 3px;
  overflow: hidden;
}

.skill-bar {
  height: 100%;
  background-color: #2c3e50;
  border-radius: 3px;
  transition: width 1.5s ease;
}

.stats-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 30px;
  margin-bottom: 50px;
  padding: 40px;
  background: white;
  border: 1px solid #e9ecef;
  border-radius: 8px;
}

.stat-item {
  text-align: center;
}

.stat-number {
  font-size: 2.2rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 8px;
}

.stat-label {
  font-size: 1rem;
  color: #6c757d;
}

.contact-section {
  text-align: center;
}

.contact-section h4 {
  font-size: 1.5rem;
  color: #2c3e50;
  margin-bottom: 25px;
  font-weight: 600;
}

.contact-links {
  display: flex;
  justify-content: center;
  gap: 30px;
  flex-wrap: wrap;
}

.contact-link {
  padding: 12px 24px;
  background: white;
  border: 2px solid #2c3e50;
  border-radius: 6px;
  text-decoration: none;
  color: #2c3e50;
  font-weight: 500;
  transition: all 0.3s ease;
}

.contact-link:hover {
  background-color: #2c3e50;
  color: white;
}

/* Responsive Design */
@media (max-width: 768px) {
  .about-section {
    padding: 40px 0;
  }
  
  .container {
    padding: 0 15px;
  }
  
  .section-title {
    font-size: 2rem;
  }
  
  .profile-section {
    flex-direction: column;
    text-align: center;
  }
  
  .profile-image {
    margin-right: 0;
    margin-bottom: 25px;
  }
  
  .profile-image img {
    width: 120px;
    height: 120px;
  }
  
  .skills-grid {
    grid-template-columns: 1fr;
  }
  
  .stats-section {
    grid-template-columns: repeat(2, 1fr);
    padding: 25px;
  }
  
  .contact-links {
    flex-direction: column;
    align-items: center;
  }
  
  .contact-link {
    width: 200px;
  }
}

@media (max-width: 480px) {
  .stats-section {
    grid-template-columns: 1fr;
  }
}
</style>