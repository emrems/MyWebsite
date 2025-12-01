<template>
  <div class="admin-layout">
    <!-- Admin için özel mini navigasyon -->
    <header class="admin-navbar">
      <div class="admin-nav-content">
        <div class="admin-nav-title">
          <h2>Admin Paneli</h2>
          <span class="user-info">Hoş geldiniz, {{ currentUser.username }}</span>
        </div>
        <div class="admin-nav-actions">
          <button @click="handleLogout" class="admin-logout-btn">
            Çıkış Yap
          </button>
        </div>
      </div>
    </header>

    <main class="admin-main">
      <div class="admin-container">
        <h1>Admin Kontrol Paneli</h1>
        
        <div class="admin-welcome">
          <p><strong>Kullanıcı:</strong> {{ currentUser.username }}</p>
          <p><strong>Rol:</strong> <span class="role-badge">{{ currentUser.role }}</span></p>
        </div>

        <div class="admin-actions">
          <div class="action-card" @click="navigateToSection('users')">
            <h3>👥 Kullanıcı Yönetimi</h3>
            <p>Kullanıcıları görüntüle ve yönet</p>
          </div>
          
          <div class="action-card" @click="navigateToSection('categories')">
            <h3>📁 Kategori Yönetimi</h3>
            <p>Kategorileri düzenle ve organize et</p>
          </div>
          
          <div class="action-card" @click="navigateToSection('articles')">
            <h3>📝 Makale Yönetimi</h3>
            <p>Blog makalelerini yönet</p>
          </div>
          
          <div class="action-card" @click="navigateToSection('projects')">
            <h3>💼 Proje Yönetimi</h3>
            <p>Projeleri düzenle</p>
          </div>
          
          <div class="action-card" @click="navigateToSection('settings')">
            <h3>⚙️ Sistem Ayarları</h3>
            <p>Site ayarlarını yapılandır</p>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'AdminPage',
  computed: {
    ...mapGetters('auth', ['currentUser']),
  },
  methods: {
    ...mapActions('auth', ['logout']),
    
    handleLogout() {
      this.logout();
      this.$router.push('/login');
    },
    
    navigateToSection(section) {
      // İlgili bölüme yönlendirme
      this.$router.push(`/admin/${section}`);
    }
  },
  mounted() {
    // Sayfa yüklendiğinde admin yetkisini kontrol et
    if (this.currentUser.role !== 'Admin') {
      this.$router.push('/');
    }
  },
};
</script>

<style scoped>
.admin-layout {
  min-height: 100vh;
  background: #f8f9fa;
}

.admin-navbar {
  background: #2c3e50;
  color: white;
  padding: 1rem 2rem;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.admin-nav-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1400px;
  margin: 0 auto;
}

.admin-nav-title h2 {
  margin: 0;
  color: white;
}

.user-info {
  color: #bdc3c7;
  font-size: 0.9rem;
}

.admin-logout-btn {
  padding: 0.5rem 1.5rem;
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.3s;
}

.admin-logout-btn:hover {
  background: #c0392b;
}

.admin-main {
  padding: 2rem;
}

.admin-container {
  max-width: 1400px;
  margin: 0 auto;
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.admin-container h1 {
  color: #2c3e50;
  margin-bottom: 1rem;
  text-align: center;
}

.admin-welcome {
  background: #ecf0f1;
  padding: 1rem;
  border-radius: 5px;
  margin-bottom: 2rem;
}

.role-badge {
  background: #e74c3c;
  color: white;
  padding: 0.2rem 0.5rem;
  border-radius: 3px;
  font-size: 0.8rem;
}

.admin-actions {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}

.action-card {
  padding: 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  text-align: center;
}

.action-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
}

.action-card h3 {
  margin-bottom: 0.5rem;
  font-size: 1.2rem;
}

.action-card p {
  margin: 0;
  opacity: 0.9;
  font-size: 0.9rem;
}

@media (max-width: 768px) {
  .admin-nav-content {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }
  
  .admin-main {
    padding: 1rem;
  }
  
  .admin-container {
    padding: 1rem;
  }
  
  .admin-actions {
    grid-template-columns: 1fr;
  }
}
</style>