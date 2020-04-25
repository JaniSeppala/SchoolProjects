import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthenticationService,
    private router : Router,
    private dbService: DatabaseService) {
    this.loginForm = this.formBuilder.group({
      username: '',
      pswd: ''
    });
  }

  ngOnInit() {
    if (this.authService.loggedIn) {
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit(data) {
    this.authService.authenticateUser(data).subscribe(response => {
      if (response.token) {
        this.authService.token = response.token;
        this.authService.loggedIn = true;
        this.authService.user = data;
        let workspaces = [];
        this.dbService.getUsersWorkspaces().subscribe((result)=>{
          workspaces = result;
          if (workspaces.length > 0) {
            workspaces.forEach((workspace)=>{
              this.dbService.getWorkspacePages(workspace.workspaceID).subscribe(result=>{
                workspace.pages = result;
              });
              workspace.isOpen = false;
            });
            this.authService.navlist = workspaces;
          }
        });
        this.router.navigate(['/dashboard']);
      } else {
        alert('Väärä käyttäjätunnus tai salasana!');
      }
    });
  }

}
