import api from './axios';
import { LoginCredentials, AuthResponse } from '../types';

export const login = async (credentials: LoginCredentials) => {
  const response = await api.post<AuthResponse>('/users/login', credentials);
  return response.data;
};

export const register = async (data: any) => {
  const response = await api.post<AuthResponse>('/users/register', data);
  return response.data;
};