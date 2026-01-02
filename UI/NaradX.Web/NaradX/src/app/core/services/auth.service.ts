import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { Router } from '@angular/router';

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

  login(credentials: any): Observable<any> {
    return this.apiService.post<any>('Auth/Login', credentials).pipe(map(AuthResponse => {
        // Assuming the API returns { token: '...', user: '...' }
        // Adjust based on actual API response
        if (AuthResponse && AuthResponse.token) {
           this.setAuth(AuthResponse);
        }
        return AuthResponse;
    }));
  }

  logout() {
    this.purgeAuth();
    this.router.navigate(['/auth/login']);
  }

  private setAuth(authData: any) {
    localStorage.setItem('authToken', authData.token);
    this.currentUserSubject.next(authData.user);
    this.isAuthenticatedSubject.next(true);
  }

  private purgeAuth() {
    localStorage.removeItem('authToken');
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }
}
