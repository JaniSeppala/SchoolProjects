import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  apiURL;

  constructor(
    private http: HttpClient,
    private auth: AuthenticationService) {
      this.apiURL = 'http://localhost:3000/'
    }

  getUser(username): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'users/' + username, httpOptions);
  }

  getPage(pageID): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      pageID: pageID
    };
    return this.http.post<any>(this.apiURL + 'pages/findPage', data, httpOptions);
  }

  getUsers(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'users/allUsers', httpOptions);
  }

  getUsersWorkspaces(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'users/findWorkspaces', httpOptions);
  } 

  getStudents(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'users/allStudents', httpOptions);
  }

  getTeachers(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'users/allTeachers', httpOptions);
  }

  updateWorkspace(data, workspacename): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.put<any>(this.apiURL + 'workspaces/' + workspacename, data, httpOptions);
  }

  addUser(data): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.post<any>(this.apiURL + 'users/addUser', data, httpOptions);
  }

  addWorkspacePage(data) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.post<any>(this.apiURL + 'pages/addPage', data, httpOptions);
  }

  getWorkspace(workspacename): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'workspaces/' + workspacename, httpOptions);
  }

  getWorkspaces(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'workspaces/allWorkspaces', httpOptions);
  }

  getWorkspaceUsers(workspace): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.get<any>(this.apiURL + 'workspaces/findUsers/' + workspace, httpOptions);
  }

  getWorkspaceTeachers(workspace): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.post<any>(this.apiURL + 'workspaces/findTeachers', {wsID: workspace}, httpOptions);
  }

  getWorkspacePages(workspaceID): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      wsID: workspaceID
    }
    return this.http.post<any>(this.apiURL + 'pages/allWorkspacePages', data, httpOptions);
  }

  addWorkspace(data): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.post<any>(this.apiURL + 'workspaces/addWorkspace', data, httpOptions);
  }

  addWorkspaceUsers(array: any[], workspaceID): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      userArray: array,
      wsID: workspaceID
    }
    return this.http.post<any>(this.apiURL + 'workspaces/addUsers', data, httpOptions);
  }

  addWorkspaceTeachers(array: any[], workspaceID): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      userArray: array,
      wsID: workspaceID
    }
    return this.http.post<any>(this.apiURL + 'workspaces/addTeachers', data, httpOptions);
  }

  removeWorkspaceUsers(array: any[], workspaceID): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      userArray: array,
      wsID: workspaceID
    }
    return this.http.post<any>(this.apiURL + 'workspaces/removeUsers', data, httpOptions);
  }

  updateUser(data): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.put<any>(this.apiURL + 'users/' + data.username, data, httpOptions);
  }

  updatePage(data): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.put<any>(this.apiURL + 'pages/updatePage', data, httpOptions);
  }

  deleteUser(username): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.delete<any>(this.apiURL + 'users/' + username, httpOptions);
  }

  deleteWorkspace(workspacename): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    return this.http.delete<any>(this.apiURL + 'workspaces/' + workspacename, httpOptions);
  }

  deletePage(pageID) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'x-access-token': this.auth.token
      })
    };
    const data = {
      pageID: pageID
    }
    return this.http.post<any>(this.apiURL + 'pages/deletePage', data, httpOptions);
  }

}
