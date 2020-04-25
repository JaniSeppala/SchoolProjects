import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { DatabaseService } from '../database.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  user;
  userForm;
  unavailable;

  constructor(
    private router: Router,
    private dbService: DatabaseService,
    private formBuilder: FormBuilder) {
      this.userForm = this.formBuilder.group({
        username: '',
        pswd: '',
        firstname: '',
        lastname: '',
        teacher: false,
        administrator: false
      });
      this.user = this.userForm.value;
      this.unavailable = false;
    }

  ngOnInit() {

  }

  checkUsernameAvailability(username) {
    if (!username) {
      return
    }
    this.dbService.getUser(username).subscribe((result)=>{
      if (result.length > 0) {
        this.unavailable = true;
      } else {
        this.unavailable = false;
      }
    });
  }

  onSubmit(data) {
    this.dbService.addUser(data).subscribe((result)=>{
      if (result.error) {
        return alert('Odottamaton virhe lisätessä käyttäjää. Kokeile hetken päästä uudelleen.');
      }
      alert('Käyttäjä lisättiin onnistuneesti!');
      this.router.navigate(['/users']);
    });
  }

}
