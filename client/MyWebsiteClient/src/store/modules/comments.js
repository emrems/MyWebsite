import ApiService from "@/services/ApiService";

const state = {
  comments: [],
  loading: false,
  error: null
};

const mutations = {
  setComments(state, data) {
    state.comments = data;
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, err) {
    state.error = err;
  },
  addComment(state, comment) {
    state.comments.unshift(comment);
  }
};

const actions = {
  async fetchCommentsByArticle({ commit }, articleId) {
    commit("setLoading", true);
    commit("setError", null);

    try {
      const res = await ApiService.fetch(`comment/${articleId}`);

      if (res.success) {
       
        let comments = res.data;
        
        // Eğer tek obje dönüyorsa array'e çevir
        if (comments && !Array.isArray(comments)) {
          comments = [comments];
        }
        
        // Null veya undefined ise boş array
        if (!comments) {
          comments = [];
        }

        commit("setComments", comments);
      } else {
        commit("setError", res.message || "Yorumlar yüklenemedi.");
        commit("setComments", []); // Hata durumunda boş array
      }
    } catch (err) {
      console.error("Yorum yükleme hatası:", err);
      commit("setError", "Yorumlar yüklenirken hata oluştu.");
      commit("setComments", []); // Hata durumunda boş array
    } finally {
      commit("setLoading", false);
    }
  },

  // Yeni yorum ekleme
  async createComment({ commit, dispatch }, { articleId, content }) {
    try {
      const res = await ApiService.create("comment/CreateComment", {
        articleId,
        content
      });

      if (res.success) {
        // Yorumları yeniden yükle
        await dispatch("fetchCommentsByArticle", articleId);
        return { success: true, message: res.message || "Yorum eklendi." };
      } else {
        return { success: false, message: res.message || "Yorum eklenemedi." };
      }
    } catch (err) {
      console.error("Yorum ekleme hatası:", err);
      return { success: false, message: "Yorum eklenirken hata oluştu." };
    }
  }
};

const getters = {
  articleComments: (state) => state.comments,
  isCommentLoading: (state) => state.loading,
  commentError: (state) => state.error,
  commentCount: (state) => state.comments.length
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};
