import ApiService from '@/services/ApiService';

const state = {
  articles: [],
  articleDetail: null,
  loading: false,
  error: null
};

const mutations = {
  setArticles(state, articles) {
    state.articles = articles;
  },
  setArticleDetail(state, article) {
    state.articleDetail = article;
  },
  addArticle(state, article) {
    if (article && article.id) {
      state.articles.unshift(article);
    }
  },
  updateArticle(state, updatedArticle) {
    if (!updatedArticle || !updatedArticle.id) {
      console.error('Geçersiz article objesi:', updatedArticle);
      return;
    }
    
    const index = state.articles.findIndex(a => a.id === updatedArticle.id);
    if (index !== -1) {
      state.articles.splice(index, 1, updatedArticle);
    } else {
      console.warn('Güncellenmek istenen makale bulunamadı:', updatedArticle.id);
    }
  },
  removeArticle(state, articleId) {
    state.articles = state.articles.filter(a => a.id !== articleId);
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchArticles({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    const result = await ApiService.fetch('article/all');
    
    if (result.success) {
      commit('setArticles', result.data);
    } else {
      commit('setError', result.message);
      commit('setArticles', []);
    }
    
    commit('setLoading', false);
  },

  async fetchArticleBySlug({ commit }, slug) {
    commit('setLoading', true);
    commit('setError', null);
    commit('setArticleDetail', null);

    const result = await ApiService.fetch(`article/slug/${slug}`);

    if (result.success) {
      console.log("sonuç", result);
      commit('setArticleDetail', result.data);
    } else {
      commit('setError', result.message || "Makale bulunamadı");
    }

    commit('setLoading', false);
  },

  async createArticle({ commit, rootGetters }, articleData) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      // AuthorId'yi JWT'den al
      const currentUser = rootGetters['auth/currentUser'];
      const dataToSend = {
        ...articleData,
        authorId: currentUser.id
      };

      const response = await ApiService.create('article/create', dataToSend);
      
      if (response.success) {
        commit('addArticle', response.data);
        return { 
          success: true, 
          message: response.message || 'Makale başarıyla oluşturuldu.', 
          data: response.data 
        };
      } else {
        commit('setError', response.message || 'Makale oluşturulamadı.');
        return { 
          success: false, 
          message: response.message || 'Makale oluşturulamadı.' 
        };
      }
    } catch (err) {
      const errorMsg = 'Makale oluşturulamadı. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Makale oluşturulurken hata oluştu:", err);
      return { 
        success: false, 
        message: errorMsg 
      };
    } finally {
      commit('setLoading', false);
    }
  },

  async updateArticle({ commit }, { articleId, articleData }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      console.log('Güncellenecek makale ID:', articleId);
      console.log('Gönderilen veri:', articleData);

      const response = await ApiService.update(`article/update/${articleId}`, articleData);
      
      console.log('API Yanıtı:', response);
      
      if (response.success) {
        // Yanıtta güncellenmiş makale verisi var mı kontrol et
        if (response.data) {
          commit('updateArticle', response.data);
        } else {
          // Eğer API sadece mesaj döndürüyorsa, mevcut listeyi güncelle
          commit('setError', null);
        }
        return { 
          success: true, 
          message: response.message || 'Makale başarıyla güncellendi.', 
          data: response.data 
        };
      } else {
        const errorMsg = response.message || 'Makale güncellenemedi.';
        commit('setError', errorMsg);
        console.error('API hatası:', errorMsg);
        return { 
          success: false, 
          message: errorMsg 
        };
      }
    } catch (err) {
      const errorMsg = 'Makale güncellenemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Makale güncellenirken hata oluştu:", err);
      return { 
        success: false, 
        message: errorMsg 
      };
    } finally {
      commit('setLoading', false);
    }
  },

  async deleteArticle({ commit }, articleId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.remove(`article/${articleId}`);
      
      if (response.success) {
        commit('removeArticle', articleId);
        return { 
          success: true, 
          message: response.message || 'Makale başarıyla silindi.' 
        };
      } else {
        const errorMsg = response.message || 'Makale silinemedi.';
        commit('setError', errorMsg);
        return { 
          success: false, 
          message: errorMsg 
        };
      }
    } catch (err) {
      const errorMsg = 'Makale silinemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Makale silinirken hata oluştu:", err);
      return { 
        success: false, 
        message: errorMsg 
      };
    } finally {
      commit('setLoading', false);
    }
  }
};

const getters = {
  allArticles: state => state.articles,
  articleDetail: state => state.articleDetail,
  isLoading: state => state.loading,
  getError: state => state.error
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};