import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { map, catchError } from 'rxjs/operators';

import { HttpClient, HttpHeaders } from '@angular/common/http';

import { BoolText } from '../models/bool-text';
import { Enter } from '../models/enter';
import { WhelcomeService } from '../services/whelcome.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'

})
export class UserComponent {

  servresp: BoolText = new BoolText("Pusto", false);
  errorMes: string = "Pusto";
  done: boolean = false;
  reg: boolean = false;
  auth: boolean = true;
  passdone: boolean = false;
  otpr: Enter = new Enter("", "", "");
  user: Enter | undefined;

  constructor(private http: HttpClient, private router: Router, private whelcomeServ: WhelcomeService) { }

  authform() {
    this.reg = false;
    this.auth = true;
  }
  regform() {
    this.reg = true;
    this.auth = false;
  }



  authsubmit(otpr: Enter) {
    this.whelcomeServ.tryEnter(true, otpr).subscribe({
      next: (data: BoolText) => {
        if (data.good) {
          this.passdone = false;
          this.auth = false;
          localStorage.setItem("AlToken", "Bearer " + data.text);
          this.router.navigateByUrl("/my-profile");
        }
        else {
          this.passdone = true;
          this.errorMes = this.whelcomeServ.errorMess;
        }
      }
    });
  }


  submit(otpr: Enter) {
    if (otpr.password === otpr.passconf) {
      this.whelcomeServ.tryEnter(false, otpr).subscribe({
        next: (data: BoolText) => {
          if (data.good) {
            var arr = <string[]>data.text.split(" ");
            var token = arr[arr.length - 1];
            this.passdone = false;
            this.reg = false;
            localStorage.setItem("AlToken", "Bearer " + token);
            this.router.navigateByUrl("/my-profile");
          }
          else {
            this.passdone = true;
            this.errorMes = this.whelcomeServ.errorMess;
          }
        }
      });
    }
    else {
      this.servresp.text = "Пароли не совпадают";
      this.passdone = true;
    }
    
  }


  ngOnInit() {
    var stor = localStorage.getItem("AlToken");
    console.log(stor);
    if (stor !== null) {
      const Headers = new HttpHeaders().set('Authorization', <string>stor);
      this.http.get('https://localhost:7170/User/AuthValid', { headers: Headers })
        .subscribe({
          next: (data: any) => {
            if (data.good) this.router.navigateByUrl("/my-profile");
          },
          error: error => { this.errorMes = "DADA" + error; console.log(error); this.passdone = true }
        });
    }
    
  }
}

