import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class HttpService {
  authable: boolean = false;
  constructor(private http: HttpClient) { }

  getAuthBool(): Observable<boolean> {
    var token = localStorage.getItem("AlToken");
    if (token === null || token === undefined) token = "sleep";
    const Headers = new HttpHeaders().set('Authorization', token);
    return this.http.get('https://localhost:7170/User/AuthValid', { headers: Headers }).pipe(map((data: any) => {
      this.authable = data.good;
      return this.authable;
    }), catchError(err => {
      console.log(err);
      return of(this.authable);
    }));
  }
}