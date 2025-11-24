<template>
  <div class="home-page">
    
    <section class="hero-section">
      <div class="hero-background"></div>
      <div class="hero-content">
        <h1 class="hero-title">
          Merhaba, ben <span class="hero-highlight">Emre</span>.<br>
          Dijital dünyayı kodluyorum.
        </h1>
        <p class="hero-text">
          Vue.js, ASP.NET Core ve MSSQL odaklı web çözümleri geliştiren bir yazılım mühendisiyim. İhtiyaca yönelik, ölçeklenebilir ve kullanıcı dostu uygulamalar tasarlıyorum.
        </p>
        <div class="hero-buttons">
          <router-link to="/project" class="btn btn-primary">
            <span>Projelerim</span>
            <span class="btn-arrow">→</span>
          </router-link>
          <router-link to="/contact" class="btn btn-primary">
            <span>İletişim</span>
            <span class="btn-arrow">→</span>
          </router-link>
          <a href="/path/to/your/cv.pdf" download class="btn btn-secondary">
            <span>CV'mi İndir</span>
            <span class="btn-icon">↓</span>
          </a>
        </div>
      </div>
    </section>

    <section class="section about-me-section">
      <div class="section-container">
        <h2 class="section-heading">Ben Kimim?</h2>
        <div class="about-me-content">
          <p>
            Sakarya Uygulamalı Bilimler Üniversitesi Bilgisayar Mühendisliği bölümünden mezun oldum. Yazılıma olan tutkum, beni sürekli yeni teknolojiler öğrenmeye ve en iyi uygulamaları benimsemeye yönlendiriyor.
          </p>
          <p>
            <span class="text-highlight">ASP.NET Core</span> ile güçlü ve güvenli backend servisleri oluştururken, <span class="text-highlight">Vue.js</span> ile dinamik ve modern kullanıcı arayüzleri geliştiriyorum. Veri yönetiminde <span class="text-highlight">MSSQL</span> kullanıyor, projelerimde <span class="text-highlight">REST API</span> ve <span class="text-highlight">Clean Architecture</span> prensiplerine bağlı kalıyorum. Benim için yazılım, sadece kod yazmaktan çok, problem çözme sanatıdır.
          </p>
        </div>
      </div>
    </section>

    <section class="section experiences-section">
      <div class="section-container">
        <h2 class="section-heading">Deneyimlerim</h2>
        <div class="accordion-container">
          <AllExperience />
        </div>
      </div>
    </section>

    <RecentProjects />

    <section class="section services-section">
      <div class="section-container">
        <h2 class="section-heading">Neler Yapıyorum?</h2>
        <div class="services-grid">
          <div class="service-card">
            <div class="service-icon">🌐</div>
            <h3 class="service-title">Web Geliştirme</h3>
            <p class="service-description">İhtiyaca özel, modern ve duyarlı web siteleri ve uygulamaları tasarlayıp geliştiriyorum.</p>
          </div>
          <div class="service-card">
            <div class="service-icon">⚡</div>
            <h3 class="service-title">API Geliştirme</h3>
            <p class="service-description">Güvenli ve dokümante edilmiş RESTful API'ler oluşturarak, uygulamalarınızın altyapısını kuruyorum.</p>
          </div>
          <div class="service-card">
            <div class="service-icon">🗄️</div>
            <h3 class="service-title">Veritabanı Yönetimi</h3>
            <p class="service-description">Uygulamalar için veritabanı şeması tasarlıyor ve MSSQL üzerinde performanslı çözümler üretiyorum.</p>
          </div>
          <div class="service-card">
            <div class="service-icon">💡</div>
            <h3 class="service-title">Teknik Danışmanlık</h3>
            <p class="service-description">Proje planlama, mimari tasarım ve teknoloji seçimi gibi konularda yol gösteriyorum.</p>
          </div>
        </div>
      </div>
    </section>

    <section class="section statistics-section">
      <div class="section-container">
        <h2 class="section-heading">Sayılarla Ben</h2>
        <div class="statistics-grid">
          <div class="statistic-item">
            <span class="statistic-number">20+</span>
            <p class="statistic-label">Tamamlanan Proje</p>
            <div class="statistic-bar"></div>
          </div>
          <div class="statistic-item">
            <span class="statistic-number">5+</span>
            <p class="statistic-label">Uzman Olduğum Teknoloji</p>
            <div class="statistic-bar"></div>
          </div>
          <div class="statistic-item">
            <span class="statistic-number">3</span>
            <p class="statistic-label">Deneyim Yılı</p>
            <div class="statistic-bar"></div>
          </div>
          <div class="statistic-item">
            <span class="statistic-number">100%</span>
            <p class="statistic-label">Problem Çözme Odaklılık</p>
            <div class="statistic-bar"></div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import RecentProjects from '@/components/Home/RecentProjects.vue';
