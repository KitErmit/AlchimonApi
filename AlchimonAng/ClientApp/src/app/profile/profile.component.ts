import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export class SimpleResp {
  constructor(public id: string, public nik: string, public email: string, public role: string, public money: number) { }
}

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent {

  putable: boolean = false;

  resp: SimpleResp = new SimpleResp("Xyita", " ", " ", " ", 0);
  bufer: SimpleResp = new SimpleResp("Xyita", " ", " ", " ", 0);
  constructor(private http: HttpClient) { }

  ngOnInit() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/getplayer', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          this.resp = new SimpleResp(data.id, data.nik, data.email, data.role, data.money);
          this.bufer = new SimpleResp(this.resp.id, this.resp.nik, this.resp.email, this.resp.role, this.resp.money);
        },
        error: error => console.log(error)
      });
    
  }



  putForm() {
    this.putable = true;
  }
  revet() {
    this.putable = false;
  }
}

