import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  private formatErrors(error: any) {
    return throwError(() => error);
  }

  get<T>(path: string, params: HttpParams = new HttpParams()): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  put<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}${path}`, JSON.stringify(body), {
        headers: new HttpHeaders({ 
          'Content-Type': 'application/json' 
        })
      })
      .pipe(catchError(this.formatErrors));
  }

  post<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}${path}`, JSON.stringify(body), {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      })
      .pipe(catchError(this.formatErrors));
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}${path}`)
      .pipe(catchError(this.formatErrors));
  }

  getFile(path: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}${path}`, { responseType: 'blob' })
      .pipe(catchError(this.formatErrors));
  }

  postMultipart<T>(path: string, formData: FormData): Observable<T> {
     return this.http.post<T>(`${this.baseUrl}${path}`, formData)
       .pipe(catchError(this.formatErrors));
  }
}
