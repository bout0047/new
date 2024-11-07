export interface Room {
  roomId: number;
  name: string;
  capacity: number;
  location: string;
  description?: string;
  isActive: boolean;
}

export interface Reservation {
  reservationId: number;
  roomId: number;
  userId: number;
  startTime: string;
  endTime: string;
  purpose: string;
  notes?: string;
  status: 'Pending' | 'Confirmed' | 'Cancelled';
}

export interface User {
  userId: number;
  name: string;
  email: string;
  role: 'Employee' | 'Manager' | 'Admin';
  isActive: boolean;
}