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
 
  constructor(private http: HttpClient, private roueter: Router, private navService: NavMenuService) { }
  navServiceConcrate: NavMenuService = this.navService;

  ngOnInit() {

    this.navServiceConcrate.trygetname();

  }

  exit() {
    if (localStorage.getItem("AlToken") !== undefined || localStorage.getItem("AlToken") !== null)
      localStorage.removeItem("AlToken");
    this.navServiceConcrate.trygetname();
    this.roueter.navigateByUrl('');
  }

  


  isExpanded = false;

  collapse() {
    this.isExpanded = false;

  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

