import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserLogin } from './model/userLogin';
import { UserService } from './_services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  jwtHelper = new JwtHelperService();

  constructor(private userService: UserService) {
  }

  ngOnInit() {
      const token = localStorage.getItem('token');
      const user: UserLogin = JSON.parse(localStorage.getItem('user'));
      if (token) {
          this.userService.decodedToken = this.jwtHelper.decodeToken(token);
      }
      if (user) {
          this.userService.currentUser = user;
      }
  }

  logout() {
      this.userService.logout();
  }
}
