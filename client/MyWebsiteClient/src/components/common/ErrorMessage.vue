<template>
  <div class="error-wrapper">
    <div class="error-box">
      <h2 class="error-title">Bir Hata Oluştu</h2>
      <p class="error-message">{{ message }}</p>

      <button v-if="showRetry" class="retry-btn" @click="$emit('retry')">
        Tekrar Dene
      </button>

      <router-link v-if="goHome" to="/" class="home-btn">
        Ana Sayfaya Dön
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    message: {
      type: String,
      default: "Beklenmeyen bir hata oluştu."
    },
    showRetry: {
      type: Boolean,
      default: false
    },
    goHome: {
      type: Boolean,
      default: true
    }
  }
};
</script>

<style scoped>
.error-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 60vh;
}

.error-box {
  text-align: center;
  padding: 30px;
  border: 1px solid #ff6b6b;
  border-radius: 10px;
  background: #ffecec;
  width: 400px;
}

.error-title {
  color: #d9534f;
  margin-bottom: 10px;
}

.error-message {
  color: #555;
  margin-bottom: 20px;
}

.retry-btn,
.home-btn {
  padding: 10px;
  background: #d9534f;
  color: white;
  border-radius: 5px;
  text-decoration: none;
  border: none;
  cursor: pointer;
  margin-right: 10px;
}
</style>
