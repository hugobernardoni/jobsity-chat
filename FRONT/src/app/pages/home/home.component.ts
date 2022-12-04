import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  goToPage(pageName:string){
    this.router.navigate([`${pageName}`]);
  }

  logout() {
    this.userService.logout();   
    this.goToPage('login');
  }

}
