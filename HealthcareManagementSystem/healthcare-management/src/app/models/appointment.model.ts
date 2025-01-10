import { AppointmentStatus } from '../AppointmentStatus';

export interface Appointment {
    id: string;
    appointmentDate: Date;
    reason: string;
    status: AppointmentStatus;
    doctorId: string;
    patientId: string;
}