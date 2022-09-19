import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class AdminService {

  Url: string = "https://localhost:7170/";
  errorMess: string = "";

  constructor(private http: HttpClient) { }

  roleCheck(): Observable<boolean> {
    var fullUrl = this.Url + 'Admin/rolecheck';
    var token = localStorage.getItem("AlToken");
    if (token === null) token = ' ';
    const Headers = new HttpHeaders().set('Authorization', token);

    return this.http.get(fullUrl, { headers: Headers }).pipe(map((data: any) => {
      return data.good;
    }), catchError(err => {
      console.log(err);
      this.errorMess = err.error;

      return of(false);
    }));
  }
}
