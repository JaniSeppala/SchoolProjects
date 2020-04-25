import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';
import { DatabaseService } from '../database.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-edit-page',
  templateUrl: './edit-page.component.html',
  styleUrls: ['./edit-page.component.css']
})
export class EditPageComponent implements OnInit {

  page;
  workspace;
  pageForm;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder) {
      this.pageForm = this.formBuilder.group({
        pageName: '',
        pageHTML: '',
        visible: false
      });
      this.page = this.pageForm.value;
    }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator && !this.authService.user.teacher) {
      return this.router.navigate(['/dashboard']);
    }
    let page;
    this.route.params.subscribe(params => {
      page = params['pageID'];
      this.workspace = params['workspacename'];
    });
    //Fetches the page from the backend 
    this.dbService.getPage(page)
    //Saves the page to the page parameter of the component
    .subscribe(response => {
      this.page = response[0];
      this.pageForm.setValue({
        pageName: this.page.pageName,
        pageHTML: this.page.pageHTML,
        visible: this.page.visible
      });
    });
  }

  onSubmit(data) {
    data.pageID = this.page.pageID;
    this.dbService.updatePage(data).subscribe((result) => {
      if (result.error) {
        return alert('Sivun päivitys epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
      }
      alert('Sivun tiedot päivitettiin onnistuneesti!');
      this.router.navigate(['/managepages', this.workspace]);
    });
  }

}
