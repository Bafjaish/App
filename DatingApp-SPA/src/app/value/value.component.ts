import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
this.getValues(); // then after we dine below, we have to call the method here to run on the browser.
             }

             // here we add function to get the vlause from the api
             // the funtion get is observable and i can contain subscribtion which contains three overlods
             // -1 the resposne, 2the error, 3when resposen is has data.
 getValues(){
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
    this.values = response;
    }, error  => {
  console.log(error);
    });
 }
}
