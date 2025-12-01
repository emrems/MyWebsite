<template>
  <div class="not-found-page">
    <div class="content-container">
      <h1 class="title">404</h1>
      <h2 class="subtitle">Sayfa Bulunamadı</h2>
      <p class="description">
        Aradığınız sayfa mevcut değil veya taşınmış olabilir. Lütfen ana sayfaya dönün.
      </p>
      <router-link to="/" class="home-button">
        Ana Sayfaya Git
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: 'NotFound',
};
</script>

<style scoped>
.not-found-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f7f9fc;
  color: #333;
  text-align: center;
  padding: 20px;
}

.content-container {
  max-width: 600px;
  background: white;
  padding: 40px;
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
  transform: translateY(0); /* Navbar fixed olduğu için gerekirse ayarlama yapabilirsiniz */
}

.title {
  font-size: 6rem;
  font-weight: 900;
  color: #ff6b6b; /* Kırmızı renk tonu */
  margin-bottom: 0.5rem;
  line-height: 1;
  animation: pulse 1.5s infinite alternate;
}

.subtitle {
  font-size: 1.8rem;
  font-weight: 700;
  color: #555;
  margin-bottom: 1rem;
}

.description {
  font-size: 1.1rem;
  color: #777;
  margin-bottom: 2rem;
}

.home-button {
  display: inline-block;
  padding: 12px 25px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  text-decoration: none;
  border-radius: 50px;
  font-weight: 600;
  transition: all 0.3s ease;
  box-shadow: 0 5px 15px rgba(102, 126, 234, 0.3);
}

.home-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(102, 126, 234, 0.4);
}

@keyframes pulse {
  from {
    opacity: 1;
    transform: scale(1);
  }
  to {
    opacity: 0.8;
    transform: scale(1.05);
  }
}
</style>