import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';
import { FormBuilder } from '@angular/forms';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {

  user;
  userForm;

  private apiURL = 'http://localhost:3000/users/';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder) {
      this.userForm = this.formBuilder.group({
        firstname: '',
        lastname: '',
        teacher: false,
        administrator: false
      });
      this.user = this.userForm.value;
    }

  ngOnInit() {
    if (!this.authService.loggedIn) {
      return this.router.navigate(['/login']);
    }
    if (!this.authService.user.administrator) {
      return this.router.navigate(['/dashboard']);
    }
    let user;
    this.route.params.subscribe(params => {
      user = params['username'];
    });
    //Fetches the user from the backend 
    this.dbService.getUser(user)
    //Saves the user to the user parameter of the dashboard component
    .subscribe(response => {
      this.user = response[0];
      this.userForm.setValue({
        firstname: this.user.firstname,
        lastname: this.user.lastname,
        teacher: this.user.teacher,
        administrator: this.user.administrator
      });
    });
  }

  onSubmit(data) {
    data.username = this.user.username;
    this.dbService.updateUser(data).subscribe((result) => {
      if (result.error) {
        return alert('Käyttäjän tallennus epäonnistui odottamattoman virheen vuoksi. Yritä hetken päästä uudelleen.');
      }
      alert('Käyttäjän tiedot päivitettiin onnistuneesti!');
      this.router.navigate(['/users']);
    });
  }


}
