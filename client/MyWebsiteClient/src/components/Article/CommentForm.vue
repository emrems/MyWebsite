<template>
  <div class="comment-form-wrapper">
    <h3>Yorum Yap</h3>

    <textarea
      v-model="content"
      placeholder="Yorumunuzu yazın..."
      class="comment-textarea"
    ></textarea>

    <button class="comment-submit-btn" @click="submitComment" :disabled="loading">
      <span v-if="!loading">Gönder</span>
      <span v-else>Gönderiliyor...</span>
    </button>
  </div>
</template>

<script>
export default {
  props: {
    articleId: Number,
    loading: Boolean
  },
  data() {
    return {
      content: ""
    };
  },
  methods: {
    submitComment() {
      if (!this.content.trim()) {
        this.$notify.show({
          type: "error",
          title: "Hata",
          message: "Yorum boş olamaz."
        });
        return;
      }

      this.$emit("submit", this.content);
      this.content = "";
    }
  }
};
</script>

<style scoped>
.comment-form-wrapper {
  margin-top: 40px;
  padding: 20px;
  background: #f8fafc;
  border-radius: 12px;
}

.comment-textarea {
  width: 100%;
  height: 120px;
  padding: 12px;
  border-radius: 8px;
  border: 1px solid #cbd5e1;
  resize: none;
}

.comment-submit-btn {
  margin-top: 10px;
  padding: 10px 18px;
  background: #3b82f6;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
</style>
