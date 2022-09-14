import { Component, Inject, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../models/user'



@Component({
  selector: 'admin-child',
  template: '<p>Привет {{name}}</p>'
})

export class AdminChildComponent implements OnInit, OnChanges  {
  @Input() name: string = "";
  constructor(private http: HttpClient, private router: Router) { this.name = "kit"; }

  ngOnInit() {
    
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(`changes in child`);

  }

  


  
}

/*
ngOnChanges(changes: SimpleChanges) {
  for (let propName in changes) {

    var here = changes[propName];
    let cur = JSON.stringify(here.currentValue);
    let prev = JSON.stringify(here.previousValue);
    console.log(`${propName}: currentValue = ${cur}, previousValue = ${prev}`);
    if (here.currentValue !== "" || here.currentValue !== " " || here.currentValue !== null || here.currentValue !== undefined) {

      this.roster = this.fullroser?.filter(function (value: User, index: number, thisArray: User[]) => {
         return value.id.startsWith(here.currentValue);
       }); 
    }

  }
}
*/