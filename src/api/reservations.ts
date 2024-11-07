import api from './axios';
import { Reservation } from '../types';

export const createReservation = async (data: {
  roomId: number;
  startTime: string;
  endTime: string;
  purpose: string;
}): Promise<Reservation> => {
  const response = await api.post('/reservations', data);
  return response.data;
};