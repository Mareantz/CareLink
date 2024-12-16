import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { currentEnvironment } from '../environment.prod';
import { AuthService } from './auth.service';

export interface UserData {
  age: number;
  gender: string;
  weight: number;
  bloodPressure: number;
  cholesterolLevel: number;
  physicalActivityLevel: number;
  smokingStatus: number;
  stressLevel: number;
}

@Injectable({
  providedIn: 'root'
})
export class PatientRiskPredictionService {
  private apiUrl = currentEnvironment.apiUrl + '/api/v1/UserRiskPrediction/predict';

  constructor(private http: HttpClient, private authService: AuthService) {}

  public predictRisk(userData: UserData): Observable<any> {
    return this.http.post<any>(this.apiUrl, userData, { headers: this.authService.getAuthHeaders() })
      .pipe(
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
