import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly tokenKey = 'jwt-token';

  isAuthenticated = signal(
    !!localStorage.getItem(this.tokenKey)
  );

  constructor(private http: HttpClient) {}

  register(request: any) {
    return this.http.post(
      `${environment.apiUrl}/auth/register`,
      request
    );
  }

  login(request: any) {
    return this.http.post<any>(
      `${environment.apiUrl}/auth/login`,
      request
    );
  }

  saveToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
    this.isAuthenticated.set(true);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.isAuthenticated.set(false);
  }
}