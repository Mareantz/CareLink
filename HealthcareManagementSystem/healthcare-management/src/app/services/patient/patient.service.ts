import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
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

  public getFilteredPatients(
    page: number,
    pageSize: number,
    firstName?: string,
    lastName?: string,
    gender?: string,
  ): Observable<{ data: { data: Patient[]; totalCount: number } }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    // Only set the parameter if the filter is a non-empty string after trimming
    if (firstName && firstName.trim().length > 0) {
      params = params.set('firstName', firstName.trim());
    }

    if (lastName && lastName.trim().length > 0) {
      params = params.set('lastName', lastName.trim());
    }

    if (gender && gender.trim().length > 0) {
      params = params.set('gender', gender.trim());
    }

    return this.http.get<{ data: { data: Patient[]; totalCount: number } }>(`${this.apiURL}/filtered`, {
      headers: this.authService.getAuthHeaders(),
      params: params
    }).pipe(
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
    return throwError(() => new Error(errorMessage));
  }
}