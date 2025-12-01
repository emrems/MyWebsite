import ApiService from '@/services/ApiService';

const state = {
  categories: [],
  currentCategory: null,
  loading: false,
  error: null
};

const mutations = {
  setCategories(state, categories) {
    state.categories = categories;
  },
  setCurrentCategory(state, category) {
    state.currentCategory = category;
  },
  addCategory(state, category) {
    state.categories.push(category);
  },
  updateCategory(state, updatedCategory) {
    const index = state.categories.findIndex(c => c.id === updatedCategory.id);
    if (index !== -1) {
      state.categories.splice(index, 1, updatedCategory);
    }
  },
  removeCategory(state, categoryId) {
    state.categories = state.categories.filter(c => c.id !== categoryId);
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchCategories({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.fetch('categories');
      if (response.success) {
        commit('setCategories', response.data);
      } else {
        commit('setError', response.message || 'Kategoriler yüklenemedi.');
      }
    } catch (err) {
      commit('setError', 'Kategoriler yüklenemedi. Lütfen daha sonra tekrar deneyin.');
      console.error("Kategoriler çekilirken hata oluştu:", err);
    } finally {
      commit('setLoading', false);
    }
  },

  async fetchCategoryById({ commit }, categoryId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.fetch(`categories/getCategoriesById?id=${categoryId}`);
      if (response.success) {
        commit('setCurrentCategory', response.data);
        return { success: true, data: response.data };
      } else {
        commit('setError', response.message || 'Kategori bulunamadı.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      commit('setError', 'Kategori yüklenemedi.');
      console.error("Kategori çekilirken hata oluştu:", err);
      return { success: false, message: 'Kategori yüklenemedi.' };
    } finally {
      commit('setLoading', false);
    }
  },

  async createCategory({ commit }, categoryData) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.create('categories', categoryData);
      if (response.success) {
        commit('addCategory', response.data);
        return { success: true, message: response.message || 'Kategori başarıyla oluşturuldu.', data: response.data };
      } else {
        commit('setError', response.message || 'Kategori oluşturulamadı.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Kategori oluşturulamadı. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Kategori oluşturulurken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  },

  async updateCategory({ commit }, { categoryId, categoryData }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.update(`categories?id=${categoryId}`, categoryData);
      if (response.success) {
        commit('updateCategory', response.data);
        return { success: true, message: response.message || 'Kategori başarıyla güncellendi.', data: response.data };
      } else {
        commit('setError', response.message || 'Kategori güncellenemedi.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Kategori güncellenemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Kategori güncellenirken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  },

  async deleteCategory({ commit }, categoryId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.remove(`categories/${categoryId}`);
      if (response.success) {
        commit('removeCategory', categoryId);
        return { success: true, message: response.message || 'Kategori başarıyla silindi.' };
      } else {
        commit('setError', response.message || 'Kategori silinemedi.');
        return { success: false, message: response.message };
      }
    } catch (err) {
      const errorMsg = 'Kategori silinemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Kategori silinirken hata oluştu:", err);
      return { success: false, message: errorMsg };
    } finally {
      commit('setLoading', false);
    }
  }
};

const getters = {
  allCategories: state => state.categories,
  currentCategory: state => state.currentCategory,
  isLoading: state => state.loading,
  getError: state => state.error,
  // Kategorileri select için formatlama
  categoriesForSelect: state => state.categories.map(cat => ({
    value: cat.id,
    label: cat.name
  }))
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};