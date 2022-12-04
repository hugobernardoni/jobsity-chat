import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChatSignalRService } from 'src/app/_services/chatsignalr.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public userService: UserService, private router: Router, private chatSignalRService: ChatSignalRService) { }

  ngOnInit() {
  }

  goToPage(pageName: string) {
    this.router.navigate([`${pageName}`]);
  }

  logout() {
    this.userService.logout();
    this.chatSignalRService.stopConnection();
    this.goToPage('login');
  }

}
