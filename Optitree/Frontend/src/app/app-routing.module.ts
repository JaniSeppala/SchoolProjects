import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CurrentpageComponent } from './currentpage/currentpage.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { UsersComponent } from './users/users.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { AddUserComponent } from './add-user/add-user.component';
import { AddWorkspaceComponent } from './add-workspace/add-workspace.component';
import { WorkspacesComponent } from './workspaces/workspaces.component';
import { WorkspaceComponent } from './workspace/workspace.component';
import { WorkspaceUsersComponent } from './workspace-users/workspace-users.component';
import { WorkspaceTeachersComponent } from './workspace-teachers/workspace-teachers.component';
import { WorkspacePagesComponent } from './workspace-pages/workspace-pages.component';
import { AddPageComponent } from './add-page/add-page.component';
import { EditPageComponent } from './edit-page/edit-page.component';

const routes: Routes = [
  { path:'', component: LoginComponent },
  { path:'workspaces/:workspacename/:pageID', component: CurrentpageComponent },
  { path:'dashboard', component: DashboardComponent },
  { path:'login', component:LoginComponent },
  { path:'users', component:UsersComponent },
  { path:'edituser/:username', component: EditUserComponent },
  { path:'adduser', component: AddUserComponent },
  { path:'workspaces', component: WorkspacesComponent },
  { path:'addworkspace',component: AddWorkspaceComponent },
  { path:'manageusers/:workspacename', component: WorkspaceUsersComponent },
  { path:'manageteachers/:workspacename', component: WorkspaceTeachersComponent },
  { path:'managepages/:workspacename', component: WorkspacePagesComponent },
  { path:'addpage/:workspacename', component: AddPageComponent },
  { path:'editpage/:workspacename/:pageID', component: EditPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
