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

  done: boolean = false;
  reg: boolean = true;
  auth: boolean = false;
  passdone: boolean = false;
  otpr: User = new User(" ", " ", " ");
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
    const myHeaders = new HttpHeaders().set('Accept', 'application/json').set('Content-Type', 'application/json');
    const body = { email: otpr.email, password: otpr.password, passconf: otpr.passconf };
    this.http.post('https://localhost:7170/User/Authorize', body, { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          this.servresp = new TokenResp(data.text, data.good);
          if (this.servresp.good) {
            this.passdone = false;
            this.done = true;
            this.auth = false;
            localStorage.setItem("AlToken", "Bearer " + this.servresp.text);

            this.servresp.text = "КРАСАВЧЕГ" + <string>localStorage.getItem("AlToken");
            this.router.navigateByUrl("/test");
          }
          else {
            this.passdone = true;
          }
        },
        error: error => console.log(error)
      });
  }


  submit(otpr: User) {
    if (otpr.password === otpr.passconf) {

      const myHeaders = new HttpHeaders().set('Accept', 'application/json').set('Content-Type', 'application/json');
      const body = { email: otpr.email, password: otpr.password, passconf: otpr.passconf };
      this.http.post('https://localhost:7170/User/Registration', body, { headers: myHeaders })
        .subscribe({
          next: (data: any) => {
            this.servresp = new TokenResp(data.text, data.good);
            if (this.servresp.good) {
              this.done = true;
              this.passdone = false;
              this.reg = false;
              localStorage.setItem("AlToken", "Bearer " + this.servresp.text);
              this.servresp.text = <string>localStorage.getItem("AlToken");
              this.router.navigateByUrl("/test");
            }
            else {
              this.passdone = true;
            }
          },

          error: error => console.log(error)
        });
    }
    else {
      this.servresp.text = "Пароли не совпадают";
      this.passdone = true;
    }
    
  }
}