import AllExperience from '@/components/Experience/allExperience.vue'; 

export default {
  name: 'HomePage',
  components: {
    RecentProjects,
    AllExperience
  }
};
</script>

<style scoped>
.home-page {
  font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #1a1a2e;
  line-height: 1.7;
}

/* Hero Section */
.hero-section {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  overflow: hidden;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.hero-background {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: 
    radial-gradient(circle at 20% 50%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
    radial-gradient(circle at 80% 80%, rgba(255, 255, 255, 0.1) 0%, transparent 50%);
  animation: backgroundMove 20s ease-in-out infinite;
}

@keyframes backgroundMove {
  0%, 100% { transform: scale(1) rotate(0deg); }
  50% { transform: scale(1.1) rotate(5deg); }
}

.hero-content {
  position: relative;
  z-index: 1;
  text-align: center;
  max-width: 900px;
}

.hero-title {
  font-size: 3rem;
  font-weight: 900;
  color: white;
  line-height: 1.2;
  margin-bottom: 1.5rem;
  letter-spacing: -0.02em;
  animation: fadeInUp 0.8s ease-out;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.hero-highlight {
  background: linear-gradient(135deg, #00d4ff, #00ff88);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  position: relative;
  display: inline-block;
}

.hero-highlight::after {
  content: '';
  position: absolute;
  bottom: -5px;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #00d4ff, #00ff88);
  border-radius: 2px;
  animation: highlightExpand 1s ease-out 0.5s;
  transform-origin: left;
}

@keyframes highlightExpand {
  from { transform: scaleX(0); }
  to { transform: scaleX(1); }
}

.hero-text {
  font-size: 1.1rem;
  color: rgba(255, 255, 255, 0.95);
  line-height: 1.8;
  margin-bottom: 2.5rem;
  animation: fadeInUp 0.8s ease-out 0.2s backwards;
}

.hero-buttons {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 1rem;
  animation: fadeInUp 0.8s ease-out 0.4s backwards;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.95rem;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  text-decoration: none;
  position: relative;
  overflow: hidden;
}

.btn::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 0;
  height: 0;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  transition: width 0.6s ease, height 0.6s ease;
}

.btn:hover::before {
  width: 300px;
  height: 300px;
}

.btn-primary {
  background: rgba(255, 255, 255, 0.2);
  color: white;
  backdrop-filter: blur(10px);
  border: 2px solid rgba(255, 255, 255, 0.3);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}

.btn-primary:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: translateY(-3px);
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.2);
}

.btn-secondary {
  background: white;
  color: #667eea;
  border: 2px solid transparent;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}

.btn-secondary:hover {
  background: #f8f9ff;
  transform: translateY(-3px);
  box-shadow: 0 12px 40px rgba(102, 126, 234, 0.3);
}

.btn-arrow,
.btn-icon {
  font-size: 1.3rem;
  transition: transform 0.3s ease;
}

.btn:hover .btn-arrow {
  transform: translateX(5px);
}

.btn:hover .btn-icon {
  transform: translateY(3px);
}

/* Sections */
.section {
  padding: 4rem 2rem;
}

.section-container {
  max-width: 1200px;
  margin: 0 auto;
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
  background: linear-gradient(90deg, #667eea, #764ba2);
  border-radius: 3px;
}

/* About Me Section */
.about-me-section {
  background: white;
}

.about-me-content {
  font-size: 1.05rem;
  color: #4a4a4a;
  line-height: 1.8;
  max-width: 800px;
  margin: 0 auto;
}

.about-me-content p {
  margin-bottom: 1.5rem;
  text-align: justify;
}

.text-highlight {
  font-weight: 700;
  color: #667eea;
  position: relative;
  padding: 0 0.2rem;
}

/* Experiences Section */
.experiences-section {
  background: #f8faff;
}

.accordion-container {
  max-width: 900px;
  margin: 0 auto;
  border-radius: 1rem;
  overflow: hidden;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.08);
}

/* Services Section */
.services-section {
  background: white;
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 2rem;
}

.service-card {
  background: linear-gradient(135deg, #f8faff 0%, #ffffff 100%);
  border-radius: 1.5rem;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.06);
  padding: 2rem;
  text-align: center;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  border: 1px solid rgba(102, 126, 234, 0.1);
  position: relative;
  overflow: hidden;
}

.service-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #667eea, #764ba2);
  transform: scaleX(0);
  transition: transform 0.4s ease;
}

