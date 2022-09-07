import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
@Injectable()
export class NavMenuComponent {
  myname: string = "AlchimonAng";
  constructor(private http: HttpClient) { }
  trygetname() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/User/AuthValid', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          if (data.good) this.myname = data.text;
        },
        error: error => console.log(error)
      });
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

