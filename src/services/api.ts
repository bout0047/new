import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL;

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const roomsApi = {
  getAll: () => api.get('/rooms').then(response => response.data),
  getById: (id: number) => api.get(`/rooms/${id}`).then(response => response.data),
};

export const reservationsApi = {
  getAll: () => api.get('/reservations').then(response => response.data),
  create: (data: any) => api.post('/reservations', data).then(response => response.data),
  update: (id: number, data: any) => api.put(`/reservations/${id}`, data).then(response => response.data),
  delete: (id: number) => api.delete(`/reservations/${id}`).then(response => response.data),
};

export const authApi = {
  login: (credentials: { email: string; password: string }) => 
    api.post('/users/login', credentials).then(response => response.data),
  register: (userData: any) => 
    api.post('/users/register', userData).then(response => response.data),
};