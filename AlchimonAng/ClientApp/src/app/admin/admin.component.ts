import { Component, Inject, Input } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../models/user';



@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})

export class AdminComponent {
  name: string = "";

}
