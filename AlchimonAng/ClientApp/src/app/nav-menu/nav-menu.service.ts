import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable()
export class NavMenuService {
  authorizeble: boolean = false;
  myname: string = "AlchimonAng";
  constructor(private http: HttpClient) { }

  trygetname() {
    var myhead: string = "asd";
    if (localStorage.getItem("AlToken") !== undefined || localStorage.getItem("AlToken") !== null)
      myhead = String(localStorage.getItem("AlToken"));
    const myHeaders = new HttpHeaders().set('Authorization', myhead);
    this.http.get('https://localhost:7170/User/AuthValid', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          if (data.good) {
            console.log(data.text + "в trygetname. Тру");
            this.myname = data.text;
            this.authorizeble = true;
          }
          else {
            console.log("В trygetname. Фолс");

            this.authorizeble = false;
            this.myname = "AlchimonAng";
          }
        },
        error: error => console.log(error)
      });
  }

}