import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class HttpService {

  constructor(private http: HttpClient) { }

  getAuthBool(): Observable<boolean> {
    var token = localStorage.getItem("AlToken");
    if (token === null || token === undefined) token = "sleep";
    const myHeaders = new HttpHeaders().set('Authorization', token);
    return this.http.get('https://localhost:7170/User/AuthValid', { headers: myHeaders }).pipe(map((data: any) => {
      var authable = data.good;
      return authable;
    }));
  }
}

/*
 return authable.map(function (boo: any): boolean {
        return boo;
        )};
*/