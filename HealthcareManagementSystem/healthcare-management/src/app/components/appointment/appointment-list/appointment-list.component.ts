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
import { MedicalHistoryService } from '../../../services/medical-history/medical-history.service';

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

  isCompleteModalVisible: boolean = false;
  selectedAppointment: Appointment | null = null;
  completeForm: FormGroup;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private medicalHistoryService: MedicalHistoryService,
    private router: Router,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.completeForm = this.fb.group({
      dateRecorded: ['', Validators.required],
      diagnosis: ['', Validators.required],
      medication: [''],
      notes: [''],
      attachments: [''] 
    });
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole();
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

  cancelAppointment(appointmentId: string): void {
    
    const payload: UpdateStatusPayload = {
      appointmentId: appointmentId,
      newStatus: 2
    };
    
    this.appointmentService.updateAppointmentStatus(payload).subscribe({
      next: () => {
        this.snackBar.open('Appointment canceled successfully.', 'Close', {
          duration: 3000,
        });
        this.fetchAppointments();
      },
      error: (error) => {
        console.error('Error canceling appointment:', error);
        this.snackBar.open('Failed to cancel appointment.', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  markAsCompleted(appointment: Appointment): void {
    
    const appointmentDate = new Date(appointment.appointmentDate);
    const dateRecorded = new Date(appointmentDate.getTime() + 30 * 60000);

    this.selectedAppointment = appointment;
    this.isCompleteModalVisible = true;

    this.completeForm.setValue({
      dateRecorded: dateRecorded.toISOString(),
      diagnosis: '',
      medication: '',
      notes: '',
      attachments: ''
    });
  }

  onSubmitComplete(): void {
    if (this.completeForm.valid && this.selectedAppointment) {
      const statusPayload: UpdateStatusPayload = {
        appointmentId: this.selectedAppointment.id,
        newStatus: 1 
      };
  
      const diagnosis = this.completeForm.get('diagnosis')?.value;
      const medication = this.completeForm.get('medication')?.value;
      const notes = this.completeForm.get('notes')?.value;
      const dateRecordedValue = this.completeForm.get('dateRecorded')?.value;
  
      if (!dateRecordedValue || isNaN(new Date(dateRecordedValue).getTime())) {
        this.snackBar.open('Invalid Date Recorded value.', 'Close', {
          duration: 3000,
        });
        return;
      }
  
      this.appointmentService.updateAppointmentStatus(statusPayload).pipe(
        switchMap(() => {
          const formData = new FormData();
  
          formData.append('Diagnosis', diagnosis);
          formData.append('Medication', medication);
          formData.append('DateRecorded', dateRecordedValue);
          formData.append('Notes', notes);
          formData.append('PatientId', this.selectedAppointment?.patientId || '');
          formData.append('AttachmentsPaths', '');
  
          this.selectedFiles.forEach((file) => {
            formData.append('Attachments', file, file.name);
          });
  
          return this.medicalHistoryService.submitMedicalHistory(formData);
        })
      ).subscribe({
        next: () => {
          this.snackBar.open('Appointment completed successfully.', 'Close', {
            duration: 3000,
          });
          this.isCompleteModalVisible = false;
          this.selectedAppointment = null;
          this.selectedFiles = []; 
          this.completeForm.reset(); 
          this.fetchAppointments(); 
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

  markAsCanceled(appointmentId: string): void {
    this.cancelAppointment(appointmentId);
  }

  backToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
}