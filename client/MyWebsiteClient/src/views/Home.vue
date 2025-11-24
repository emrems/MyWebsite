<template>
  <div class="home-page">
    
    <section class="hero-section">
      <h1 class="hero-title">
        Merhaba, ben <span class="hero-highlight">Emre</span>.<br>
        Dijital dünyayı kodluyorum.
      </h1>
      <p class="hero-text">
        Vue.js, ASP.NET Core ve MSSQL odaklı web çözümleri geliştiren bir yazılım mühendisiyim. İhtiyaca yönelik, ölçeklenebilir ve kullanıcı dostu uygulamalar tasarlıyorum.
      </p>
      <div class="hero-buttons">
        <router-link to="/project" class="btn btn-primary">
          Projelerim
        </router-link>
        <router-link to="/contact" class="btn btn-primary">
          İletişim
        </router-link>
        <a href="/path/to/your/cv.pdf" download class="btn btn-secondary">
          CV'mi İndir
        </a>
      </div>
    </section>

    
    <section class="section about-me-section">
      <h2 class="section-heading">Ben Kimim?</h2>
      <div class="about-me-content">
        <p>
          Sakarya Uygulamalı Bilimler Üniversitesi Bilgisayar Mühendisliği bölümünden mezun oldum. Yazılıma olan tutkum, beni sürekli yeni teknolojiler öğrenmeye ve en iyi uygulamaları benimsemeye yönlendiriyor.
        </p>
        <p>
          <span class="text-highlight">ASP.NET Core</span> ile güçlü ve güvenli backend servisleri oluştururken, <span class="text-highlight">Vue.js</span> ile dinamik ve modern kullanıcı arayüzleri geliştiriyorum. Veri yönetiminde <span class="text-highlight">MSSQL</span> kullanıyor, projelerimde <span class="text-highlight">REST API</span> ve <span class="text-highlight">Clean Architecture</span> prensiplerine bağlı kalıyorum. Benim için yazılım, sadece kod yazmaktan çok, problem çözme sanatıdır.
        </p>
      </div>
    </section>

    <RecentProjects />
    
    
    <section class="section services-section">
      <h2 class="section-heading">Neler Yapıyorum?</h2>
      <div class="services-grid">
        <div class="service-card">
          <h3 class="service-title">Web Geliştirme</h3>
          <p class="service-description">İhtiyaca özel, modern ve duyarlı web siteleri ve uygulamaları tasarlayıp geliştiriyorum.</p>
        </div>
        <div class="service-card">
          <h3 class="service-title">API Geliştirme</h3>
          <p class="service-description">Güvenli ve dokümante edilmiş RESTful API'ler oluşturarak, uygulamalarınızın altyapısını kuruyorum.</p>
        </div>
        <div class="service-card">
          <h3 class="service-title">Veritabanı Yönetimi</h3>
          <p class="service-description">Uygulamalar için veritabanı şeması tasarlıyor ve MSSQL üzerinde performanslı çözümler üretiyorum.</p>
        </div>
        <div class="service-card">
          <h3 class="service-title">Teknik Danışmanlık</h3>
          <p class="service-description">Proje planlama, mimari tasarım ve teknoloji seçimi gibi konularda yol gösteriyorum.</p>
        </div>
      </div>
    </section>

    
    <section class="section statistics-section">
      <h2 class="section-heading">Sayılarla Ben</h2>
      <div class="statistics-grid">
        <div class="statistic-item">
          <span class="statistic-number">20+</span>
          <p class="statistic-label">Tamamlanan Proje</p>
        </div>
        <div class="statistic-item">
          <span class="statistic-number">5+</span>
          <p class="statistic-label">Uzman Olduğum Teknoloji</p>
        </div>
        <div class="statistic-item">
          <span class="statistic-number">3</span>
          <p class="statistic-label">Deneyim Yılı</p>
        </div>
        <div class="statistic-item">
          <span class="statistic-number">100%</span>
          <p class="statistic-label">Problem Çözme Odaklılık</p>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import RecentProjects from '@/components/Home/RecentProjects.vue';
