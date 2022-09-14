import { Component, Inject, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../models/user';



@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})

export class AdminComponent implements OnInit, OnChanges {
  name: string = "";

  ngOnInit() { }

  ngOnChanges(changes: SimpleChanges) {
    console.log("Parant changes");
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