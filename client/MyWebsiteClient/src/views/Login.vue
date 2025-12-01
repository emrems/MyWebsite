<!-- @ts-nocheck -->
<!-- eslint-disable -->
<template>
  <div class="login-page">
    <div class="login-container">
      <div class="login-card">
        <h2 class="login-title">Giriş Yap</h2>
        
        <form @submit.prevent="handleLogin" class="login-form">
          <div class="form-group">
            <label for="username">Kullanıcı Adı</label>
            <input
              id="username"
              v-model="loginForm.username"
              type="text"
              placeholder="Kullanıcı adınızı girin"
              required
            />
          </div>

          <div class="form-group">
            <label for="password">Şifre</label>
            <input
              id="password"
              v-model="loginForm.password"
              type="password"
              placeholder="Şifrenizi girin"
              required
            />
          </div>

          <div v-if="errorMessage" class="error-message">
            {{ errorMessage }}
          </div>

          <button type="submit" class="btn-login" :disabled="isLoading">
            {{ isLoading ? 'Giriş yapılıyor...' : 'Giriş Yap' }}
          </button>
        </form>

        <div class="register-link">
          Hesabınız yok mu? 
          <router-link to="/register">Kayıt Ol</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'LoginPage',
  data() {
    return {
      loginForm: {
        username: '',
        password: '',
      },
      errorMessage: '',
      isLoading: false,
    };
  },
  methods: {
    async handleLogin() {
      this.errorMessage = '';
      this.isLoading = true;

      try {
        const result = await this.$store.dispatch('auth/login', {
          username: this.loginForm.username,
          password: this.loginForm.password,
        });

        if (result.success) {
          // Başarılı giriş - Ana sayfaya yönlendir
          this.$router.push('/');
        } else {
          this.errorMessage = result.message || 'Giriş başarısız oldu. Lütfen bilgilerinizi kontrol edin.';
        }
      } catch (error) {
        this.errorMessage = 'Giriş yapılırken bir hata oluştu';
      } finally {
        this.isLoading = false;
      }
    },
  },
};
</script>

<style scoped>
/* RegisterPage ile uyumlu, daha sade ve küçük tasarım */
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 1rem;
}

.login-container {
  width: 100%;
  max-width: 400px; /* Max genişlik küçültüldü */
}

.login-card {
  background: white;
  border-radius: 1rem; 
  padding: 2rem; /* Padding azaltıldı */
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1); 
  animation: none;
}

.login-title {
  font-size: 1.5rem; /* Başlık küçültüldü */
  font-weight: 700;
  color: #333;
  text-align: center;
  margin-bottom: 1.5rem; 
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem; /* Boşluklar azaltıldı */
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.form-group label {
  font-weight: 500;
  color: #555;
  font-size: 0.9rem;
}

.form-group input {
  padding: 0.75rem 1rem; /* Padding azaltıldı */
  border: 1px solid #ccc; /* Daha ince border */
  border-radius: 0.5rem; /* Köşe yuvarlaklığı azaltıldı */
  font-size: 0.95rem;
  transition: border-color 0.2s;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 1px #667eea;
}

.error-message {
  padding: 0.75rem;
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-radius: 0.5rem;
  color: #721c24;
  font-size: 0.85rem;
  text-align: center;
  margin-top: 0.5rem;
  animation: none;
}

.btn-login {
  padding: 0.85rem; 
  background: #667eea; 
  color: white;
  border: none;
  border-radius: 0.5rem;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s;
  margin-top: 1rem; 
}

.btn-login:hover:not(:disabled) {
  background: #5a6edb;
  transform: none;
  box-shadow: none;
}

.btn-login:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.register-link {
  text-align: center;
  margin-top: 1.5rem;
  color: #666;
  font-size: 0.9rem;
}

.register-link a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
}

.register-link a:hover {
  text-decoration: underline;
}

@media (max-width: 640px) {
  .login-card {
    padding: 1.5rem;
  }
}
</style>