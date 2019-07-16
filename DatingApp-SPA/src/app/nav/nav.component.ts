import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { error } from 'util';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; // empty object
// here we create model that we store username and pass to server.
// tther is two parts in angular from
// the first one is in html from
// the scond one is here.

  constructor(private authService: AuthService) { }  //to use the service frist we inject it into the constrcort

  ngOnInit() {
  }
  Login() { 
    this.authService.Login(this.model).subscribe(next =>{

      console.log("logged sueccess");
    }, error => {
      console.log("falied to login");
    });

  }

  LoggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // thos means true or false !! its shortcut of if statment. 
  }

  LoggedOut() {
    localStorage.removeItem('token');
    console.log("logged out");
  }
}
