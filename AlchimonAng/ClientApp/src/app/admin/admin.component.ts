import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { SimpleResp } from '../profile/profile.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent {

  test: string = "1";
  roster: SimpleResp[] | undefined;
  constructor(private http: HttpClient, private router: Router) { }




  ngOnInit() {
    if (localStorage.getItem("AlToken") === null || localStorage.getItem("AlToken") === undefined) this.router.navigateByUrl("/my-profile");
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    
    this.http.get('https://localhost:7170/Admin/rolecheck', { headers: myHeaders })
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
          this.router.navigateByUrl("/my-profile");

        }
      });
  }




  updatedata() {
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.get('https://localhost:7170/Admin/GetRoster', { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          this.roster = data;
        },
        error: error => {
          console.log(error);
        }
      });
  }

  delet(item: SimpleResp) {
    const body = { good: true, text: item.id };
    const myHeaders = new HttpHeaders().set('Authorization', <string>localStorage.getItem("AlToken"));
    this.http.post('https://localhost:7170/Admin/delet', body, { headers: myHeaders })
      .subscribe({
        next: (data: any) => {
          if (data.good) {
            this.updatedata();
            console.log(data.text);
          }
          else alert(data.text);
        },
        error: error => {
          console.log(error);
        }
      });
  }

}


//delet