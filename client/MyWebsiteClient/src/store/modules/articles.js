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
      console.log("sonuç",result)
      commit('setArticleDetail', result.data);
    } else {
      commit('setError', result.message || "Makale bulunamadı");
    }

    commit('setLoading', false);
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
