import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  constructor(
    private router: Router,
    private authService: AuthenticationService) { }

  ngOnInit() {
  }

  toggleVisibility(workspace) {
    workspace.isOpen = !workspace.isOpen;
  }

}
