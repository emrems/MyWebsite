import ApiService from '@/services/ApiService';

// Başlangıç durumu
const state = {
  projects: [],
  loading: false,
  error: null
};

// Durumu değiştiren method'lar
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

// Asenkron işlemleri yöneten method'lar
const actions = {
  async fetchProjects({ commit }) {
    commit('setLoading', true);
    commit('setError', null); // Yeni bir işlem başladığında hatayı sıfırla

    try {
      const response = await ApiService.get('Projects');
      commit('setProjects', response.data);
    } catch (error) {
      console.error("Projeler çekilirken hata oluştu:", error);
      commit('setError', "Projeler yüklenemedi. Lütfen daha sonra tekrar deneyin.");
    } finally {
      commit('setLoading', false);
    }
  }
};

// Durumun hesaplanmış özelliklerini döndüren method'lar
const getters = {
  allProjects: state => state.projects,
  isLoading: state => state.loading,
  getError: state => state.error
};

export default {
  namespaced: true, // Modül içinde isim uzayı kullanma
  state,
  mutations,
  actions,
  getters
};