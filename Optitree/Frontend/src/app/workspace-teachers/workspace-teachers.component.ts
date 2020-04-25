import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatabaseService } from '../database.service';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-workspace-teachers',
  templateUrl: './workspace-teachers.component.html',
  styleUrls: ['./workspace-teachers.component.css']
})
export class WorkspaceTeachersComponent implements OnInit {

  workspace;
  empty = true;
  teachers;
  wsTeachers = [];
  addArray = [];
  removeArray = [];

  constructor(
    private route: ActivatedRoute,
    private dbService: DatabaseService,
    private authService: AuthenticationService,
    private router: Router) {
      this.workspace = {};//Alustetaan workspace ettei angulari itke konsolissa siitä, että sitä ei ole alustettu
    }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator && !this.authService.user.teacher) {
      return this.router.navigate(['/']);
    }
    this.route.params.subscribe(params => {
      const workspace = params['workspacename'];
      this.dbService.getWorkspace(workspace).subscribe((result)=>{
        this.workspace = result[0];
        this.getTeachers(this.workspace.workspaceID);
      });
    });
  }

  checkValueAdd(event: any) {
    if (event.currentTarget.checked) {
      this.addArray.push(event.currentTarget.name);
    } else {
      for (let i = 0; i < this.addArray.length; i++) {
        if (this.addArray[i] === event.currentTarget.name) {
          this.addArray.splice(i, 1);
          break;
        }
      }
    }
  }

  checkValueRemove(event: any) {
    if (event.currentTarget.checked) {
      this.removeArray.push(event.currentTarget.name);
    } else {
      for (let i = 0; i < this.removeArray.length; i++) {
        if (this.removeArray[i] === event.currentTarget.name) {
          this.removeArray.splice(i, 1);
          break;
        }
      }
    }
  }

  addTeachers() {
    if (this.addArray.length < 1) {
      return alert('Et ole valinnut yhtään käyttäjää!');
    }
    this.dbService.addWorkspaceTeachers(this.addArray, this.workspace.workspaceID).subscribe((result)=>{
      if (result.error) {
        return alert('Käyttäjien lisääminen työtilaan epäonnistui odottamattoman virheen vuoksi. Kokeile hetken päästä uudelleen.');
      }
      this.getTeachers(this.workspace.workspaceID);
    });
    
  }

  removeUsers() {
    if (this.removeArray.length < 1) {
      return alert('Et ole valinnut yhtään käyttäjää!');
    }
    this.dbService.removeWorkspaceUsers(this.removeArray, this.workspace.workspaceID).subscribe((result)=>{
      if (result.error) {
        return alert('Käyttäjien poistaminen työtilasta epäonnistui odottamattoman virheen vuoksi. Kokeile hetken päästä uudelleen.');
      }
      this.getTeachers(this.workspace.workspaceID);
    });
  }

  getTeachers(workspaceID) {
    this.dbService.getTeachers().subscribe((result) => {
      let teachers = result;
      this.dbService.getWorkspaceTeachers(workspaceID).subscribe((result)=>{
        let wsTeachers = result;
        this.wsTeachers = [];
        console.log(wsTeachers);
        if (wsTeachers.length > 0) {
          wsTeachers.forEach(wsTeacher => {
            for (let i = 0; i < teachers.length; i++) {
              const element = teachers[i].userID;
              if (element === wsTeacher.userID) {
                this.wsTeachers.push(...teachers.splice(i, 1));
                break;
              }
            }
          });
          this.empty = false;        
        } else {
          this.empty = true;
        }
        this.teachers = teachers;
      });
    });
  }

}
