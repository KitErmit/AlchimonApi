import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { NavMenuService } from '../nav-menu/nav-menu.service';
import { User } from '../models/user'


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {

  putable: boolean = false;
  resp: User = new User("NaN", " ", " ", " ", 0);
  bufer: User = new User("NaN", " ", " ", " ", 0);
  constructor(private http: HttpClient, private router: Router, private navmen: NavMenuService) { }

  ngOnInit() {
    if (localStorage.getItem("AlToken") === null || localStorage.getItem("AlToken") === undefined) this.router.navigateByUrl("/authorize");
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/AuthValid', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          if (data.good) this.updateProfile();
          else this.router.navigateByUrl("/authorize");
        },
        error: error => console.log(error)
      });
  }

  updateProfile() {
    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/getplayer', { headers: Headers })
      .subscribe({
        next: (data: any) => {
          this.resp = new User(data.id, data.nik, data.email, data.role, data.money);
          this.bufer = new User(this.resp.id, this.resp.nik, this.resp.email, this.resp.role, this.resp.money);
          this.navmen.trygetname();
        },
        error: error => {
          console.log(error);
          this.router.navigateByUrl("/authorize");
        }

      });
    
  }

  submit(bufer: User) {

    const Headers = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    const body = {
      id: bufer.id, nik: bufer.nik
    }
    this.http.put('https://localhost:7170/User/put_nik', body, { headers: Headers }).subscribe({
      next: (data: any) => {
        this.updateProfile();
        this.revet();
      },
      error: error => console.log(error)
    });
    
  }


  putForm() {
    this.putable = true;
    this.bufer = new User(this.resp.id, this.resp.nik, this.resp.email, this.resp.role, this.resp.money);
  }
  revet() {
    this.putable = false;
  }
}

