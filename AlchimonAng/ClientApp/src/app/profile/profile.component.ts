import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';

export class SimpleResp {
  constructor(public id: string, public nik: string, public email: string, public role: string, public money: number) { }
}

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {

  putable: boolean = false;

  resp: SimpleResp = new SimpleResp("Xyita", " ", " ", " ", 0);
  bufer: SimpleResp = new SimpleResp("Xyita", " ", " ", " ", 0);
  constructor(private http: HttpClient, private router: Router, private navmen: NavMenuComponent) { }

  ngOnInit() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/AuthValid', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          if (data.good) this.updateProfile();
          else this.router.navigateByUrl("/authorize");
        },
        error: error => console.log(error)
      });

    
  }

  updateProfile() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/getplayer', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          this.resp = new SimpleResp(data.id, data.nik, data.email, data.role, data.money);
          this.bufer = new SimpleResp(this.resp.id, this.resp.nik, this.resp.email, this.resp.role, this.resp.money);
          this.navmen.trygetname();
        },
        error: error => console.log(error)
      });
    
  }

  submit(bufer: SimpleResp) {

    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    const body = {
      id: bufer.id, nik: bufer.nik
    }
    this.http.put('https://localhost:7170/User/put_nik', body, { headers: myHeaders }).subscribe({
      next: (data: any) => {
        console.log(data.text);
        this.updateProfile();
        this.revet();
      },
      error: error => console.log(error)
    });
    
  }


  putForm() {
    this.putable = true;
    this.bufer = new SimpleResp(this.resp.id, this.resp.nik, this.resp.email, this.resp.role, this.resp.money);
  }
  revet() {
    this.putable = false;
  }
}

