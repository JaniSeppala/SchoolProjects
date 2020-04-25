import { Component, OnInit } from '@angular/core';
import { DatabaseService } from '../database.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-workspace-pages',
  templateUrl: './workspace-pages.component.html',
  styleUrls: ['./workspace-pages.component.css']
})
export class WorkspacePagesComponent implements OnInit {

  workspace;
  pages: any[];

  constructor(
    private dbService: DatabaseService,
    private route: ActivatedRoute,
    private authService: AuthenticationService,
    private router: Router) {
      this.workspace = {};
    }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator && !this.authService.user.teacher) {
      return this.router.navigate(['/']);
    }
    this.route.params.subscribe(params => {
      const workspacename = params['workspacename'];
      this.dbService.getWorkspace(workspacename).subscribe((result)=>{
        this.workspace = result[0];
        this.getPages(this.workspace.workspaceID);
      });
    });

  }

  getPages(workspaceID) {
    this.dbService.getWorkspacePages(workspaceID).subscribe((result)=>{
      this.pages = result;
    });
  }

  onDelete(pageID) {
    if (confirm(`Haluatko varmasti poistaa sivun? Tätä toimintoa ei voi peruuttaa!`)) {
      this.dbService.deletePage(pageID).subscribe((result)=>{
        if (result.error) {
          return alert('Työtilan poisto epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
        }
        alert(`Sivun poisto onnistui!`);
        this.getPages(this.workspace.workspaceID);
      });
    }
  }

}
