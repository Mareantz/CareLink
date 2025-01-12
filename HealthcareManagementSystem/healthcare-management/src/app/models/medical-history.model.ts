export interface MedicalHistory {
    appointmentId: string;
    patientId: string;
    diagnosis: string;
    medication: string;
    notes: string;
    dateRecorded: string;
    attachments: string[];
    // Attachments are handled via FormData
  }