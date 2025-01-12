import { Component, OnInit } from '@angular/core';
import { AppointmentService, UpdateStatusPayload } from '../../../services/appointment/appointment.service';
import { AuthService } from '../../../services/auth.service';
import { DoctorService } from '../../../services/doctor/doctor.service';
import { PatientService } from '../../../services/patient/patient.service';
import { Appointment } from '../../../models/appointment.model';
import { UserRole } from '../../../UserRole';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { forkJoin, of } from 'rxjs';
import { catchError, switchMap, map } from 'rxjs/operators';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { Router } from '@angular/router';
import { AppointmentStatus } from '../../../AppointmentStatus';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ModalComponent } from '../../shared/modal/modal.component';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MedicalHistory } from '../../../models/medical-history.model';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [
    MatCardModule, 
    CommonModule, 
    MatButton, 
    MatIcon,
    MatSnackBarModule,
    ModalComponent,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {
  appointments: Appointment[] = [];
  userRole: UserRole = UserRole.None;
  UserRole = UserRole;
  public AppointmentStatus = AppointmentStatus;
  selectedFiles: File[] = [];

  // Modal related properties
  isCompleteModalVisible: boolean = false;
  selectedAppointment: Appointment | null = null;
  completeForm: FormGroup;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private router: Router,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.completeForm = this.fb.group({
      dateRecorded: [{ value: '', disabled: true }, Validators.required],
      diagnosis: ['', Validators.required],
      medication: [''],
      notes: [''],
      attachments: [''] 
    });
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole();
    console.log('User Role:', this.userRole);
    this.fetchAppointments();
  }

  fetchAppointments(): void {
    let fetchObservable;

    if (this.userRole === UserRole.Patient) {
      const patientId = this.authService.getUserId();
      if (!patientId) {
        console.error('Patient ID is missing');
        return;
      }
      fetchObservable = this.appointmentService.getAppointmentsByPatientId(patientId);
    } else if (this.userRole === UserRole.Doctor) {
      const doctorId = this.authService.getUserId();
      if (!doctorId) {
        console.error('Doctor ID is missing');
        return;
      }
      fetchObservable = this.appointmentService.getAppointmentsByDoctorId(doctorId);
    }

    if (fetchObservable) {
      fetchObservable.pipe(
        switchMap((appointments: Appointment[]) => {
          this.appointments = appointments;
          console.log('Fetched Appointments:', this.appointments);

          const uniqueDoctorIds = [...new Set(appointments.map(appt => appt.doctorId))];
          const uniquePatientIds = [...new Set(appointments.map(appt => appt.patientId))];

          const doctorObservables = uniqueDoctorIds.map(id =>
            this.doctorService.getDoctorById(id).pipe(
              catchError(err => {
                console.error(`Error fetching doctor with ID ${id}:`, err);
                return of({ userId: id, firstName: 'Unknown', lastName: '' });
              })
            )
          );

          const patientObservables = uniquePatientIds.map(id =>
            this.patientService.getPatientById(id).pipe(
              catchError(err => {
                console.error(`Error fetching patient with ID ${id}:`, err);
                return of({ userId: id, firstName: 'Unknown', lastName: '' });
              })
            )
          );

          return forkJoin([...doctorObservables, ...patientObservables]);
        }),
        map((users: any[]) => {
          const doctorMap = new Map<string, string>();
          const patientMap = new Map<string, string>();

          users.forEach(user => {
            if ('userId' in user && user.firstName && user.lastName) {
              const fullName = `${user.firstName} ${user.lastName}`;
              doctorMap.set(user.userId, fullName);
              patientMap.set(user.userId, fullName);
            }
          });

          console.log('Doctor Map:', doctorMap);
          console.log('Patient Map:', patientMap);

          this.appointments.forEach(appt => {
            if (this.userRole === UserRole.Patient) {
              appt.doctorName = doctorMap.get(appt.doctorId) || 'Unknown';
            } else if (this.userRole === UserRole.Doctor) {
              appt.patientName = patientMap.get(appt.patientId) || 'Unknown';
            }
          });

          this.appointments.sort((a, b) => new Date(a.appointmentDate).getTime() - new Date(b.appointmentDate).getTime());
          return this.appointments;
        })
      ).subscribe({
        next: (appointments) => {
          this.appointments = appointments;
          console.log('Appointments with names:', this.appointments);
        },
        error: (error) => {
          console.error('Error fetching appointments:', error);
        }
      });
    }
  }

  getStatusText(status: AppointmentStatus): string {
    switch(status) {
      case AppointmentStatus.Scheduled:
        return 'Scheduled';
      case AppointmentStatus.Completed:
        return 'Completed';
      case AppointmentStatus.Canceled:
        return 'Canceled';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: AppointmentStatus): string {
    switch(status) {
      case AppointmentStatus.Scheduled:
        return 'status-scheduled';
      case AppointmentStatus.Completed:
        return 'status-completed';
      case AppointmentStatus.Canceled:
        return 'status-canceled';
      default:
        return '';
    }
  }

  /**
   * Cancel an appointment.
   * @param appointmentId - The ID of the appointment to cancel.
   */
  cancelAppointment(appointmentId: string): void {
    console.log(`Cancel Appointment: ${appointmentId}`);
    
    const payload: UpdateStatusPayload = {
      appointmentId: appointmentId,
      newStatus: 2 // Assuming 2 represents 'Canceled'
    };
    
    this.appointmentService.updateAppointmentStatus(payload).subscribe({
      next: () => {
        console.log('Appointment canceled successfully.');
        this.snackBar.open('Appointment canceled successfully.', 'Close', {
          duration: 3000,
        });
        this.fetchAppointments(); // Refresh the appointment list
      },
      error: (error) => {
        console.error('Error canceling appointment:', error);
        this.snackBar.open('Failed to cancel appointment.', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  /**
   * Open the Complete Appointment Modal.
   * @param appointment - The appointment to mark as completed.
   */
  markAsCompleted(appointment: Appointment): void {
    console.log(`Mark as Completed: ${appointment.id}`);
    
    // Calculate DateRecorded as appointmentDate + 30 minutes
    const appointmentDate = new Date(appointment.appointmentDate);
    const dateRecorded = new Date(appointmentDate.getTime() + 30 * 60000); // Add 30 minutes

    this.selectedAppointment = appointment;
    this.isCompleteModalVisible = true;

    // Initialize the form with computed DateRecorded
    this.completeForm.setValue({
      dateRecorded: dateRecorded.toISOString(),
      diagnosis: '',
      medication: '',
      notes: '',
      attachments: ''
    });
  }

  /**
   * Handle the form submission from the modal.
   */
  onSubmitComplete(): void {
    if (this.completeForm.valid && this.selectedAppointment) {
      const statusPayload: UpdateStatusPayload = {
        appointmentId: this.selectedAppointment.id,
        newStatus: 1 // Assuming 1 represents 'Completed'
      };

      const medicalHistoryPayload: MedicalHistory = {
        appointmentId: this.selectedAppointment.id,
        diagnosis: this.completeForm.get('diagnosis')?.value,
        medication: this.completeForm.get('medication')?.value,
        notes: this.completeForm.get('notes')?.value,
      };

      // First POST request: Update Appointment Status
      this.appointmentService.updateAppointmentStatus(statusPayload).pipe(
        // After updating status, submit medical history
        switchMap(() => {
          // Prepare FormData for medical history with attachments
          const formData = new FormData();
          formData.append('appointmentId', medicalHistoryPayload.appointmentId);
          formData.append('diagnosis', medicalHistoryPayload.diagnosis);
          formData.append('medication', medicalHistoryPayload.medication);
          formData.append('notes', medicalHistoryPayload.notes);

          // Append each selected file
          this.selectedFiles.forEach((file, index) => {
            formData.append(`attachments`, file, file.name);
          });

          return this.appointmentService.submitMedicalHistory(formData).pipe(
            catchError(err => {
              console.error('Error submitting medical history:', err);
              this.snackBar.open('Failed to submit medical history.', 'Close', {
                duration: 3000,
              });
              return of(null); // Continue the observable chain
            })
          );
        })
      ).subscribe({
        next: () => {
          this.snackBar.open('Appointment completed successfully.', 'Close', {
            duration: 3000,
          });
          this.isCompleteModalVisible = false;
          this.selectedAppointment = null;
          this.selectedFiles = []; // Reset selected files
          this.completeForm.reset(); // Reset the form
          this.fetchAppointments(); // Refresh the appointment list
        },
        error: (error) => {
          console.error('Error completing appointment:', error);
          this.snackBar.open('Failed to complete appointment.', 'Close', {
            duration: 3000,
          });
        }
      });
    } else {
      this.snackBar.open('Please complete all required fields.', 'Close', {
        duration: 3000,
      });
    }
  }

  /**
   * Close the Complete Appointment Modal without completing.
   */
  closeCompleteModal(): void {
    this.isCompleteModalVisible = false;
    this.selectedAppointment = null;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const filesArray = Array.from(input.files);
      if (filesArray.length + this.selectedFiles.length > 5) {
        this.snackBar.open('You can only upload a maximum of 5 files.', 'Close', {
          duration: 3000,
        });
        return;
      }
      this.selectedFiles.push(...filesArray);
    }
  }

  /**
   * Mark an appointment as canceled.
   * @param appointmentId - The ID of the appointment to cancel.
   */
  markAsCanceled(appointmentId: string): void {
    console.log(`Mark as Canceled: ${appointmentId}`);
    this.cancelAppointment(appointmentId);
  }

  backToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
}