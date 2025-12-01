// store/modules/auth.js
import ApiService from '@/services/ApiService';

const state = {
  token: localStorage.getItem('authToken') || null,
  user: JSON.parse(localStorage.getItem('user')) || null,
  tokenExpiration: localStorage.getItem('tokenExpiration') || null,
  isAuthenticated: !!localStorage.getItem('authToken'),
};

const getters = {
  isAuthenticated: (state) => state.isAuthenticated,
  currentUser: (state) => state.user,
  token: (state) => state.token,
  isTokenExpired: (state) => {
    if (!state.tokenExpiration) return true;
    return new Date(state.tokenExpiration) < new Date();
  },
};

const mutations = {
  SET_TOKEN(state, token) {
    state.token = token;
    state.isAuthenticated = !!token;
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  },

  SET_USER(state, user) {
    state.user = user;
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
    } else {
      localStorage.removeItem('user');
    }
  },

  SET_TOKEN_EXPIRATION(state, expiration) {
    state.tokenExpiration = expiration;
    if (expiration) {
      localStorage.setItem('tokenExpiration', expiration);
    } else {
      localStorage.removeItem('tokenExpiration');
    }
  },

  CLEAR_AUTH(state) {
    state.token = null;
    state.user = null;
    state.tokenExpiration = null;
    state.isAuthenticated = false;
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
    localStorage.removeItem('tokenExpiration');
  },
};

const actions = {
  async login({ commit }, loginDto) {
    try {
      const response = await ApiService.create('Auth/login', loginDto);

      if (response.success && response.data) {
        const { token, expiration } = response.data;

        // Token'ı kaydet
        commit('SET_TOKEN', token);
        commit('SET_TOKEN_EXPIRATION', expiration);

        // Token'dan user bilgisini çıkar (JWT decode)
        const userInfo = parseJwt(token);
        commit('SET_USER', {
          id: userInfo['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
          username: userInfo['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
          role: userInfo['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
        });

        return { success: true, message: response.message };
      }

      return { success: false, message: response.message || 'Giriş başarısız' };
    } catch (error) {
      return { success: false, message: 'Giriş sırasında bir hata oluştu' };
    }
  },

  logout({ commit }) {
    commit('CLEAR_AUTH');
    // İsteğe bağlı: Kullanıcıyı login sayfasına yönlendir
    // router.push('/login');
  },

  // Token'ı kontrol et ve gerekirse yenile
  checkAuth({ state, commit }) {
    if (state.token && state.tokenExpiration) {
      const isExpired = new Date(state.tokenExpiration) < new Date();
      if (isExpired) {
        commit('CLEAR_AUTH');
        return false;
      }
      return true;
    }
    return false;
  },
};

// JWT token'ı parse et
function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('Token parse hatası:', error);
    return {};
  }
}

export default {
  namespaced: true,
  state,
  getters,
  mutations,
  actions,
};