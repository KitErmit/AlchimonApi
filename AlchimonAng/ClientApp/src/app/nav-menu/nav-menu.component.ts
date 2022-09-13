import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavMenuService } from '../nav-menu/nav-menu.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
@Injectable()
export class NavMenuComponent {
  authorizeble: boolean = false;
  myname: string = "AlchimonAng";
 
  constructor(private http: HttpClient, private roueter: Router, private navser: NavMenuService) { }
  navsercon: NavMenuService = this.navser;
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

  exit() {
    if (localStorage.getItem("AlToken") !== undefined || localStorage.getItem("AlToken") !== null)
      localStorage.removeItem("AlToken");
    this.authorizeble = false;
    this.myname = "AlchimonAng";
    this.roueter.navigateByUrl('');
  }

  authMenu(good: boolean) {
    this.authorizeble = good;
  }


  ngOnInit() {
    this.trygetname();
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;

  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

