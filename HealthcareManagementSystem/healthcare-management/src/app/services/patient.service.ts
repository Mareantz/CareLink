import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Patient } from '../models/patient.model';
import { environment } from '../environment.prod'

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private apiURL = environment.apiUrl + 'api/v1/Patients';

  constructor(private http: HttpClient) { }

  public getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.apiURL).pipe(
      catchError(this.handleError)
    );
  }

  public createPatient(patient: Patient): Observable<any> {
    return this.http.post<Patient>(this.apiURL, patient).pipe(
      catchError(this.handleError)
    );
  }

 public updatePatient(id: string, patientData: any): Observable<any> {
    return this.http.put<Patient>(`/api/patients/${id}`, patientData);
}



  public getPatientById(id: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiURL}/${id}`).pipe(
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