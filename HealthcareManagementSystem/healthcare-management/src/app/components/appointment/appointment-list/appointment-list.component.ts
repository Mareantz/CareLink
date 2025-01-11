import { Component, OnInit } from '@angular/core';
import { AppointmentService } from '../../../services/appointment/appointment.service';
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


@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [MatCardModule, CommonModule, MatButton, MatIcon],
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {
  appointments: Appointment[] = [];
  userRole: UserRole = UserRole.None;
  UserRole = UserRole;
  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private router: Router
  ) {}

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
                return of({ id: id, firstName: 'Unknown', lastName: '' });
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
            }

            if ('userId' in user && user.firstName && user.lastName) {
              const fullName = `${user.firstName} ${user.lastName}`;
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
          console.log('Appointments with names:', this.appointments);
        },
        error: (error) => {
          console.error('Error fetching appointments:', error);
        }
      });
    }
  }

  
  cancelAppointment(appointmentId: string): void {
    console.log(`Cancel Appointment: ${appointmentId}`);
  }

  
  markAsCompleted(appointmentId: string): void {
    console.log(`Mark as Completed: ${appointmentId}`);
  }

  
  markAsCanceled(appointmentId: string): void {
    console.log(`Mark as Canceled: ${appointmentId}`);
  }

  backToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
}