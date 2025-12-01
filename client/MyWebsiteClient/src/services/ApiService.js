  // ApiService.js
  import axios from "axios";
  import store from "@/store"; // Vuex store'u import et

  const API_URL = "https://localhost:7239/api";

  // ⭐ AXIOS INTERCEPTOR ile token'ı dinamik olarak ekle
  axios.interceptors.request.use(
    (config) => {
      // Store'dan token'ı al
      const token = store.state.auth?.token || localStorage.getItem('authToken');
      
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      
      // Content-Type otomatik olarak ayarlanır, sadece gerekirse override et
      if (!config.headers['Content-Type']) {
        config.headers['Content-Type'] = 'application/json';
      }
      
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  // ⭐ RESPONSE INTERCEPTOR - Token süresi dolduğunda otomatik logout
  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        // Token geçersiz veya süresi dolmuş
        store.dispatch('auth/logout');
        // İsteğe bağlı: Login sayfasına yönlendir
        // window.location.href = '/login';
      }
      return Promise.reject(error);
    }
  );

  const ApiService = {
    // ------------------------------------
    // BASIC CRUD METHODS
    // ------------------------------------

    get(resource) {
      return axios.get(`${API_URL}/${resource}`);
    },

    post(resource, data) {
      return axios.post(`${API_URL}/${resource}`, data);
    },

    put(resource, data) {
      return axios.put(`${API_URL}/${resource}`, data);
    },

    delete(resource) {
      return axios.delete(`${API_URL}/${resource}`);
    },

    async fetch(resource) {
      try {
        const response = await axios.get(`${API_URL}/${resource}`);

        return {
          success: response.data.isSuccess,
          data: response.data.isSuccess ? response.data.data : null,
          message: response.data.message,
          errorCode: response.data.errorCode,
        };
      } catch (error) {
        return {
          success: false,
          data: null,
          message: this.getErrorMessage(error),
          errorCode: error.response?.status || null,
        };
      }
    },

    async create(resource, data) {
      try {
        const response = await axios.post(`${API_URL}/${resource}`, data);

        return {
          success: response.data.isSuccess,
          data: response.data.isSuccess ? response.data.data : null,
          message: response.data.message,
          errorCode: response.data.errorCode,
        };
      } catch (error) {
        return {
          success: false,
          data: null,
          message: this.getErrorMessage(error),
          errorCode: error.response?.status || null,
        };
      }
    },

    async update(resource, data) {
      try {
        const response = await axios.put(`${API_URL}/${resource}`, data);

        return {
          success: response.data.isSuccess,
          data: response.data.isSuccess ? response.data.data : null,
          message: response.data.message,
          errorCode: response.data.errorCode,
        };
      } catch (error) {
        return {
          success: false,
          data: null,
          message: this.getErrorMessage(error),
          errorCode: error.response?.status || null,
        };
      }
    },

    async remove(resource) {
      try {
        const response = await axios.delete(`${API_URL}/${resource}`);

        return {
          success: response.data.isSuccess,
          data: response.data.isSuccess ? response.data.data : null,
          message: response.data.message,
          errorCode: response.data.errorCode,
        };
      } catch (error) {
        return {
          success: false,
          data: null,
          message: this.getErrorMessage(error),
          errorCode: error.response?.status || null,
        };
      }
    },

    getErrorMessage(error) {
      if (error.response) {
        const status = error.response.status;

        if (status === 404) return "İstenilen kaynak bulunamadı.";
        if (status === 401) return "Lütfen giriş yapın.";
        if (status === 403) return "Bu işlem için yetkiniz bulunmamaktadır.";
        if (status === 500)
          return "Sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin.";

        if (status >= 400 && status < 500)
          return error.response.data?.message || "İstek hatası oluştu.";

        if (status >= 500)
          return "Sunucu hatası. Lütfen daha sonra tekrar deneyin.";
      } else if (error.request) {
        return "Lütfen internet bağlantınızı kontrol edin veya sunucunun çalıştığından emin olun.";
      }

      return "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.";
    },
  };

  export default ApiService;