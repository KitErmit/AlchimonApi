import { Component, Inject, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../models/user'



@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})

export class AdminComponent implements OnInit, OnChanges  {
  name: string = "";
  test: string = "1";
  roster: User[] | undefined;
  fullroser: User[] | undefined;
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem("AlToken") === null || localStorage.getItem("AlToken") === undefined) this.router.navigateByUrl("/my-profile");
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));


    this.http.get('https://localhost:7170/Admin/rolecheck', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          if (data.good) {
            this.test = data.text;
            this.updatedata();
          }
          else this.router.navigateByUrl("/my-profile");

        },
        error: error => {
          console.log(error);
          this.router.navigateByUrl("/my-profile");
        }
      });
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(`changes in parant`);

  }

  




  updatedata() {
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/Admin/GetRoster', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          this.roster = data;
          this.fullroser = data;
        },
        error: error => {
          console.log(error);
        }
      });
  }

  delet(item: User) {
    const body = { good: true, text: item.id };
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.post('https://localhost:7170/Admin/delet', body, { headers: Headers })
      .subscribe({
        next: (data: any) => {
          if (data.good) {
            this.updatedata();
            this.name = "";
            console.log(data.text);
          }
          else alert(data.text);
        },
        error: error => {
          console.log(error);
        }
      });
  }

  
  
}

/*
ngOnChanges(changes: SimpleChanges) {
  for (let propName in changes) {

    var here = changes[propName];
    let cur = JSON.stringify(here.currentValue);
    let prev = JSON.stringify(here.previousValue);
    console.log(`${propName}: currentValue = ${cur}, previousValue = ${prev}`);
    if (here.currentValue !== "" || here.currentValue !== " " || here.currentValue !== null || here.currentValue !== undefined) {

      this.roster = this.fullroser?.filter(function (value: User, index: number, thisArray: User[]) => {
         return value.id.startsWith(here.currentValue);
       }); 
    }

  }
}
*/