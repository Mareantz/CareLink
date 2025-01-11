import { Injectable } from '@angular/core';
import { currentEnvironment } from '../../environment.prod';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../auth.service';
import { Doctor } from '../../models/doctor.model';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private apiURL = currentEnvironment.apiUrl + '/api/v1/Doctors';

  constructor(private http: HttpClient, private authService: AuthService) { }
  
  public getDoctors() : Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.apiURL, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(this.handleError)
    );
  }

  updateDoctor(id: string, doctorData: any): Observable<any> {
    return this.http.put<Doctor>(`${this.apiURL}/${id}`, doctorData, { headers: this.authService.getAuthHeaders() }).pipe(
          catchError(this.handleError));
  }
  
  

  public getDoctorById(id: string): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.apiURL}/${id}`, { headers: this.authService.getAuthHeaders() }).pipe(
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
