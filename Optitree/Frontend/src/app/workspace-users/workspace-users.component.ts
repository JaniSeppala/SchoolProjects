import { Component, OnInit } from '@angular/core';
import { DatabaseService } from '../database.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-workspace-users',
  templateUrl: './workspace-users.component.html',
  styleUrls: ['./workspace-users.component.css']
})
export class WorkspaceUsersComponent implements OnInit {

  workspace;
  empty = true;
  users;
  wsUsers = [];
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
        this.getUsers(this.workspace.workspacename);
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

  addUsers() {
    if (this.addArray.length < 1) {
      return alert('Et ole valinnut yhtään käyttäjää!');
    }
    this.dbService.addWorkspaceUsers(this.addArray, this.workspace.workspaceID).subscribe((result)=>{
      if (result.error) {
        return alert('Käyttäjien lisääminen työtilaan epäonnistui odottamattoman virheen vuoksi. Kokeile hetken päästä uudelleen.');
      }
      this.getUsers(this.workspace.workspacename);
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
      this.getUsers(this.workspace.workspacename);
    });
  }

  getUsers(workspace) {
    this.dbService.getStudents().subscribe((result) => {
      let users = result;
      this.dbService.getWorkspaceUsers(workspace).subscribe((result)=>{
        let wsUsers = result;
        this.wsUsers = [];
        if (wsUsers.length > 0) {
          wsUsers.forEach(wsUser => {
            for (let i = 0; i < users.length; i++) {
              const element = users[i].userID;
              if (element === wsUser.userID) {
                this.wsUsers.push(...users.splice(i, 1));
                break;
              }
            }
          });
          this.empty = false;        
        } else {
          this.empty = true;
        }
        this.users = users;
      });
    });
  }

}
