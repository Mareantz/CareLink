import { Injectable } from '@angular/core';
import { currentEnvironment } from '../../environment.prod';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Appointment } from '../../models/appointment.model';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';
import { map, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AppointmentPost } from '../../models/appointmentPost.model';

export interface UpdateStatusPayload {
  appointmentId: string;
  newStatus: number;
}

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private apiURL = currentEnvironment.apiUrl + '/api/v1/Appointments';

  constructor(private http: HttpClient, private authService : AuthService) {}

  private formatDate(date: string | Date): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (`0${d.getMonth() + 1}`).slice(-2); // Months are zero-based
    const day = (`0${d.getDate()}`).slice(-2);
    return `${year}.${month}.${day}`;
  }

  getAppointmentsByDoctorAndDate(doctorId: string, date: string | Date): Observable<Appointment[]> {
    const formattedDate = this.formatDate(date);
    const url = `${this.apiURL}/doctor/${doctorId}/date/${formattedDate}`;

    return this.http.get<any>(url, { headers: this.authService.getAuthHeaders() }).pipe(
      map(response => response.data), // Extract the data property
      catchError(error => {
        console.error('Error fetching appointments:', error);
        return throwError(() => new Error('Failed to fetch appointments.'));
      })
    );
  }

  getAppointmentsByPatientId(patientId: string): Observable<Appointment[]> {
    const url = `${this.apiURL}/patient/${patientId}`;

    return this.http.get<any>(url, { headers: this.authService.getAuthHeaders() }).pipe(
      map(response => response.data),
      catchError(error => {
        console.error('Error fetching appointments:', error);
        return throwError(() => new Error('Failed to fetch appointments.'));
      })
    );
  }

  getAppointmentsByDoctorId(doctorId: string): Observable<Appointment[]> {
    const url = `${this.apiURL}/doctor/${doctorId}`;

    return this.http.get<any>(url, { headers: this.authService.getAuthHeaders() }).pipe(
      map(response => response.data),
      catchError(error => {
        console.error('Error fetching appointments:', error);
        return throwError(() => new Error('Failed to fetch appointments.'));
      })
    );
  }

  createAppointment(appointment: AppointmentPost): Observable<Appointment> {
    return this.http.post<Appointment>(this.apiURL, appointment, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(error => {
        console.error('Error booking appointment:', error);
        return throwError(() => new Error('Failed to book appointment.'));
      })
    );
  }

  updateAppointmentStatus(payload: UpdateStatusPayload): Observable<any> {
    const url = `${this.apiURL}/update-status`;
    return this.http.put<any>(url, payload, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(error => {
        console.error('Error updating appointment status:', error);
        return throwError(() => new Error('Failed to update appointment status.'));
      })
    );
  }

}
