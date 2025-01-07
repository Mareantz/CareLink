import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { currentEnvironment } from '../environment.prod';
import { Observable } from 'rxjs';
import { UserRole } from '../UserRole';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = currentEnvironment.apiUrl + '/api/Auth';
  constructor(private http: HttpClient, private router: Router) { }
  register(userData: any): Observable<any>{
    return this.http.post(this.apiUrl + '/register', userData);
  }
  login(userData: any): Observable<any>{
    return this.http.post(this.apiUrl + '/login', userData);
  }
  // Store the token in localStorage
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  // Retrieve the token from localStorage
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Clear the token (for logout)
  clearToken(): void {
    localStorage.removeItem('token');
  }
  getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }

  public getUserRole(): UserRole {
    const token = this.getToken();
    if (!token) {
      return UserRole.None;
    }
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      switch (payload.role) {
        case 'Admin': return UserRole.Admin;
        case 'Doctor': return UserRole.Doctor;
        case 'Patient': return UserRole.Patient;
        default: return UserRole.None;
      }
    } catch {
      return UserRole.None;
    }
  }

  public logout(): void {
  this.clearToken();
  this.router.navigate(['/login']);
}
}
