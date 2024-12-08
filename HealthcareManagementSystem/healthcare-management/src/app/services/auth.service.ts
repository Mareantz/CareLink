import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
}
