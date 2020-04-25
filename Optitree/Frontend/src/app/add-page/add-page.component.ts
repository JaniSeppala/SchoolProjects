import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DatabaseService } from '../database.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-add-page',
  templateUrl: './add-page.component.html',
  styleUrls: ['./add-page.component.css']
})
export class AddPageComponent implements OnInit {
  page;
  pageForm;
  workspace;

  constructor(
    private router: Router,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute) {
      this.pageForm = this.formBuilder.group({
        pageName: '',
        pageHTML: '',
        visible: false,
      });
      this.page = this.pageForm.value;
      this.workspace = {};
    }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const workspacename = params['workspacename'];
      this.dbService.getWorkspace(workspacename).subscribe((result)=>{
        this.workspace = result[0];
      });
    });
  }

  onSubmit(data) {
    data.workspaceID = this.workspace.workspaceID;
    this.dbService.addWorkspacePage(data).subscribe((result)=>{
      if (result.error) {
        return alert('Odottamaton virhe lisätessä sivua. Kokeile hetken päästä uudelleen.');
      }
      alert('Sivu lisättiin onnistuneesti!');
      this.router.navigate(['/managepages', this.workspace.workspacename]);
    });
  }

}
