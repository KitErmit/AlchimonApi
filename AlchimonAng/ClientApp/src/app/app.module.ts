import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';  

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavMenuService } from './nav-menu/nav-menu.service';
import { UserComponent } from './user/user.component';
import { ProfileComponent } from './profile/profile.component';
import { AdminComponent } from './admin/admin.component';
import { HttpService } from './services/http.service';
import { WhelcomeService } from './services/whelcome.service';
import { AdminService } from './services/admin.service';
import { NavGuard } from './guards/my.guard';
import { AdminGuard } from './guards/admin.guard';

import { AdminChildComponent } from './admin/admin-child.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UserComponent,
    ProfileComponent,
    AdminComponent,
    AdminChildComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    CommonModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: UserComponent},
      { path: 'authorize', component: UserComponent },
      { path: 'my-profile', component: ProfileComponent, canActivate: [NavGuard] },
      { path: 'admin', component: AdminComponent, canActivate: [AdminGuard] },
    ])
  ],
  providers: [NavMenuComponent, NavGuard, AdminGuard, HttpService, NavMenuService, WhelcomeService, AdminService],
  bootstrap: [AppComponent]
})
export class AppModule { }

