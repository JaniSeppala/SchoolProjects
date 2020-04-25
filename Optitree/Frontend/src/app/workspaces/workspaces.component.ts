import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-workspaces',
  templateUrl: './workspaces.component.html',
  styleUrls: ['./workspaces.component.css']
})
export class WorkspacesComponent implements OnInit {

  workspaces = [];

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private dbService: DatabaseService) { }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator && !this.authService.user.teacher) {
      return this.router.navigate(['/']);
    }
    this.dbService.getWorkspaces().subscribe((results) => {
      this.workspaces = results;
    });
  }

  onDelete(workspacename) {
    if (confirm(`Haluatko varmasti poistaa työtilan ${workspacename}? Tätä toimintoa ei voi peruuttaa!`)) {
      this.dbService.deleteWorkspace(workspacename).subscribe((result)=>{
        if (result.error) {
          return alert('Työtilan poisto epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
        }
        alert(`Työtilan ${workspacename} poisto onnistui!`);
        this.dbService.getWorkspaces().subscribe((results) => {
          this.workspaces = results;
        });
      });
    }
  }

}
