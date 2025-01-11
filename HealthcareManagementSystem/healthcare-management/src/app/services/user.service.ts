import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { currentEnvironment } from '../environment.prod';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = currentEnvironment.apiUrl;

  constructor(private http: HttpClient) {}

  getPatientById(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/patients/${id}`);
  }

  getDoctorById(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/doctors/${id}`);
  }
}
