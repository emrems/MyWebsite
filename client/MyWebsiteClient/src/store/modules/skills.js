import ApiService from '@/services/ApiService';

const state = {
  skills: [],
  loading: false,
  error: null
};

const mutations = {
  setSkills(state, skills) {
    state.skills = skills;
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchSkills({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.fetch('skills'); // ApiService.fetch kullanıyoruz
      if (response.success) {
        commit('setSkills', response.data);
      } else {
        commit('setError', response.message || 'Yetenekler yüklenemedi.');
      }
    } catch (err) {
      commit('setError', 'Yetenekler yüklenemedi. Lütfen daha sonra tekrar deneyin.');
      console.error("Yetenekler çekilirken hata oluştu:", err);
    } finally {
      commit('setLoading', false);
    }
  }
};

const getters = {
  allSkills: state => state.skills,
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
