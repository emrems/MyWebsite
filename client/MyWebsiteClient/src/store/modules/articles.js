
import ApiService from '@/services/ApiService';

const state = {
  articles: [],
  loading: false,
  error: null
};

const mutations = {
  setArticles(state, articles) {
    state.articles = articles;
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
  }
};

const getters = {
  allArticles: state => state.articles,
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