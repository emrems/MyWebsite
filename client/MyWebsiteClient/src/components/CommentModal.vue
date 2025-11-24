<template>
  <div v-if="isVisible" class="modal-overlay" @click="closeModal">
    <div class="comment-modal" @click.stop>
      <div class="modal-header">
        <h3>Yorum Yap</h3>
        <button class="close-button" @click="closeModal">
          <i class="fas fa-times"></i>
        </button>
      </div>
      
      <div class="modal-body">
        <div class="article-info" v-if="articleTitle">
          <p class="article-title">{{ articleTitle }}</p>
        </div>
        
        <div class="comment-form">
          <label for="comment" class="form-label">Yorumunuz</label>
          <textarea 
            id="comment"
            v-model="commentText" 
            placeholder="Makale hakkındaki düşüncelerinizi yazın..."
            rows="6"
            class="comment-textarea"
            :disabled="isSubmitting"
          ></textarea>
          
          <div class="character-count">
            {{ commentText.length }}/500 karakter
          </div>
        </div>
      </div>
      
      <div class="modal-footer">
        <button 
          @click="cancelComment" 
          class="cancel-btn"
          :disabled="isSubmitting"
        >
          İptal
        </button>
        <button 
          @click="submitComment" 
          class="submit-btn" 
          :disabled="!commentText.trim() || isSubmitting || commentText.length > 500"
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
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from '@/services/ApiService';

export default {
  name: 'CommentModal',
  
  props: {
    isVisible: {
      type: Boolean,
      default: false
    },
    articleId: {
      type: Number,
      default: null
    },
    articleTitle: {
      type: String,
      default: ''
    }
  },

  data() {
    return {
      commentText: '',
      isSubmitting: false
    };
  },

  methods: {
    closeModal() {
      this.commentText = '';
      this.isSubmitting = false;
      this.$emit('close');
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

      if (this.commentText.length > 500) {
        this.showNotification({
          type: 'warning',
          title: 'Uyarı',
          message: 'Yorum 500 karakteri geçemez'
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

        const result = await ApiService.create('Comment', commentData);

        if (result.success) {
          this.showNotification({
            type: 'success',
            title: 'Başarılı',
            message: 'Yorumunuz başarıyla gönderildi!'
          });
          
          this.$emit('comment-submitted', result.data);
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
      if (this.commentText.trim() && !this.isSubmitting) {
        if (confirm('Yorumunuz kaydedilmedi. İptal etmek istediğinize emin misiniz?')) {
          this.closeModal();
        }
      } else {
        this.closeModal();
      }
    },

    showNotification(options) {
      if (this.$notify) {
        this.$notify(options);
      } else {
        console.log('Notification:', options);
      }
    }
  },

  watch: {
    isVisible(newVal) {
      if (!newVal) {
        this.commentText = '';
        this.isSubmitting = false;
      }
    }
  }
};
</script>

<style scoped>
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
  from { opacity: 0; }
  to { opacity: 1; }
}

.comment-modal {
  background: white;
  border-radius: 16px;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow: hidden;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  animation: slideUp 0.3s ease;
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

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 24px;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h3 {
  margin: 0;
  color: #1f2937;
  font-size: 1.5rem;
  font-weight: 600;
}

.modal-header .close-button {
  background: none;
  border: none;
  color: #6b7280;
  cursor: pointer;
  padding: 8px;
  border-radius: 8px;
  transition: all 0.2s ease;
  font-size: 1.2rem;
}

.modal-header .close-button:hover {
  background: #f3f4f6;
  color: #374151;
}

.modal-body {
  padding: 24px;
}

.article-info {
  margin-bottom: 20px;
  padding: 16px;
  background: #f8fafc;
  border-radius: 8px;
  border-left: 4px solid #8b5cf6;
}

.article-title {
  margin: 0;
  color: #475569;
  font-size: 0.95rem;
  font-weight: 500;
}

.comment-form {
  margin-bottom: 20px;
}

.form-label {
  display: block;
  margin-bottom: 8px;
  color: #374151;
  font-weight: 500;
  font-size: 0.95rem;
}

.comment-textarea {
  width: 100%;
  padding: 16px;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  resize: vertical;
  font-family: inherit;
  font-size: 1rem;
  line-height: 1.5;
  transition: all 0.2s ease;
  background: white;
}

.comment-textarea:focus {
  outline: none;
  border-color: #8b5cf6;
  box-shadow: 0 0 0 3px rgba(139, 92, 246, 0.1);
}

.comment-textarea:disabled {
  background: #f9fafb;
  cursor: not-allowed;
}

.character-count {
  text-align: right;
  margin-top: 8px;
  color: #6b7280;
  font-size: 0.875rem;
}

.modal-footer {
  display: flex;
  gap: 12px;
  padding: 24px;
  border-top: 1px solid #e5e7eb;
  background: #fafafa;
}

.cancel-btn {
  flex: 1;
  padding: 12px 24px;
  background: white;
  color: #6b7280;
  border: 2px solid #e5e7eb;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s ease;
}

.cancel-btn:hover:not(:disabled) {
  background: #f3f4f6;
  border-color: #d1d5db;
}

.submit-btn {
  flex: 2;
  padding: 12px 24px;
  background: #8b5cf6;
  color: white;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.submit-btn:hover:not(:disabled) {
  background: #7c3aed;
  transform: translateY(-1px);
}

.submit-btn:disabled {
  background: #cbd5e1;
  cursor: not-allowed;
  transform: none;
}

.button-loading {
  display: flex;
  align-items: center;
  gap: 8px;
}

@media (max-width: 768px) {
  .comment-modal {
    width: 95%;
    margin: 20px;
  }
  
  .modal-header,
  .modal-body,
  .modal-footer {
    padding: 20px;
  }
  
  .modal-footer {
    flex-direction: column-reverse;
  }
  
  .cancel-btn,
  .submit-btn {
    flex: none;
  }
}
</style>