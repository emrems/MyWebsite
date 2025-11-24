<template>
  <div v-if="isVisible" class="modal-overlay" @click="closeModal">
    <div class="share-modal" @click.stop>
      <h3>Paylaş</h3>
      
      <!-- Yorum Bölümü -->
      <div class="comment-section" v-if="showCommentSection">
        <h4>Yorum Yap</h4>
        <textarea 
          v-model="commentText" 
          placeholder="Yorumunuzu buraya yazın..."
          rows="4"
          class="comment-textarea"
          :disabled="isSubmitting"
        ></textarea>
        <div class="comment-actions">
          <button 
            @click="submitComment" 
            class="submit-comment-btn" 
            :disabled="!commentText.trim() || isSubmitting"
          >
            <span v-if="isSubmitting" class="button-loading">
              <i class="fas fa-spinner fa-spin"></i>
              Gönderiliyor...
            </span>
            <span v-else>
              <i class="fas fa-paper-plane"></i>
              Yorumu Gönder
            </span>
          </button>
          <button 
            @click="cancelComment" 
            class="cancel-comment-btn"
            :disabled="isSubmitting"
          >
            İptal
          </button>
        </div>
      </div>

      <div class="share-options">
        <button @click="shareOn('whatsapp')" class="share-option whatsapp">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.825 9.825 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413Z"/>
          </svg>
          WhatsApp
        </button>
        <button @click="shareOn('twitter')" class="share-option twitter">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M18.244 2.25h3.308l-7.227 8.26 8.502 11.24H16.17l-5.214-6.817L4.99 21.75H1.68l7.73-8.835L1.254 2.25H8.08l4.713 6.231zm-1.161 17.52h1.833L7.084 4.126H5.117z"/>
          </svg>
          Twitter
        </button>
        <button @click="shareOn('facebook')" class="share-option facebook">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/>
          </svg>
          Facebook
        </button>
        <button @click="shareOn('linkedin')" class="share-option linkedin">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433c-1.144 0-2.063-.926-2.063-2.065 0-1.138.92-2.063 2.063-2.063 1.14 0 2.064.925 2.064 2.063 0 1.139-.925 2.065-2.064 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z"/>
          </svg>
          LinkedIn
        </button>
        <button @click="copyLink" class="share-option copy-link">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M10.586 2.1a2 2 0 012.828 0l3.879 3.879a2 2 0 010 2.828l-1.586 1.586a2 2 0 01-2.828 0l-1.172-1.172a.5.5 0 00-.707.707l1.172 1.172a3 3 0 004.243 0l1.586-1.586a3 3 0 000-4.243L13.121.393a3 3 0 00-4.243 0L7.05 2.222a.5.5 0 00.707.707l1.828-1.829zm2.828 19.799a2 2 0 01-2.828 0L6.707 18.02a2 2 0 010-2.828l1.586-1.586a2 2 0 012.828 0l1.172 1.172a.5.5 0 00.707-.707l-1.172-1.172a3 3 0 00-4.243 0l-1.586 1.586a3 3 0 000 4.243l3.879 3.879a3 3 0 004.243 0l1.828-1.829a.5.5 0 00-.707-.707l-1.828 1.829zm.172-6.728a.5.5 0 00-.707-.707l-5.657 5.657a.5.5 0 10.707.707l5.657-5.657z"/>
          </svg>
          Linki Kopyala
        </button>
        <!-- Yorum Yap Butonu -->
        <button @click="showCommentSection = true" class="share-option comment">
          <svg viewBox="0 0 24 24" class="social-icon">
            <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2z"/>
          </svg>
          Yorum Yap
        </button>
      </div>
      <button @click="closeModal" class="close-button">Kapat</button>
    </div>
  </div>
</template>

<script>
import ApiService from '@/services/ApiService';

