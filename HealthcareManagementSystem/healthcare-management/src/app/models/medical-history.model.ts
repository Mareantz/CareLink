export interface MedicalHistory {
    appointmentId: string;
    diagnosis: string;
    medication: string;
    notes: string;
    // Attachments are handled via FormData
  }