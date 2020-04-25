import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  users = [];

  constructor(
    private authService: AuthenticationService,
    private dbService: DatabaseService,
    private router: Router) { }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator) {
      return this.router.navigate(['/']);
    }
    this.dbService.getUsers().subscribe((results) => {
      this.users = results;
    });
  }



  onDelete(username) {
    if (username === this.authService.user.username) {
      return alert(`Et voi poistaa käyttäjää ${username}, koska olet kirjautunut sisään sillä käyttäjällä!`);
    }
    if (confirm(`Haluatko varmasti poistaa käyttäjän ${username}? Tätä toimintoa ei voi peruuttaa!`)) {
      this.dbService.deleteUser(username).subscribe((result)=>{
        if (result.error) {
          return alert('Käyttäjän poisto epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
        }
        alert(`Käyttäjän ${username} poisto onnistui!`);
        this.dbService.getUsers().subscribe((results) => {
          this.users = results;
        });
      });
    }
  }

}
