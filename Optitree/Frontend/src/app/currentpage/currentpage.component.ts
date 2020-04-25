import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-currentpage',
  templateUrl: './currentpage.component.html',
  styleUrls: ['./currentpage.component.css']
})
export class CurrentpageComponent implements OnInit {

  workspace;
  page;

  constructor(
    private route: ActivatedRoute,
    private dbService: DatabaseService) {
      this.workspace = {};
    }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const workspacename = params['workspacename'];
      const pageID = params['pageID'];
      this.dbService.getWorkspace(workspacename).subscribe((result)=>{
        this.workspace = result[0];
        this.dbService.getPage(pageID).subscribe((result)=>{
          this.page = result[0];
        });
      });
    });
    
  }

}
