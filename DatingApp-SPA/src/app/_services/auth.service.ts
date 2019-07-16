import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { logging } from 'protractor';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

baseUrl = 'http://localhost:5000/api/auth/';

constructor(private Http: HttpClient) { }
//  we inject our http cilent

Login(model: any) { // model is took from the navbar

return this.Http.post(this.baseUrl + 'Login', model) // retun url,login andthe model
// the retun thing is token, how we deal with token
// rxjs to deal with token is used
   .pipe(

   map((response: any) => {
    const user = response;
    if (user) {
         localStorage.setItem('token', user.token);
        }
   })   // to use rxjs we we  pipe to deal with object from the comes from the service

   );
}
reister(model: any){
    return this.Http.post(this.baseUrl + 'register', model);

}

}
