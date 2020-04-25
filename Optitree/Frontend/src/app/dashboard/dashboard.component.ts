import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {


  user = {};//Houses the users information as an object

  constructor(
    private authService: AuthenticationService,
    private dbService: DatabaseService,
    private router: Router) { }

  //Gets the users info from the backend server

  ngOnInit() {
    //Redirect to login page if not loggedin
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/']);
    }
    //Fetches the user from the backend 
    this.dbService.getUser(this.authService.user.username)
    //Saves the user to the user parameter of the dashboard component
    .subscribe(response => {
      this.authService.user = response[0];
      this.user = response[0];
    });
  }

}
