import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  loggedIn = false;
  user: any;
  token;
  navlist = [];

  private apiURL = 'http://localhost:3000/login';

  constructor(
    private http: HttpClient,
    private router: Router) { }

  authenticateUser(user: any): Observable<any> {
    return this.http.post<any>(this.apiURL, user);
  }

logout() {
  this.loggedIn = false;
  this.user = {};
  this.token = '';
  this.navlist = [];
  this.router.navigate(['/login']);
}

}

