import React from 'react';
import { useForm } from 'react-hook-form';
import { Room } from '../types';

interface ReservationModalProps {
  room: Room;
  onClose: () => void;
  onSubmit: (data: any) => void;
  isLoading: boolean;
}

export const ReservationModal: React.FC<ReservationModalProps> = ({
  room,
  onClose,
  onSubmit,
  isLoading
}) => {
  const { register, handleSubmit, formState: { errors } } = useForm();

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
      <div className="bg-white rounded-lg p-6 max-w-md w-full">
        <h2 className="text-2xl font-bold mb-4">Reserve {room.name}</h2>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Date</label>
            <input
              type="date"
              {...register('date', { required: true })}
              className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
            />
            {errors.date && <span className="text-red-500 text-sm">Date is required</span>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">Start Time</label>
            <input
              type="time"
              {...register('startTime', { required: true })}
              className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
            />
            {errors.startTime && <span className="text-red-500 text-sm">Start time is required</span>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">End Time</label>
            <input
              type="time"
              {...register('endTime', { required: true })}
              className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
            />
            {errors.endTime && <span className="text-red-500 text-sm">End time is required</span>}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700">Purpose</label>
            <textarea
              {...register('purpose', { required: true })}
              className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              rows={3}
            />
            {errors.purpose && <span className="text-red-500 text-sm">Purpose is required</span>}
          </div>

          <div className="flex justify-end space-x-4">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isLoading}
              className="px-4 py-2 text-sm font-medium text-white bg-blue-500 rounded-md hover:bg-blue-600 disabled:opacity-50"
            >
              {isLoading ? 'Reserving...' : 'Reserve'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};