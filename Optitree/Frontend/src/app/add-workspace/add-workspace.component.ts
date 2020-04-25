import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-add-workspace',
  templateUrl: './add-workspace.component.html',
  styleUrls: ['./add-workspace.component.css']
})
export class AddWorkspaceComponent implements OnInit {

  workspaceForm;
  unavailable;

  constructor(
    private router: Router,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder) {
      this.workspaceForm = this.formBuilder.group({
        workspacename: ''
      });
      this.unavailable = false;
    }

  ngOnInit() {
  }

  checkWorkspacenameAvailability(workspacename) {
    if (!workspacename) {
      return
    }
    this.dbService.getWorkspace(workspacename).subscribe((result)=>{
      if (result.length > 0) {
        this.unavailable = true;
      } else {
        this.unavailable = false;
      }
    });
  }

  onSubmit(data) {
    this.dbService.addWorkspace(data).subscribe((result)=>{
      if (result.error) {
        return alert('Odottamaton virhe luodessa työtilaa. Kokeile hetken päästä uudelleen.');
      }
      alert('Työtila luotiin onnistuneesti!');
      this.router.navigate(['/workspaces']);
    });
  }

}
