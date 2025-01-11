export interface AppointmentPost {
    patientId: string;
    doctorId: string;
    date: string; // ISO string format
    reason: string;
}