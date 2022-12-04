import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { environment } from './../../environments/environment';
import { UserViewModel } from '../model/userViewModel';
import { UserLoginInputModel } from '../model/userLoginInputModel';
import { UserLogin } from '../model/userLogin';
import { UserInputModel } from '../model/userInputModel';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
    providedIn: 'root'
  })
  export class UserService {
    baseUrl = environment.apiUrl + 'authentication/';
    jwtHelper = new JwtHelperService();
    decodedToken: any;
    currentUser: UserLogin = new UserLogin();
  
    constructor(private http: HttpClient) {
  
    }
  
    getToken() {
      return localStorage.getItem('token');
    }
  
    login(user: UserLoginInputModel) {
      return this.http.post<UserViewModel>(this.baseUrl + 'login', user)
        .pipe(
          map((response: UserViewModel) => {
            if (response) {
  
              localStorage.setItem('token', response.accessToken);
              this.currentUser.id = response.id;
              this.currentUser.username = response.username;
              this.decodedToken = this.jwtHelper.decodeToken(response.accessToken);
              this.currentUser.role = this.decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  
              localStorage.setItem('user', JSON.stringify(response));
  
            }
          })
        );
    }  

    register(userInput: UserInputModel) {
      return this.http.post(this.baseUrl + 'create', userInput)
    }  
   
    logout() {
      localStorage.removeItem('token');
      localStorage.removeItem('user');    
    }
  }
  
  