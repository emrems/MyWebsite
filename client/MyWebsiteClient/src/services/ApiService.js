
import axios from 'axios';

const API_URL = 'https://localhost:7239/api'; 

const ApiService = {
 
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

 // response yakalama 
  async fetch(resource) {
    try {
      const response = await axios.get(`${API_URL}/${resource}`);
      return {
        success: response.data.isSuccess,
        data: response.data.isSuccess ? response.data.data : null,
        message: response.data.message,
        errorCode: response.data.errorCode
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: this.getErrorMessage(error),
        errorCode: error.response?.status || null
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
        errorCode: response.data.errorCode
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: this.getErrorMessage(error),
        errorCode: error.response?.status || null
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
        errorCode: response.data.errorCode
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: this.getErrorMessage(error),
        errorCode: error.response?.status || null
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
        errorCode: response.data.errorCode
      };
    } catch (error) {
      return {
        success: false,
        data: null,
        message: this.getErrorMessage(error),
        errorCode: error.response?.status || null
      };
    }
  },
  // hata yakalama status coduna gçre kullanıcıya uygun cevap verme
  getErrorMessage(error) {
    if (error.response) {
      // Sunucu yanıt verdi ama hata kodu döndü 
      const status = error.response.status;
      
      if (status === 404) {
        return 'İstenilen kaynak bulunamadı.';
      } else if (status === 401) {
        return 'Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.';
      } else if (status === 403) {
        return 'Bu işlem için yetkiniz bulunmamaktadır.';
      } else if (status === 500) {
        return 'Sunucu hatası oluştu. Lütfen daha sonra tekrar deneyin.';
      } else if (status >= 400 && status < 500) {
        return error.response.data?.message || 'İstek hatası oluştu.';
      } else if (status >= 500) {
        return 'Sunucu hatası. Lütfen daha sonra tekrar deneyin.';
      }
    } else if (error.request) {
     
      return 'Sunucuya bağlanılamıyor. Lütfen internet bağlantınızı kontrol edin veya sunucunun çalıştığından emin olun.';
    }
    
   
    return 'Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.';
  }
};

export default ApiService;