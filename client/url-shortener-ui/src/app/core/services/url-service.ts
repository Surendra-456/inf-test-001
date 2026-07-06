import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class UrlService {

  constructor(private http: HttpClient) {}

  
createUrl(data: { originalUrl: string }) {
  const token = localStorage.getItem('jwt-token');

  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  return this.http.post<{ shortUrl: string }>(
    `${environment.apiUrl}/url/short`,
    data,
    { headers }
  );
}

}