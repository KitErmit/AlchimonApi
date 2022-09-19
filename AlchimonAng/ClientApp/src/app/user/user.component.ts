import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { map, catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from './model';
import { TokenResp } from './tokenresp';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'

})
export class UserComponent {

  servresp: TokenResp = new TokenResp("Pusto", false);
  errorMes: string = "Pusto";
  done: boolean = false;
  reg: boolean = false;
  auth: boolean = true;
  passdone: boolean = false;
  otpr: User = new User("", "", "");
  user: User | undefined;

  constructor(private http: HttpClient, private router: Router) { }

  authform() {
    this.reg = false;
    this.auth = true;
  }
  regform() {
    this.reg = true;
    this.auth = false;
  }



  authsubmit(otpr: User) {
    const Headers = new HttpHeaders().set('Accept', 'application/json').set('Content-Type', 'application/json');
    const body = { email: otpr.email, password: otpr.password, passconf: otpr.passconf };
    this.http.post('https://localhost:7170/User/Authorize', body, { headers: Headers })
      .subscribe({
        next: (data: any) => {
          this.servresp = new TokenResp(data.text, data.good);
          if (this.servresp.good) {
            this.passdone = false;
            this.done = true;
            this.auth = false;
            localStorage.setItem("AlToken", "Bearer " + this.servresp.text);
            this.servresp.text = "КРАСАВЧЕГ" + <string>localStorage.getItem("AlToken");
            this.router.navigateByUrl("/my-profile");
          }
          else {
            this.passdone = true;
            this.errorMes = this.servresp.text;
          }
        },
        error: error => { console.log; this.errorMes = error; this.passdone = true }
      });
  }


  submit(otpr: User) {
    if (otpr.password === otpr.passconf) {

      const myHeaders = new HttpHeaders().set('Accept', 'application/json').set('Content-Type', 'application/json');
      const body = { email: otpr.email, password: otpr.password, passconf: otpr.passconf };
      this.http.post('https://localhost:7170/User/Registration', body, { headers: myHeaders })
        .subscribe({
          next: (data: any) => {

            var arr = <string[]>data.text.split(" ");
            this.servresp = new TokenResp(arr[arr.length - 1], data.good);
            this.errorMes = data.text;
            if (this.servresp.good) {
              this.done = true;
              this.passdone = false;
              this.reg = false;
              localStorage.setItem("AlToken", "Bearer " + this.servresp.text);
              this.servresp.text = <string>localStorage.getItem("AlToken");
              this.router.navigateByUrl("/my-profile");
            }
            else {
              this.passdone = true;
            }
          },

          error: error => { this.errorMes = JSON.stringify(error); this.passdone = true }
        });
    }
    else {
      this.servresp.text = "Пароли не совпадают";
      this.passdone = true;
    }
    
  }


  ngOnInit() {
    var stor = localStorage.getItem("AlToken");
    if (stor !== null || stor !== undefined) {
      const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
      this.http.get('https://localhost:7170/User/AuthValid', { headers: Headers })
        .subscribe({
          next: (data: any) => {
            if (data.good) this.router.navigateByUrl("/my-profile");
          },
          error: error => { this.errorMes = "DADA" + error; this.passdone = true }
        });
    }
    
  }
}

