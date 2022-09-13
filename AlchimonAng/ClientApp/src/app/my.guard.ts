import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from "@angular/router";
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpService } from './services/http.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { Observable, first } from "rxjs";
import { map } from 'rxjs/operators';
import { NavMenuService } from './nav-menu/nav-menu.service';

@Injectable()
export class NavGuard implements CanActivate {
  constructor(private http: HttpClient, private router: Router, private navmen: NavMenuService, private httpserv: HttpService) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    return this.httpserv.getAuthBool()
      .pipe(map((auth: boolean) => {
        if (auth) {
          this.navmen.trygetname();
          console.log('authenticated');
          return true;
        }
        this.navmen.trygetname();
        console.log('not authenticated');
        this.router.navigateByUrl('/authorize');
        return false;
      }));
   
  }
}

/*
subscribe({
  next: (data: boolean) => {
    var good = data;
    if (good) this.navmen.authorizeble = true;
    else this.navmen.authorizeble = false;
    return good;

  }
  
});



.map((auth: boolean) => {
        if (auth) {
          this.navmen.authorizeble = true;
          console.log('authenticated');
          return true;
        }
        console.log('not authenticated');
        this.router.navigateByUrl('/authorize');
        return false;
      }).first();
*/