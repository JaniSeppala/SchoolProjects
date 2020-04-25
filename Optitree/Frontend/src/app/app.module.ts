import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MyhomeComponent } from './myhome/myhome.component';
import { WorkspaceComponent } from './workspace/workspace.component';
import { CurrentpageComponent } from './currentpage/currentpage.component';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';
import { FooterComponent } from './footer/footer.component';
import { UsersComponent } from './users/users.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { AddUserComponent } from './add-user/add-user.component';
import { AddWorkspaceComponent } from './add-workspace/add-workspace.component';
import { WorkspacesComponent } from './workspaces/workspaces.component';
import { WorkspaceUsersComponent } from './workspace-users/workspace-users.component';
import { WorkspaceTeachersComponent } from './workspace-teachers/workspace-teachers.component';
import { WorkspacePagesComponent } from './workspace-pages/workspace-pages.component';
import { AddPageComponent } from './add-page/add-page.component';
import { EditPageComponent } from './edit-page/edit-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    MyhomeComponent,
    WorkspaceComponent,
    CurrentpageComponent,
    HeaderComponent,
    LoginComponent,
    FooterComponent,
    UsersComponent,
    EditUserComponent,
    AddUserComponent,
    AddWorkspaceComponent,
    WorkspacesComponent,
    WorkspaceUsersComponent,
    WorkspaceTeachersComponent,
    WorkspacePagesComponent,
    AddPageComponent,
    EditPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
