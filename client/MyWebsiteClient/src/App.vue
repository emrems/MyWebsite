<template>
  <header class="navbar">
    <div class="wrapper">
      <nav class="nav-links">
        <!-- Giriş yapmamış kullanıcılar ve User rolü için temel linkler -->
        <template v-if="!isAuthenticated || (currentUser && currentUser.role === 'User')">
          <RouterLink to="/" class="nav-item">Ana Sayfa</RouterLink>
          <RouterLink to="/about" class="nav-item">Hakkımda</RouterLink>
          <RouterLink to="/contact" class="nav-item">İletişim</RouterLink>
          <RouterLink to="/articles" class="nav-item">Blog</RouterLink>
          <RouterLink to="/project" class="nav-item">Projeler</RouterLink>
        </template>

        <!-- Giriş yapmış kullanıcılar için -->
        <template v-if="isAuthenticated">
          <!-- Admin rolünde ise sadece Admin Paneli linki -->
          <RouterLink 
            v-if="currentUser && currentUser.role === 'Admin'" 
            to="/admin" 
            class="nav-item admin-link"
          >
            Admin Paneli
          </RouterLink>
          
          <!-- Tüm giriş yapmış kullanıcılar için Çıkış Yap butonu -->
          <button @click="handleLogout" class="logout-btn">
            Çıkış Yap
          </button>
        </template>

        <!-- Giriş yapmamış kullanıcılar için -->
        <RouterLink v-if="!isAuthenticated" to="/login" class="login-btn">
          Giriş Yap / Kayıt Ol
        </RouterLink>
      </nav>
    </div>
  </header>

  <!-- Admin kullanıcısı ana sayfadaysa Admin Paneline yönlendir -->
  <RouterView v-if="!shouldRedirectToAdmin" />
  <Notification />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import Notification from '@/components/Notification.vue';

export default {
  name: 'App',
  components: {
    Notification,
  },
  computed: {
    ...mapGetters('auth', ['isAuthenticated', 'currentUser']),
    // Admin kullanıcısı ana sayfadaysa yönlendirme yap
    shouldRedirectToAdmin() {
      if (this.isAuthenticated && 
          this.currentUser && 
          this.currentUser.role === 'Admin' && 
          this.$route.path === '/') {
        this.$router.push('/admin');
        return true;
      }
      return false;
    }
  },
  methods: {
    ...mapActions('auth', ['logout', 'checkAuth']),

    handleLogout() {
      this.logout();
      if (this.$route.path !== '/login') {
        this.$router.push('/login');
      }
    },
  },
  mounted() {
    this.checkAuth();
    
    // Sayfa yüklendiğinde admin kontrolü yap
    if (this.isAuthenticated && 
        this.currentUser && 
        this.currentUser.role === 'Admin' && 
        this.$route.path === '/') {
      this.$router.push('/admin');
    }
  },
  watch: {
    // Route değişikliklerini izle
    '$route.path': {
      immediate: true,
      handler(newPath) {
        if (this.isAuthenticated && 
            this.currentUser && 
            this.currentUser.role === 'Admin' && 
            newPath === '/') {
          this.$router.push('/admin');
        }
      }
    },
    // Kullanıcı durumu değişikliklerini izle
    isAuthenticated: {
      immediate: true,
      handler(isAuth) {
        if (isAuth && 
            this.currentUser && 
            this.currentUser.role === 'Admin' && 
            this.$route.path === '/') {
          this.$router.push('/admin');
        }
      }
    }
  }
};
</script>

<style scoped>
/* Navbar stilleri */
.navbar {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  background-color: var(--color-background-soft);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 1rem 2rem;
  z-index: 1000;
  display: flex;
  justify-content: center;
  align-items: center;
}

.wrapper {
  display: flex;
  justify-content: center;
  width: 100%;
}

.nav-links {
  display: flex;
  gap: 1.5rem;
  align-items: center;
}

.nav-item {
  color: var(--color-text);
  text-decoration: none;
  font-size: 1rem;
  padding: 0.5rem 1rem;
  transition: all 0.3s ease;
  border-radius: 0.5rem;
}

.nav-item:hover {
  color: var(--color-primary);
  background-color: rgba(102, 126, 234, 0.1);
}

.nav-item.router-link-active {
  color: var(--color-primary);
  font-weight: 600;
}

/* Admin link özel stil */
.admin-link {
  color: #ff6b6b;
  font-weight: 700;
  font-size: 1.1rem;
}

.admin-link:hover {
  background-color: rgba(255, 107, 107, 0.1);
}

/* Login/Register butonu */
.login-btn {
  padding: 0.5rem 1.25rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  text-decoration: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.9rem;
  transition: all 0.3s ease;
  border: none;
  cursor: pointer;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
}

/* Logout butonu */
.logout-btn {
  padding: 0.5rem 1.25rem;
  background: #f44336;
  color: white;
  border: none;
  border-radius: 50px;
  font-weight: 600;
  font-size: 0.9rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.logout-btn:hover {
  background: #d32f2f;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(244, 67, 54, 0.4);
}

/* Responsive */
@media (max-width: 1024px) {
  .nav-links {
    gap: 1rem;
    font-size: 0.9rem;
  }

  .nav-item {
    padding: 0.4rem 0.8rem;
  }
}

@media (max-width: 768px) {
  .navbar {
    padding: 1rem;
  }

  .nav-links {
    gap: 0.5rem;
    flex-wrap: wrap;
    justify-content: center;
  }

  .nav-item {
    padding: 0.3rem 0.6rem;
    font-size: 0.85rem;
  }

  .login-btn,
  .logout-btn {
    padding: 0.4rem 1rem;
    font-size: 0.85rem;
  }
}

@media (min-width: 1024px) {
  .navbar {
    padding: 1rem 4rem;
  }
}
</style>