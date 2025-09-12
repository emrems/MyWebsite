<template>
  <section class="contact-section">
    <div class="container">
      <!-- Başlık -->
      <div class="section-header">
        <h2 class="section-title">İletişim</h2>
        <div class="title-underline"></div>
        <p class="section-subtitle">
          Benimle iletişime geçmek için aşağıdaki formu kullanabilir veya doğrudan bana ulaşabilirsiniz.
        </p>
      </div>

      <div class="contact-content">
        <!-- İletişim Formu -->
        <div class="contact-form-section">
          <h3>Mesaj Gönder</h3>
          <form @submit.prevent="submitForm" class="contact-form">
            <div class="form-group">
              <label for="name">Ad Soyad *</label>
              <input 
                type="text" 
                id="name" 
                v-model="form.name" 
                required
                :class="{ 'error': errors.name }"
              />
              <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
            </div>

            <div class="form-group">
              <label for="email">E-posta *</label>
              <input 
                type="email" 
                id="email" 
                v-model="form.email" 
                required
                :class="{ 'error': errors.email }"
              />
              <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
            </div>

         
            <div class="form-group">
              <label for="message">Mesaj *</label>
              <textarea 
                id="message" 
                v-model="form.message" 
                rows="6" 
                required
                :class="{ 'error': errors.message }"
              ></textarea>
              <span v-if="errors.message" class="error-message">{{ errors.message }}</span>
            </div>

            <button 
              type="submit" 
              class="submit-btn"
              :disabled="isSubmitting"
            >
              {{ isSubmitting ? 'Gönderiliyor...' : 'Mesaj Gönder' }}
            </button>

            <div v-if="submitMessage" class="submit-message" :class="submitMessageType">
              {{ submitMessage }}
            </div>
          </form>
        </div>

        <!-- İletişim Bilgileri -->
        <div class="contact-info-section">
          <h3>İletişim Bilgileri</h3>
          
          <div class="contact-info">
            <div class="info-item">
              <div class="info-icon">📧</div>
              <div class="info-content">
                <h4>E-posta</h4>
                <a href="mailto:email@domain.com">email@domain.com</a>
              </div>
            </div>

            <div class="info-item">
              <div class="info-icon">📱</div>
              <div class="info-content">
                <h4>Telefon</h4>
                <a href="tel:+905551234567">+90 555 123 45 67</a>
              </div>
            </div>

            <div class="info-item">
              <div class="info-icon">📍</div>
              <div class="info-content">
                <h4>Konum</h4>
                <p>İstanbul, Türkiye</p>
              </div>
            </div>

            <div class="info-item">
              <div class="info-icon">🕐</div>
              <div class="info-content">
                <h4>Çalışma Saatleri</h4>
                <p>Pazartesi - Cuma<br>09:00 - 18:00</p>
              </div>
            </div>
          </div>

          <!-- Sosyal Medya -->
          <div class="social-media">
            <h4>Sosyal Medya</h4>
            <div class="social-links">
              <a 
                v-for="social in socialLinks" 
                :key="social.platform"
                :href="social.url" 
                class="social-link"
                target="_blank"
                rel="noopener noreferrer"
              >
                {{ social.platform }}
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import ApiService from '@/services/ApiService';
export default {
  name: 'ContactPage',
  data() {
    return {
      form: {
        name: '',
        email: '',
        message: ''
      },
      errors: {},
      isSubmitting: false,
      submitMessage: '',
      submitMessageType: '',
      
      socialLinks: [
        { platform: "LinkedIn", url: "https://linkedin.com/in/kullaniciadi" },
        { platform: "GitHub", url: "https://github.com/kullaniciadi" },
        { platform: "Twitter", url: "https://twitter.com/kullaniciadi" },
        { platform: "Instagram", url: "https://instagram.com/kullaniciadi" }
      ]
    }
  },
  methods: {
    validateForm() {
      this.errors = {};
      
      if (!this.form.name.trim()) {
        this.errors.name = 'Ad soyad gerekli';
      }
      
      if (!this.form.email.trim()) {
        this.errors.email = 'E-posta gerekli';
      } else if (!this.isValidEmail(this.form.email)) {
        this.errors.email = 'Geçerli bir e-posta adresi girin';
      }
      
      if (!this.form.message.trim()) {
        this.errors.message = 'Mesaj gerekli';
      } else if (this.form.message.trim().length < 10) {
        this.errors.message = 'Mesaj en az 10 karakter olmalı';
      }
      
      return Object.keys(this.errors).length === 0;
    },
    
    isValidEmail(email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(email);
    },
    
    async submitForm() {
      if (!this.validateForm()) {
        return;
      }
      
      this.isSubmitting = true;
      this.submitMessage = '';
      
      try {
       
        await this.CreateFormSubmission();
        
       // this.submitMessage = 'Mesajınız başarıyla gönderildi! En kısa sürede size dönüş yapacağım.';
        this.submitMessageType = 'success';
        
   
        this.form = {
          name: '',
          email: '',
          subject: '',
          message: ''
        };
        
      } catch (error) {
        this.submitMessage = 'Mesaj gönderilirken bir hata oluştu. Lütfen tekrar deneyin.';
        this.submitMessageType = 'error';
      } finally {
        this.isSubmitting = false;
      }
    },
    
    // Form gönderimini simüle et (gerçek implementasyon için kaldırılabilir)
    async CreateFormSubmission() {
      try {
        const data = {
            Name:this.form.name,
            Email:this.form.email,
            Content: this.form.message
        }
        const response = await ApiService.post('message/send',data);
        console.log("messageden dönen cevap",response.data);
        this.submitMessage = response.data;
        
      } catch (error) {
        console.log("hata",error);
        this.errors = error;
      }
    }
  }
}
</script>

