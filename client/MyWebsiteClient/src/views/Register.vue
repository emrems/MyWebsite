<template>
  <div class="register-page">
    <div class="register-container">
      <div class="register-card">
        <h2 class="register-title">Kayıt Ol</h2>
        
        <form @submit.prevent="handleRegister" class="register-form">
          <div class="form-group">
            <label for="username">Kullanıcı Adı</label>
            <input
              id="username"
              v-model="registerForm.username"
              type="text"
              placeholder="Kullanıcı adınızı girin"
              required
            />
          </div>

          <div class="form-group">
            <label for="email">E-posta</label>
            <input
              id="email"
              v-model="registerForm.email"
              type="email"
              placeholder="E-posta adresinizi girin"
              required
            />
          </div>

          <div class="form-group">
            <label for="surname">Soyadı</label>
            <input
              id="surname"
              v-model="registerForm.surname"
              type="text"
              placeholder="Soyadınızı girin"
              required
            />
          </div>

          <div class="form-group">
            <label for="password">Şifre</label>
            <input
              id="password"
              v-model="registerForm.password"
              type="password"
              placeholder="Şifrenizi girin (min. 6 karakter)"
              required
            />
          </div>

          <div class="form-group">
            <label for="confirmPassword">Şifre Tekrar</label>
            <input
              id="confirmPassword"
              v-model="registerForm.confirmPassword"
              type="password"
              placeholder="Şifrenizi tekrar girin"
              required
            />
          </div>

          <div v-if="errorMessage" class="error-message">
            {{ errorMessage }}
          </div>

          <div v-if="successMessage" class="success-message">
            {{ successMessage }}
          </div>
          <button type="submit" class="btn-register" :disabled="isLoading">
            {{ isLoading ? 'İşleniyor...' : 'Kayıt Ol' }}
          </button>
        </form>

        <div class="login-link">
          Zaten hesabınız var mı? 
          <router-link to="/login">Giriş Yap</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from '@/services/ApiService';

export default {
  name: 'RegisterPage',
  data() {
    return {
      registerForm: {
        username: '',
        email: '',
        surname: '',
        password: '',
        confirmPassword: '',
      },
      errorMessage: '',
      successMessage: '',
      isLoading: false,
    };
  },
  methods: {
    async handleRegister() {
      this.errorMessage = '';
      this.successMessage = '';

      // 1. Şifrelerin Eşleşme Kontrolü
      if (this.registerForm.password !== this.registerForm.confirmPassword) {
        this.errorMessage = 'Şifreler eşleşmiyor!';
        return;
      }

      // 2. Şifre Uzunluğu Kontrolü
      if (this.registerForm.password.length < 6) {
        this.errorMessage = 'Şifre en az 6 karakter olmalıdır!';
        return;
      }

      this.isLoading = true;

      try {
        // API isteği
        const result = await ApiService.create('users', { // Endpoint'i 'User' olarak kabul ettim.
          username: this.registerForm.username,
          email: this.registerForm.email,
          surName: this.registerForm.surname, // API Servisindeki alan adıyla uyumlu olmalı
          password: this.registerForm.password,
        });

        if (result.success) {
          // Başarılı senaryo
          this.successMessage = result.message || 'Kayıt başarılı! Giriş sayfasına yönlendiriliyorsunuz.';
          
          // 2 saniye sonra login sayfasına yönlendir
          setTimeout(() => {
            this.$router.push('/login');
          }, 2000);
        } else {
          // Başarısız senaryo (API'den gelen hata mesajı)
          this.errorMessage = result.message || 'Kayıt başarısız oldu. Lütfen bilgilerinizi kontrol edin.';
        }
      } catch (error) {
        // Beklenmeyen hata senaryosu (Ağ hatası vb.)
        this.errorMessage = 'Kayıt yapılırken beklenmeyen bir hata oluştu.';
      } finally {
        this.isLoading = false;
      }
    },
  },
};
</script>

<style scoped>
/* Daha sade ve küçük bir görünüm için stil revizyonu */
.register-page {
  /* Arka planı koruyoruz */
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 1rem;
  /* margin-top kaldırıldı, sayfayı ortalamak için */
}

.register-container {
  width: 100%;
  /* Max genişlik küçültüldü */
  max-width: 400px; 
}

.register-card {
  background: white;
  /* Daha yumuşak köşe */
  border-radius: 1rem; 
  /* Daha az padding */
  padding: 2rem; 
  /* Daha sade gölge */
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1); 
  animation: none; /* Animasyon kaldırıldı, sadeleştirmek için */
}

.register-title {
  font-size: 1.5rem; /* Başlık küçültüldü */
  font-weight: 700;
  color: #333;
  text-align: center;
  margin-bottom: 1.5rem; /* Boşluk azaltıldı */
}

.register-form {
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
  box-shadow: 0 0 0 1px #667eea; /* Gölge sadeleştirildi */
}

/* Mesaj ve Buton Stilleri Sadeleştirildi */
.error-message, .success-message {
  padding: 0.75rem;
  border-radius: 0.5rem;
  font-size: 0.85rem;
  text-align: center;
  margin-top: 0.5rem;
  /* Animasyonlar kaldırıldı, sadeleştirmek için */
  animation: none; 
}

.error-message {
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  color: #721c24;
}

.success-message {
  background: #d4edda;
  border: 1px solid #c3e6cb;
  color: #155724;
}

.btn-register {
  padding: 0.85rem; /* Padding azaltıldı */
  background: #667eea; /* Gradient yerine düz renk */
  color: white;
  border: none;
  border-radius: 0.5rem;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s, transform 0.2s;
  margin-top: 1rem; 
}

.btn-register:hover:not(:disabled) {
  background: #5a6edb;
  transform: none; /* Hover animasyonu kaldırıldı */
  box-shadow: none;
}

.btn-register:active:not(:disabled) {
  transform: none;
}

.btn-register:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.login-link {
  text-align: center;
  margin-top: 1rem;
  font-size: 0.9rem;
}

.login-link a {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
}

.login-link a:hover {
  text-decoration: underline;
}

/* Küçük ekranlar için medya sorgusu da sadeleştirildi */
@media (max-width: 640px) {
  .register-card {
    padding: 1.5rem;
  }
}
</style>