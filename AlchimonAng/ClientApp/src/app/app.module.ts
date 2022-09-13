import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';  

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { UserComponent } from './user/user.component';
import { ProfileComponent } from './profile/profile.component';
import { NavMenuService } from './nav-menu/nav-menu.service';
import { HttpService } from './services/http.service';
import { NavGuard } from './my.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    UserComponent,
    ProfileComponent,
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
    ])
  ],
  providers: [NavMenuComponent, NavGuard, HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }

