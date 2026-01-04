import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { Router } from '@angular/router';

import { AuthResponse } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  public isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();


  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    this.populate();
  }

  populate() {
    const token = localStorage.getItem('authToken');
    if (token) {
      // In a real app, we'd validate the token or decode it to get user info.
      // For now, assume if token exists, user is logged in.
      // We might want to call a /me endpoint here.
      this.isAuthenticatedSubject.next(true);
      // Mock user for now or decode token
      this.currentUserSubject.next({ name: 'User' });
    }
  }

  login(credentials: any): Observable<AuthResponse> {
    return this.apiService.post<AuthResponse>('Auth/Login', credentials).pipe(map(response => {
        if (response && response.accessToken) {
           this.setAuth(response);
        }
        return response;
    }));
  }

  logout() {
    this.purgeAuth();
    this.router.navigate(['/auth/login']);
  }

  private setAuth(authData: AuthResponse) {
    localStorage.setItem('authToken', authData.accessToken);
    this.currentUserSubject.next({ email: authData.email, id: authData.userId });
    this.isAuthenticatedSubject.next(true);
  }

  private purgeAuth() {
    localStorage.removeItem('authToken');
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }
}
