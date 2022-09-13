import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable()
export class NavMenuService {
  myname: string = "AlchimonAng";
  constructor(private http: HttpClient) { }
  trygetname() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/AuthValid', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          if (data.good) this.myname = data.text;
        },
        error: error => console.log(error)
      });
  }

}