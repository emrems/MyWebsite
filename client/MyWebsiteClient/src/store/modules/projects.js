import ApiService from '@/services/ApiService';

const state = {
  projects: [],
  currentProject: null,
  loading: false,
  error: null
};

const mutations = {
  setProjects(state, projects) {
    state.projects = projects;
  },
  setCurrentProject(state, project) {
    state.currentProject = project;
  },
  addProject(state, project) {
    state.projects.push(project);
  },
  updateProject(state, updatedProject) {
    const index = state.projects.findIndex(p => p.id === updatedProject.id);
    if (index !== -1) {
      state.projects.splice(index, 1, updatedProject);
    }
  },
  removeProject(state, projectId) {
    state.projects = state.projects.filter(p => p.id !== projectId);
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
      const response = await ApiService.fetch('Projects');
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
  },

  async fetchProjectById({ commit }, projectId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.fetch(`Projects/${projectId}`);
      if (response.success) {
        commit('setCurrentProject', response.data);
        return { success: true, data: response.data };
      } else {
        commit('setError', response.message || 'Proje bulunamadı.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      commit('setError', 'Proje yüklenemedi.');
      console.error("Proje çekilirken hata oluştu:", err);
      return { success: false, message: 'Proje yüklenemedi.' };
    } finally {
      commit('setLoading', false);
    }
  },

  async createProject({ commit }, projectData) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.create('Projects', projectData);
      if (response.success) {
        commit('addProject', response.data);
        return { success: true, message: response.message || 'Proje başarıyla oluşturuldu.', data: response.data };
      } else {
        commit('setError', response.message || 'Proje oluşturulamadı.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Proje oluşturulamadı. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Proje oluşturulurken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  },

  async updateProject({ commit }, { projectId, projectData }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.update(`Projects/${projectId}`, projectData);
      if (response.success) {
        commit('updateProject', response.data);
        return { success: true, message: response.message || 'Proje başarıyla güncellendi.', data: response.data };
      } else {
        commit('setError', response.message || 'Proje güncellenemedi.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Proje güncellenemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Proje güncellenirken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  },

  async deleteProject({ commit }, projectId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.remove(`Projects/${projectId}`);
      if (response.success) {
        commit('removeProject', projectId);
        return { success: true, message: response.message || 'Proje başarıyla silindi.' };
      } else {
        commit('setError', response.message || 'Proje silinemedi.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Proje silinemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Proje silinirken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  }
};

const getters = {
  allProjects: state => state.projects,
  currentProject: state => state.currentProject,
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