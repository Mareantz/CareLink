import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environment.prod';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl + '/api/Auth';
  constructor(private http: HttpClient) { }
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
}