export default {
  name: 'ShareModal',
  
  props: {
    isVisible: {
      type: Boolean,
      default: false
    },
    title: {
      type: String,
      default: 'Paylaş'
    },
    shareText: {
      type: String,
      default: ''
    },
    articleId: {
      type: Number,
      default: null
    }
  },

  data() {
    return {
      showCommentSection: false,
      commentText: '',
      isSubmitting: false
    };
  },

  inject: ['notify'],

  methods: {
    closeModal() {
      this.showCommentSection = false;
      this.commentText = '';
      this.$emit('close');
    },

    shareOn(platform) {
      const url = encodeURIComponent(window.location.href);
      const text = encodeURIComponent(this.shareText);
      
      const shareUrls = {
        whatsapp: `https://wa.me/?text=${text}%20${url}`,
        twitter: `https://twitter.com/intent/tweet?url=${url}&text=${text}`,
        facebook: `https://www.facebook.com/sharer/sharer.php?u=${url}`,
        linkedin: `https://www.linkedin.com/sharing/share-offsite/?url=${url}`
      };
      
      window.open(shareUrls[platform], '_blank', 'width=600,height=400');
      
      // Paylaşım bildirimi
      this.showNotification({
        type: 'success',
        title: 'Paylaşıldı',
        message: `${this.getPlatformName(platform)}'da paylaşıldı`
      });
      
      this.closeModal();
    },

    getPlatformName(platform) {
      const names = {
        whatsapp: 'WhatsApp',
        twitter: 'Twitter',
        facebook: 'Facebook',
        linkedin: 'LinkedIn'
      };
      return names[platform] || platform;
    },

    async copyLink() {
      try {
        await navigator.clipboard.writeText(window.location.href);
        this.showNotification({
          type: 'success',
          title: 'Başarılı',
          message: 'Link panoya kopyalandı!'
        });
        this.$emit('link-copied');
        this.closeModal();
      } catch (err) {
        console.error('Kopyalama hatası:', err);
        this.showNotification({
          type: 'error',
          title: 'Hata',
          message: 'Link kopyalanırken bir hata oluştu'
        });
      }
    },

    async submitComment() {
      if (!this.commentText.trim()) {
        this.showNotification({
          type: 'warning',
          title: 'Uyarı',
          message: 'Lütfen yorumunuzu yazın'
        });
        return;
      }

      if (!this.articleId) {
        this.showNotification({
          type: 'error',
          title: 'Hata',
          message: 'Makale bilgisi bulunamadı'
        });
        return;
      }

      this.isSubmitting = true;

      try {
        const commentData = {
          content: this.commentText.trim(),
          articleId: this.articleId
        };

        const result = await ApiService.create('comment/CreateComment', commentData);

        if (result.success) {
          this.showNotification({
            type: 'success',
            title: 'Başarılı',
            message: 'Yorumunuz başarıyla gönderildi!'
          });
          
          this.$emit('comment-submitted', result.data);
          this.commentText = '';
          this.showCommentSection = false;
          this.closeModal();
        } else {
          this.showNotification({
            type: 'error',
            title: 'Hata',
            message: result.message || 'Yorum gönderilemedi'
          });
        }
      } catch (error) {
        console.error('Yorum gönderme hatası:', error);
        this.showNotification({
          type: 'error',
          title: 'Hata',
          message: 'Yorum gönderilirken bir hata oluştu. Lütfen tekrar deneyin.'
        });
      } finally {
        this.isSubmitting = false;
      }
    },

    cancelComment() {
      this.showCommentSection = false;
      this.commentText = '';
    },

    showNotification(options) {
      if (this.notify && typeof this.notify.show === 'function') {
        return this.notify.show(options);
      } else if (this.$notify && typeof this.$notify.show === 'function') {
        return this.$notify.show(options);
      }
      
      // Fallback to console if notification system not available
      console.log('Notification:', options);
    }
  },

  watch: {
    isVisible(newVal) {
      if (!newVal) {
        this.showCommentSection = false;
        this.commentText = '';
        this.isSubmitting = false;
      }
    }
  }
};
</script>


<style scoped>
.button-loading {
  display: flex;
  align-items: center;
  gap: 8px;
}

.submit-comment-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  animation: fadeIn 0.2s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.share-modal {
  background: white;
  padding: 30px;
  border-radius: 12px;
  max-width: 450px;
  width: 90%;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
  animation: slideUp 0.3s ease;
  max-height: 90vh;
  overflow-y: auto;
}

@keyframes slideUp {
  from {
    transform: translateY(30px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.share-modal h3 {
  margin-bottom: 20px;
  text-align: center;
  color: #2c3e50;
  font-size: 1.5rem;
}

/* Yorum Bölümü Stilleri */
.comment-section {
  margin-bottom: 25px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  border: 1px solid #e9ecef;
}

.comment-section h4 {
  margin-bottom: 15px;
  color: #2c3e50;
  font-size: 1.1rem;
}

.comment-textarea {
  width: 100%;
  padding: 12px;
  border: 1px solid #ddd;
  border-radius: 6px;
  resize: vertical;
  font-family: inherit;
  font-size: 0.95rem;
  margin-bottom: 15px;
}

.comment-textarea:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.1);
}

.comment-actions {
  display: flex;
  gap: 10px;
}

.submit-comment-btn {
  flex: 1;
  padding: 10px 15px;
  background: #3b82f6;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.3s ease;
}

.submit-comment-btn:hover:not(:disabled) {
  background: #2563eb;
}

.submit-comment-btn:disabled {
  background: #9ca3af;
  cursor: not-allowed;
}

.cancel-comment-btn {
  padding: 10px 15px;
  background: #6b7280;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.3s ease;
}

.cancel-comment-btn:hover {
  background: #4b5563;
}

.share-options {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
  margin-bottom: 20px;
}

.share-option {
  padding: 15px;
  border: 2px solid;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  font-size: 0.9rem;
  font-weight: 500;
}

.share-option:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.social-icon {
  width: 32px;
  height: 32px;
  fill: currentColor;
}

/* WhatsApp */
.share-option.whatsapp {
  color: #25D366;
  border-color: #25D366;
}

.share-option.whatsapp:hover {
  background: #25D366;
  color: white;
}

/* Twitter */
.share-option.twitter {
  color: #1DA1F2;
  border-color: #1DA1F2;
}

.share-option.twitter:hover {
  background: #1DA1F2;
  color: white;
}

/* Facebook */
.share-option.facebook {
  color: #1877F2;
  border-color: #1877F2;
}

.share-option.facebook:hover {
  background: #1877F2;
  color: white;
}

/* LinkedIn */
.share-option.linkedin {
  color: #0A66C2;
  border-color: #0A66C2;
}

.share-option.linkedin:hover {
  background: #0A66C2;
  color: white;
}

/* Copy Link */
.share-option.copy-link {
  color: #6c757d;
  border-color: #6c757d;
}

.share-option.copy-link:hover {
  background: #6c757d;
  color: white;
}

/* Yorum Butonu */
.share-option.comment {
  color: #8b5cf6;
  border-color: #8b5cf6;
}

.share-option.comment:hover {
  background: #8b5cf6;
  color: white;
}

.close-button {
  width: 100%;
  padding: 12px;
  background: #e9ecef;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  color: #2c3e50;
  transition: background 0.3s ease;
}

.close-button:hover {
  background: #dee2e6;
}

@media (max-width: 768px) {
  .share-modal {
    width: 95%;
    padding: 20px;
  }

  .share-options {
    grid-template-columns: 1fr;
  }

  .comment-actions {
    flex-direction: column;
  }
}
</style>