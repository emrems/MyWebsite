import axios from 'axios';

// Backend API adresinizi buraya yazın
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
  }
};

export default ApiService;