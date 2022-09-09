import { Component, Inject, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { SimpleResp } from '../profile/profile.component';



@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})

export class AdminComponent implements OnInit, OnChanges {
  @Input() name: string = "";
  test: string = "1";
  roster: SimpleResp[] | undefined;
  fullroser: SimpleResp[] | undefined;
  constructor(private http: HttpClient, private router: Router) { }

  

  




  updatedata() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/Admin/GetRoster', { headers: myHeaders })
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

  delet(item: SimpleResp) {
    const body = { good: true, text: item.id };
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.post('https://localhost:7170/Admin/delet', body, { headers: myHeaders })
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
  ngOnInit() {
    if (localStorage.getItem("AlToken") === null || localStorage.getItem("AlToken") === undefined) this.router.navigateByUrl("/my-profile");
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));


    this.http.get('https://localhost:7170/Admin/rolecheck', { headers: myHeaders })
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
    for (let propName in changes) {

      var here = changes[propName];
      let cur = JSON.stringify(here.currentValue);
      let prev = JSON.stringify(here.previousValue);
      console.log(`${propName}: currentValue = ${cur}, previousValue = ${prev}`);
      if (here.currentValue !== "" || here.currentValue !== " " || here.currentValue !== null || here.currentValue !== undefined) {

        this.roster = this.fullroser?.filter(function (value: SimpleResp, index: number, thisArray: SimpleResp[]) => {
          return value.id.startsWith(here.currentValue);
        });
      }

    }
  }

  
}


