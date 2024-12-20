import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Patient } from '../../models/patient.model';
import { currentEnvironment } from '../../environment.prod'
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiURL = currentEnvironment.apiUrl + '/api/v1/Patients';

  constructor(private http: HttpClient, private authService : AuthService) { }

  public getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.apiURL, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  public createPatient(patient: Patient): Observable<any> {
    return this.http.post<Patient>(this.apiURL, patient, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  public updatePatient(id: string, patientData: any): Observable<any> {
    return this.http.put<Patient>(`${this.apiURL}/${id}`, patientData, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  public getPatientById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiURL}/${id}`, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  public deletePatientById(id: string): Observable<any> {
    return this.http.delete(`${this.apiURL}/${id}`, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side errors
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side errors
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}