<style scoped>
.contact-section {
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
  margin: 0 auto 20px auto;
}

.section-subtitle {
  font-size: 1.1rem;
  color: #6c757d;
  max-width: 600px;
  margin: 0 auto;
  line-height: 1.6;
}

.contact-content {
  display: grid;
  grid-template-columns: 1fr 400px;
  gap: 60px;
  align-items: start;
}

.contact-form-section h3,
.contact-info-section h3 {
  font-size: 1.5rem;
  color: #2c3e50;
  margin-bottom: 30px;
  font-weight: 600;
}

.contact-form {
  background: white;
  padding: 40px;
  border: 1px solid #e9ecef;
  border-radius: 8px;
}

.form-group {
  margin-bottom: 25px;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #2c3e50;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 12px 15px;
  border: 1px solid #e9ecef;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
  font-family: inherit;
}

.form-group input:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #2c3e50;
}

.form-group input.error,
.form-group textarea.error {
  border-color: #dc3545;
}

.error-message {
  color: #dc3545;
  font-size: 0.875rem;
  margin-top: 5px;
  display: block;
}

.form-group textarea {
  resize: vertical;
  min-height: 120px;
}

.submit-btn {
  background-color: #2c3e50;
  color: white;
  padding: 15px 30px;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s ease;
  width: 100%;
}

.submit-btn:hover:not(:disabled) {
  background-color: #1a252f;
}

.submit-btn:disabled {
  background-color: #6c757d;
  cursor: not-allowed;
}

.submit-message {
  margin-top: 20px;
  padding: 15px;
  border-radius: 6px;
  text-align: center;
  font-weight: 500;
}

.submit-message.success {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.submit-message.error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.contact-info {
  background: white;
  padding: 40px;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  margin-bottom: 30px;
}

.info-item {
  display: flex;
  align-items: flex-start;
  margin-bottom: 30px;
}

.info-item:last-child {
  margin-bottom: 0;
}

.info-icon {
  font-size: 1.5rem;
  margin-right: 15px;
  margin-top: 2px;
}

.info-content h4 {
  color: #2c3e50;
  font-size: 1.1rem;
  font-weight: 600;
  margin-bottom: 5px;
}

.info-content p {
  color: #6c757d;
  margin: 0;
  line-height: 1.5;
}

.info-content a {
  color: #2c3e50;
  text-decoration: none;
  transition: color 0.3s ease;
}

.info-content a:hover {
  color: #1a252f;
  text-decoration: underline;
}

.social-media {
  background: white;
  padding: 30px;
  border: 1px solid #e9ecef;
  border-radius: 8px;
}

.social-media h4 {
  color: #2c3e50;
  font-size: 1.2rem;
  font-weight: 600;
  margin-bottom: 20px;
}

.social-links {
  display: flex;
  flex-wrap: wrap;
  gap: 15px;
}

.social-link {
  padding: 10px 20px;
  background: #f8f9fa;
  border: 1px solid #e9ecef;
  border-radius: 6px;
  text-decoration: none;
  color: #2c3e50;
  font-weight: 500;
  transition: all 0.3s ease;
  text-align: center;
  flex: 1;
  min-width: 80px;
}

.social-link:hover {
  background-color: #2c3e50;
  color: white;
}

/* Responsive Design */
@media (max-width: 992px) {
  .contact-content {
    grid-template-columns: 1fr;
    gap: 40px;
  }
  
  .contact-info-section {
    order: -1;
  }
}

@media (max-width: 768px) {
  .contact-section {
    padding: 40px 0;
  }
  
  .container {
    padding: 0 15px;
  }
  
  .section-title {
    font-size: 2rem;
  }
  
  .contact-form,
  .contact-info {
    padding: 25px;
  }
  
  .social-media {
    padding: 20px;
  }
  
  .social-links {
    flex-direction: column;
  }
  
  .social-link {
    flex: none;
  }
}
</style>