import React from 'react';
import { Room } from '../types';

interface RoomCardProps {
  room: Room;
  onReserve: (room: Room) => void;
}

export const RoomCard: React.FC<RoomCardProps> = ({ room, onReserve }) => {
  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h3 className="text-xl font-semibold mb-2">{room.name}</h3>
      <div className="text-gray-600 mb-4">
        <p>Capacity: {room.capacity} people</p>
        <p>Location: {room.location}</p>
        {room.description && <p>Description: {room.description}</p>}
      </div>
      <button
        onClick={() => onReserve(room)}
        className="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 transition-colors"
      >
        Reserve Room
      </button>
    </div>
  );
};