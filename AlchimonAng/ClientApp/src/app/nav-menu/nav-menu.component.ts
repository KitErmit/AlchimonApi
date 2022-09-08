import { Component } from '@angular/core';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavMenuService } from '../nav-menu/nav-menu.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
@Injectable()
export class NavMenuComponent {
  constructor(private http: HttpClient, private navser: NavMenuService) {
    
  }

  navsercon: NavMenuService = this.navser;
  
  ngOnInit() {

    this.navsercon.trygetname();

  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