export default {
  name: 'HomePage',
  components: {
    RecentProjects
  }
};
</script>

<style scoped>
/* Genel Stil Resetleri ve Temel Ayarlar */
.home-page {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  color: #333;
  line-height: 1.6;
}

/* Hero Section */
.hero-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 6rem 2rem;
  background-color: #f8faff; /* Açık mavi ton */
  min-height: 80vh; /* Minimum ekran yüksekliği */
}

.hero-title {
  font-size: 3.5rem; /* Büyük başlık */
  font-weight: 800;
  color: #2c3e50;
  line-height: 1.2;
  margin-bottom: 1rem;
}

.hero-highlight {
  color: #007bff; /* Mavi vurgu */
}

.hero-text {
  font-size: 1.25rem;
  color: #555;
  max-width: 48rem; /* Metin genişliği */
  margin-bottom: 2.5rem;
}

.hero-buttons {
  display: flex;
  flex-wrap: wrap; /* Küçük ekranlarda alta geçsin */
  justify-content: center;
  gap: 1rem; /* Butonlar arası boşluk */
}

.btn {
  padding: 0.75rem 1.5rem;
  border-radius: 9999px; /* Tam yuvarlak */
  font-weight: 600;
  transition: all 0.3s ease;
  text-decoration: none;
  display: inline-block;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
}

.btn-primary:hover {
  background-color: #0056b3;
  transform: translateY(-2px);
  box-shadow: 0 6px 16px rgba(0, 123, 255, 0.4);
}

.btn-secondary {
  background-color: white;
  color: #4a4a4a;
  border: 1px solid #e0e0e0;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.btn-secondary:hover {
  background-color: #f2f2f2;
  transform: translateY(-2px);
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.btn-dark {
  background-color: #333;
  color: white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

.btn-dark:hover {
  background-color: #555;
  transform: translateY(-2px);
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3);
}

/* Genel Bölüm Stilleri */
.section {
  padding: 4rem 2rem;
  max-width: 72rem; /* Max genişlik */
  margin: 0 auto;
}

.section-heading {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  text-align: center;
  margin-bottom: 3rem;
  position: relative;
}

.section-heading::after {
  content: '';
  position: absolute;
  bottom: -10px;
  left: 50%;
  transform: translateX(-50%);
  width: 80px;
  height: 4px;
  background-color: #007bff;
  border-radius: 2px;
}


/* About Me Section */
.about-me-section {
  background-color: white;
}

.about-me-content {
  font-size: 1.1rem;
  color: #4a4a4a;
  max-width: 60rem;
  margin: 0 auto;
}

.about-me-content p {
  margin-bottom: 1.5rem;
}

.text-highlight {
  font-weight: bold;
  color: #007bff;
}

/* Services Section */
.services-section {
  background-color: white;
}

.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.service-card {
  background-color: #fdfdfd;
  border-radius: 1rem;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.06);
  padding: 2rem;
  text-align: center;
  transition: all 0.3s ease;
}

.service-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
}

.service-title {
  font-size: 1.4rem;
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.75rem;
}

.service-description {
  color: #666;
}

/* Statistics Section */
.statistics-section {
  background-color: #f8faff;
}

.statistics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 2rem;
  max-width: 60rem;
  margin: 0 auto;
}

.statistic-item {
  text-align: center;
  padding: 1.5rem;
  background-color: white;
  border-radius: 1rem;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
  transition: transform 0.3s ease;
}

.statistic-item:hover {
  transform: translateY(-5px);
}

.statistic-number {
  font-size: 3rem;
  font-weight: 800;
  color: #007bff;
  display: block;
  margin-bottom: 0.5rem;
}

.statistic-label {
  font-size: 1rem;
  color: #555;
  font-weight: 500;
}

/* Küçük Ekranlar İçin Medya Sorguları */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }
  .hero-text {
    font-size: 1rem;
  }
  .section-heading {
    font-size: 2rem;
  }
  .projects-grid, .services-grid, .statistics-grid {
    grid-template-columns: 1fr;
  } 
  .hero-buttons {
    flex-direction: column;
    gap: 0.75rem;
  }
}
</style>