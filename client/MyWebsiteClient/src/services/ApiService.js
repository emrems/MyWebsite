// ApiService.js
import axios from "axios";

const API_URL = "https://localhost:7239/api";

// ⭐ STATIC TOKEN (istediğin gibi)
const TOKEN =
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoia2FkaXIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzY0MTA5NTM2LCJpc3MiOiJNeVdlYnNpdGUiLCJhdWQiOiJNeVdlYnNpdGVVc2VycyJ9.b2hVRG8lFuginGV_eQqvwFKsORe1CWDMdrzqX_7xyo4";

// ⭐ TÜM İSTEKLERE OTOMATİK TOKEN + JSON FORMATINI EKLE
axios.defaults.headers.common["Authorization"] = `Bearer ${TOKEN}`;
axios.defaults.headers.common["Content-Type"] = "application/json";

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
