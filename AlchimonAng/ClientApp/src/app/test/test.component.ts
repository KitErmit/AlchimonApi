import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export class SimpleResp {
  constructor(public id: string, public nik: string, public email: string, public role: string, public money: number) { }
}

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
})
export class TestComponent {
  resp: SimpleResp = new SimpleResp("Xyita", " ", " ", " ", 0);
  constructor(private http: HttpClient) { }

  ngOnInit() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/play', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          this.resp = new SimpleResp(data.id, data.nik, data.email, data.role, data.money)
        },
        error: error => console.log(error)
      });
    
  }
}

