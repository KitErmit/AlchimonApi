import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from "@angular/router";
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { AdminService } from '../services/admin.service';
import { BoolText } from '../models/bool-text';

@Injectable()
export class AdminGuard implements CanActivate {
  constructor(private http: HttpClient, private router: Router, private adminServ: AdminService) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    return this.adminServ.roleCheck()
      .pipe(map((data: boolean) => {
        if (data) {
          return true;
        }
        this.router.navigateByUrl('/my-profile');
        return false;
      }));
   
  }
}