import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './model';

@Component({
  selector: 'app-user',
  template: `<div>
                    <p>Имя пользователя: {{user?.name}}</p>
                    <p>Возраст пользователя: {{user?.age}}</p>
               </div>`

})
export class AppComponent {
  title = 'app';
  user: User | undefined;

  constructor(private http: HttpClient) { }

  ngOnInit() {

    this.http.get('assets/user.json').subscribe({ next: (data: any) => this.user = new User(data.name, data.age) });
  }
}

