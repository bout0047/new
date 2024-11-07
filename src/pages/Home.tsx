import React, { useState } from 'react';
import { useQuery, useMutation } from '@tanstack/react-query';
import { getRooms } from '../api/rooms';
import { createReservation } from '../api/reservations';
import { RoomCard } from '../components/RoomCard';
import { ReservationModal } from '../components/ReservationModal';
import { Room } from '../types';

export const Home: React.FC = () => {
  const [selectedRoom, setSelectedRoom] = useState<Room | null>(null);
  
  const { data: rooms, isLoading, error } = useQuery(['rooms'], getRooms);

  const createReservationMutation = useMutation(createReservation, {
    onSuccess: () => {
      setSelectedRoom(null);
    },
  });

  const handleReserve = (room: Room) => {
    setSelectedRoom(room);
  };

  const handleSubmitReservation = (data: any) => {
    if (!selectedRoom) return;

    const startDateTime = new Date(data.date + 'T' + data.startTime);
    const endDateTime = new Date(data.date + 'T' + data.endTime);

    createReservationMutation.mutate({
      roomId: selectedRoom.roomId,
      startTime: startDateTime.toISOString(),
      endTime: endDateTime.toISOString(),
      purpose: data.purpose,
    });
  };

  if (isLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">Loading rooms...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center text-red-500">Error loading rooms</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Available Rooms</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {rooms?.map((room) => (
          <RoomCard key={room.roomId} room={room} onReserve={handleReserve} />
        ))}
      </div>
      
      {selectedRoom && (
        <ReservationModal
          room={selectedRoom}
          onClose={() => setSelectedRoom(null)}
          onSubmit={handleSubmitReservation}
          isLoading={createReservationMutation.isLoading}
        />
      )}
    </div>
  );
};