import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:3000/api'; // URL-ul backendului

  constructor(private http: HttpClient) {}

  getPatientById(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/patients/${id}`);
  }

  getDoctorById(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/doctors/${id}`);
  }
}
