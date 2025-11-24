
import ApiService from '@/services/ApiService';

const state = {
  experiences: [],
  loading: false,
  error: null
};

const mutations = {
  setExperiences(state, experiences) {
    state.experiences = experiences;
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchExperiences({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    const result = await ApiService.fetch('Experince');
    
    if (result.success) {
      commit('setExperiences', result.data);
    } else {
      commit('setError', result.message);
      commit('setExperiences', []);
    }
    
    commit('setLoading', false);
  }
};

const getters = {
  allExperiences: state => state.experiences,
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