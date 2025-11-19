import ApiService from '@/services/ApiService';

const state = {
  projects: [],
  loading: false,
  error: null
};

const mutations = {
  setProjects(state, projects) {
    state.projects = projects;
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchProjects({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.fetch('Projects'); // ApiService.fetch kullanıyoruz
      if (response.success) {
        commit('setProjects', response.data);
      } else {
        commit('setError', response.message || 'Projeler yüklenemedi.');
      }
    } catch (err) {
      commit('setError', 'Projeler yüklenemedi. Lütfen daha sonra tekrar deneyin.');
      console.error("Projeler çekilirken hata oluştu:", err);
    } finally {
      commit('setLoading', false);
    }
  }
};

const getters = {
  allProjects: state => state.projects,
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
