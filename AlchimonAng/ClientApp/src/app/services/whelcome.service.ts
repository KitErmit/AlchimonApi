import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { BoolText } from '../models/bool-text';
import { Enter } from '../models/enter';

@Injectable()
export class WhelcomeService {
  errorMess: string = "";
  resp: BoolText | undefined;
  Url: string = "https://localhost:7170/";
  constructor(private http: HttpClient) { this.http.get('assets/url.json').subscribe({ next: (data: any) => this.Url = data.defUrl }); }

  tryEnter(authorize: boolean, model: Enter): Observable<BoolText> {
    var fullUrl = this.Url;
    if (authorize) fullUrl = fullUrl + 'User/Authorize';
    else fullUrl = fullUrl + 'User/Registration';
    const Headers = new HttpHeaders().set('Accept', 'application/json').set('Content-Type', 'application/json');
    const body = { email: model.email, password: model.password, passConf: model.passconf };

    return this.http.post(fullUrl, body, { headers: Headers }).pipe(map((data: any) => {

      return this.resp = new BoolText(data.text, data.good);
    }), catchError(err => {
      console.log(err);
      this.errorMess = err.error;
      this.resp = new BoolText("", false);
      return of(this.resp);
    }));
  }
}