// store/modules/users.js
import ApiService from '@/services/ApiService';

const state = {
  users: [],
  currentUser: null,
  loading: false,
  error: null
};

const mutations = {
  setUsers(state, users) {
    state.users = users;
  },
  setCurrentUser(state, user) {
    state.currentUser = user;
  },
  addUser(state, user) {
    if (user && user.id) {
      state.users.unshift(user);
    }
  },
  updateUser(state, updatedUser) {
    if (!updatedUser || !updatedUser.id) {
      console.error('Geçersiz user objesi:', updatedUser);
      return;
    }
    
    const index = state.users.findIndex(u => u.id === updatedUser.id);
    if (index !== -1) {
      state.users.splice(index, 1, updatedUser);
    } else {
      console.warn('Güncellenmek istenen user bulunamadı:', updatedUser.id);
    }
  },
  removeUser(state, userId) {
    state.users = state.users.filter(u => u.id !== userId);
  },
  setLoading(state, status) {
    state.loading = status;
  },
  setError(state, error) {
    state.error = error;
  }
};

const actions = {
  async fetchUsers({ commit }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const result = await ApiService.fetch('users');
      
      if (result.success) {
        // API'den gelen veriyi normalize et
        const normalizedUsers = result.data.map(user => ({
          ...user,
          // comments array'i yoksa boş array oluştur
          comments: user.comments || [],
          // articles array'i yoksa boş array oluştur
          articles: user.articles || []
        }));
        commit('setUsers', normalizedUsers);
      } else {
        commit('setError', result.message);
        commit('setUsers', []);
      }
    } catch (error) {
      console.error('Kullanıcılar yüklenirken hata:', error);
      commit('setError', 'Kullanıcılar yüklenirken bir hata oluştu.');
      commit('setUsers', []);
    } finally {
      commit('setLoading', false);
    }
  },

  async fetchUserById({ commit }, userId) {
    commit('setLoading', true);
    commit('setError', null);
    commit('setCurrentUser', null);

    try {
      const result = await ApiService.fetch(`users/${userId}`);

      if (result.success) {
        commit('setCurrentUser', result.data);
      } else {
        commit('setError', result.message || "Kullanıcı bulunamadı");
      }
    } catch (error) {
      console.error('Kullanıcı detayı yüklenirken hata:', error);
      commit('setError', 'Kullanıcı detayı yüklenirken bir hata oluştu.');
    } finally {
      commit('setLoading', false);
    }
  },

  async updateUser({ commit }, { userId, userData }) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      console.log('Güncellenecek kullanıcı ID:', userId);
      console.log('Gönderilen veri:', userData);

      const response = await ApiService.update(`users/${userId}`, userData);
      
      console.log('API Yanıtı:', response);
      
      if (response.success) {
        if (response.data) {
          commit('updateUser', response.data);
        }
        return { 
          success: true, 
          message: response.message || 'Kullanıcı başarıyla güncellendi.', 
          data: response.data 
        };
      } else {
        const errorMsg = response.message || 'Kullanıcı güncellenemedi.';
        commit('setError', errorMsg);
        console.error('API hatası:', errorMsg);
        return { 
          success: false, 
          message: errorMsg 
        };
      }
    } catch (err) {
      const errorMsg = 'Kullanıcı güncellenemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Kullanıcı güncellenirken hata oluştu:", err);
      return { 
        success: false, 
        message: errorMsg 
      };
    } finally {
      commit('setLoading', false);
    }
  },

  async deleteUser({ commit }, userId) {
    commit('setLoading', true);
    commit('setError', null);

    try {
      const response = await ApiService.remove(`users/${userId}`);
      
      if (response.success) {
        commit('removeUser', userId);
        return { 
          success: true, 
          message: response.message || 'Kullanıcı başarıyla silindi.' 
        };
      } else {
        const errorMsg = response.message || 'Kullanıcı silinemedi.';
        commit('setError', errorMsg);
        return { 
          success: false, 
          message: errorMsg 
        };
      }
    } catch (err) {
      const errorMsg = 'Kullanıcı silinemedi. Lütfen daha sonra tekrar deneyin.';
      commit('setError', errorMsg);
      console.error("Kullanıcı silinirken hata oluştu:", err);
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
  allUsers: state => state.users,
  currentUserDetail: state => state.currentUser,
  isLoading: state => state.loading,
  getError: state => state.error,
  adminUsers: state => state.users.filter(u => u.role === 'Admin'),
  regularUsers: state => state.users.filter(u => u.role === 'User'),
  userCount: state => state.users.length,
  adminCount: state => state.users.filter(u => u.role === 'Admin').length,
  
  // Kullanıcının doğrudan yorum sayısını hesapla (API'den gelen comments array'inden)
  userDirectCommentsCount: (state) => (userId) => {
    const user = state.users.find(u => u.id === userId);
    return user && user.comments ? user.comments.length : 0;
  },
  
  // Kullanıcının makalelerindeki yorum sayısını hesapla
  userArticlesCommentsCount: (state) => (userId) => {
    const user = state.users.find(u => u.id === userId);
    if (!user || !user.articles) return 0;
    
    return user.articles.reduce((total, article) => {
      return total + (article.comments ? article.comments.length : 0);
    }, 0);
  },
  
  // Kullanıcının toplam yorum sayısını hesapla (hem doğrudan hem makalelerdeki)
  userTotalCommentsCount: (state) => (userId) => {
    const user = state.users.find(u => u.id === userId);
    if (!user) return 0;
    
    let total = 0;
    
    // Doğrudan kullanıcı yorumları
    if (user.comments) {
      total += user.comments.length;
    }
    
    // Makalelerdeki yorumlar
    if (user.articles) {
      total += user.articles.reduce((sum, article) => {
        return sum + (article.comments ? article.comments.length : 0);
      }, 0);
    }
    
    return total;
  },
  
  // Kullanıcının toplam makale sayısını hesapla
  userArticlesCount: (state) => (userId) => {
    const user = state.users.find(u => u.id === userId);
    return user && user.articles ? user.articles.length : 0;
  },
  
  // Kullanıcının makalelerindeki toplam beğeni sayısını hesapla
  userTotalLikes: (state) => (userId) => {
    const user = state.users.find(u => u.id === userId);
    if (!user || !user.articles) return 0;
    
    return user.articles.reduce((total, article) => {
      return total + (article.articleLikeCount || 0);
    }, 0);
  }
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
};