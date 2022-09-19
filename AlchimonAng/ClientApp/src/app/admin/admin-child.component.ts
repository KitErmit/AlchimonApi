import { Component, Inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../models/user';



@Component({
  selector: 'admin-child',
  templateUrl: './admin-child.component.html'
})

export class AdminChildComponent implements  OnInit, OnChanges  {
  @Input() name: string = "";
  nameForCheck: string = "";
  test: string = "not ok";
  roster: User[] | undefined;
  fullroser: User[] | undefined;
  constructor(private http: HttpClient, private router: Router) { }

  updatedata() {
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/Admin/GetRoster', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          this.fullroser = data;
          if (this.nameForCheck === "" ) this.roster = data;
          else {
            this.roster = this.fullroser?.filter(item => item.nik.toUpperCase().startsWith(this.nameForCheck.toUpperCase()) );
            if (this.roster === undefined) {
              this.roster = this.fullroser;
            }
          }
          
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
        }
      });
  }
  

  ngOnChanges(changes: SimpleChanges) {


    if (changes.name.currentValue !== undefined) {
      this.roster = this.fullroser?.filter(item => item.nik.toUpperCase().startsWith(changes.name.currentValue.toUpperCase()));
      this.nameForCheck = changes.name.currentValue;
    }
    if (this.roster === undefined) {
      this.roster = this.fullroser;
    }

  }


  
}