.service-card:hover::before {
  transform: scaleX(1);
}

.service-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 20px 60px rgba(102, 126, 234, 0.15);
}

.service-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
  display: inline-block;
  transition: transform 0.4s ease;
}

.service-card:hover .service-icon {
  transform: scale(1.2) rotate(10deg);
}

.service-title {
  font-size: 1.3rem;
  font-weight: 700;
  color: #1a1a2e;
  margin-bottom: 0.75rem;
}

.service-description {
  color: #666;
  line-height: 1.7;
  font-size: 0.95rem;
}

/* Statistics Section */
.statistics-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.statistics-section .section-heading {
  color: white;
}

.statistics-section .section-heading::after {
  background: linear-gradient(90deg, #00d4ff, #00ff88);
}

.statistics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 2rem;
}

.statistic-item {
  text-align: center;
  padding: 2rem 1.5rem;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  border-radius: 1.5rem;
  border: 2px solid rgba(255, 255, 255, 0.2);
  transition: all 0.4s ease;
  position: relative;
  overflow: hidden;
}

.statistic-item::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.1), transparent);
  opacity: 0;
  transition: opacity 0.4s ease;
}

.statistic-item:hover::before {
  opacity: 1;
}

.statistic-item:hover {
  transform: translateY(-10px);
  border-color: rgba(255, 255, 255, 0.4);
}

.statistic-number {
  font-size: 2.75rem;
  font-weight: 900;
  display: block;
  margin-bottom: 0.5rem;
  background: linear-gradient(135deg, #00d4ff, #00ff88);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.statistic-label {
  font-size: 0.95rem;
  font-weight: 600;
  color: rgba(255, 255, 255, 0.95);
  margin-bottom: 0.75rem;
}

.statistic-bar {
  width: 0;
  height: 4px;
  background: linear-gradient(90deg, #00d4ff, #00ff88);
  border-radius: 2px;
  margin: 0 auto;
  animation: barExpand 1.5s ease-out forwards;
}

@keyframes barExpand {
  to { width: 60%; }
}

.statistic-item:hover .statistic-bar {
  animation: barPulse 1s ease-in-out infinite;
}

@keyframes barPulse {
  0%, 100% { width: 60%; }
  50% { width: 80%; }
}

/* Responsive */
@media (max-width: 968px) {
  .hero-title {
    font-size: 3rem;
  }
  
  .hero-text {
    font-size: 1.1rem;
  }
  
  .section-heading {
    font-size: 2.5rem;
  }
  
  .section {
    padding: 4rem 1.5rem;
  }
}

@media (max-width: 640px) {
  .hero-title {
    font-size: 2.5rem;
  }
  
  .hero-text {
    font-size: 1rem;
  }
  
  .hero-buttons {
    flex-direction: column;
    width: 100%;
  }
  
  .btn {
    width: 100%;
    justify-content: center;
  }
  
  .section-heading {
    font-size: 2rem;
  }
  
  .about-me-content {
    font-size: 1.05rem;
  }
  
  .services-grid,
  .statistics-grid {
    grid-template-columns: 1fr;
  }
}
</style>