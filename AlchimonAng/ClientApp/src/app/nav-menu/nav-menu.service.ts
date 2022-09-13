import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable()
export class NavMenuService {
  authorizeble: boolean = false;
  myname: string | undefined;

  constructor(private http: HttpClient) {
    this.updateFavicon();
  }

  trygetname() {
    var myhead: string = "asd";
    if (localStorage.getItem("AlToken") !== undefined || localStorage.getItem("AlToken") !== null)
      myhead = String(localStorage.getItem("AlToken"));
    const Headers = new HttpHeaders().set('Authorization', myhead);
    this.http.get('https://localhost:7170/User/AuthValid', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          if (data.good) {
            this.myname = data.text;
            this.authorizeble = true;
          }
          else {

            this.authorizeble = false;
            this.updateFavicon();
          }
        },
        error: error => console.log(error)
      });
  }

  updateFavicon() {
    this.http.get('assets/navconst.json').subscribe({ next: (data: any) => this.myname = data.name });
  }
}

