<template>
  <div class="comment-container">
    <h2 class="comment-title">
      Yorumlar 
      <span v-if="comments.length > 0" class="comment-count">({{ comments.length }})</span>
    </h2>

    <p v-if="isLoading" class="loading-text">
      <i class="fas fa-spinner fa-spin"></i> Yorumlar yükleniyor...
    </p>

    <ErrorMessage 
      v-else-if="error" 
      :message="error"
      :showRetry="true"
      @retry="$emit('retry')"
    />

    <div v-else>
      <div 
        v-for="c in comments" 
        :key="c.id" 
        class="comment-item"
      >
        <div class="comment-header">
          <div class="comment-avatar">
            <i class="fas fa-user"></i>
          </div>
          <div class="comment-info">
            <strong class="comment-user">{{ c.userName }}</strong>
            <span class="comment-date">{{ formatDate(c.createdDate) }}</span>
          </div>
        </div>
        <p class="comment-text">{{ c.content }}</p>
        <div v-if="c.articleTitle" class="comment-article">
          <i class="fas fa-file-alt"></i> {{ c.articleTitle }}
        </div>
      </div>

      <div v-if="comments.length === 0" class="empty-comments">
        <i class="fas fa-comments"></i>
        <p>Henüz yorum yapılmamış. İlk yorumu siz yapın!</p>
      </div>
    </div>
  </div>
</template>

<script>
import ErrorMessage from "@/components/common/ErrorMessage.vue";

export default {
  name: "CommentList",
  components: { ErrorMessage },

  props: {
    comments: {
      type: Array,
      default: () => []
    },
    isLoading: {
      type: Boolean,
      default: false
    },
    error: {
      type: String,
      default: null
    }
  },

  methods: {
    formatDate(dateString) {
      if (!dateString) return 'Tarih belirtilmemiş';
      
      try {
        // ISO 8601 formatındaki tarihi parse et
        const date = new Date(dateString);
        
        // Geçerli tarih kontrolü
        if (isNaN(date.getTime())) {
          console.error('Geçersiz tarih:', dateString);
          return 'Geçersiz tarih';
        }

        // Türkçe yerel ayarlarla formatla
        return date.toLocaleDateString("tr-TR", {
          year: 'numeric',
          month: 'long',
          day: 'numeric',
          hour: '2-digit',
          minute: '2-digit'
        });
      } catch (error) {
        console.error('Tarih formatlama hatası:', error, dateString);
        return 'Tarih formatlanamadı';
      }
    },

    // Alternatif: Göreceli zaman (örn: "2 saat önce")
    formatRelativeTime(dateString) {
      if (!dateString) return '';
      
      try {
        const date = new Date(dateString);
        const now = new Date();
        const diffMs = now - date;
        const diffMins = Math.floor(diffMs / 60000);
        const diffHours = Math.floor(diffMs / 3600000);
        const diffDays = Math.floor(diffMs / 86400000);

        if (diffMins < 1) return 'Az önce';
        if (diffMins < 60) return `${diffMins} dakika önce`;
        if (diffHours < 24) return `${diffHours} saat önce`;
        if (diffDays < 7) return `${diffDays} gün önce`;
        
        return date.toLocaleDateString("tr-TR");
      } catch (error) {
        return this.formatDate(dateString);
      }
    }
  }
};
</script>

<style scoped>
.comment-container {
  margin-top: 50px;
  border-top: 2px solid #e2e8f0;
  padding-top: 30px;
}

.comment-title {
  font-size: 1.5rem;
  font-weight: 700;
  margin-bottom: 25px;
  color: #1e293b;
  display: flex;
  align-items: center;
  gap: 10px;
}

.comment-count {
  font-size: 1rem;
  color: #64748b;
  font-weight: 500;
}

.loading-text {
  text-align: center;
  color: #64748b;
  padding: 30px;
  font-size: 1rem;
}

.loading-text i {
  margin-right: 8px;
}

.comment-item {
  margin-bottom: 25px;
  padding: 20px;
  background: #f8fafc;
  border-radius: 12px;
  border-left: 4px solid #3b82f6;
  transition: all 0.2s ease;
}

.comment-item:hover {
  background: #f1f5f9;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.comment-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 12px;
}

.comment-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 1.1rem;
  flex-shrink: 0;
}

.comment-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.comment-user {
  color: #1e293b;
  font-size: 1rem;
  font-weight: 600;
}

.comment-date {
  font-size: 0.85rem;
  color: #64748b;
}

.comment-text {
  margin: 0;
  color: #475569;
  line-height: 1.6;
  font-size: 0.95rem;
  padding-left: 52px;
}

.comment-article {
  margin-top: 10px;
  padding: 8px 12px;
  background: white;
  border-radius: 6px;
  font-size: 0.85rem;
  color: #64748b;
  display: inline-flex;
  align-items: center;
  gap: 6px;
  margin-left: 52px;
}

.comment-article i {
  color: #3b82f6;
}

.empty-comments {
  text-align: center;
  padding: 60px 20px;
  color: #94a3b8;
  background: #f8fafc;
  border-radius: 12px;
  border: 2px dashed #e2e8f0;
}

.empty-comments i {
  font-size: 3rem;
  margin-bottom: 15px;
  color: #cbd5e1;
}

.empty-comments p {
  margin: 0;
  font-size: 1rem;
}

@media (max-width: 768px) {
  .comment-title {
    font-size: 1.3rem;
  }

  .comment-item {
    padding: 15px;
  }

  .comment-text {
    padding-left: 0;
    margin-top: 10px;
  }

  .comment-article {
    margin-left: 0;
  }
}
</style>