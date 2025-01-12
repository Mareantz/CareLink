import { Injectable } from '@angular/core';
import { currentEnvironment } from '../../environment.prod';
import { HttpClient } from '@angular/common/http';
import { MedicalHistory } from '../../models/medical-history.model';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../auth.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MedicalHistoryService {
  private apiURL = currentEnvironment.apiUrl + '/api/v1/MedicalHistory';

  constructor(private http: HttpClient, private authService: AuthService) { }

  submitMedicalHistory(formData: FormData): Observable<any> {
    console.log('Submitting medical history:', formData);
    return this.http.post(`${this.apiURL}`, formData, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(error => {
        console.error('Error submitting medical history:', error);
        return throwError(() => new Error('Failed to submit medical history.'));
      })
    );
  }


  getMedicalHistoryByPatientId(patientId: string): Observable<MedicalHistory[]> {
    const url = `${this.apiURL}/patient/${patientId}`;
    return this.http.get<MedicalHistory[]>(url, { headers: this.authService.getAuthHeaders() }).pipe(
      catchError(error => {
        console.error('Error fetching medical history:', error);
        return throwError(() => new Error('Failed to fetch medical history.'));
      })
    );
  }

}
