import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';
import { FormBuilder } from '@angular/forms';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.css']
})
export class WorkspaceComponent implements OnInit {

  workspace;
  workspaceForm;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder) {
      this.workspaceForm = this.formBuilder.group({
        workspacename: '',
      });
      this.workspace = this.workspaceForm.value;
    }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator) {
      return this.router.navigate(['/dashboard']);
    }
    let workspace;
    this.route.params.subscribe(params => {
      workspace = params['workspacename'];
    });
    //Fetches the user from the backend 
    this.dbService.getWorkspace(workspace)
    //Saves the user to the user parameter of the dashboard component
    .subscribe(response => {
      this.workspace = response[0];
      this.workspaceForm.setValue({
        workspacename: this.workspace.workspacename
      });
    });
  }

  onSubmit(data) {
    this.dbService.updateWorkspace(data, this.workspace).subscribe((result) => {
      if (result.error) {
        return alert('Työtilan muutosten tallennus epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
      }
      alert('Työtilan tiedot päivitettiin onnistuneesti!');
      this.router.navigate(['/workspaces']);
    });
  }

}
