import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkspaceUsersComponent } from './workspace-users.component';

describe('WorkspaceUsersComponent', () => {
  let component: WorkspaceUsersComponent;
  let fixture: ComponentFixture<WorkspaceUsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkspaceUsersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkspaceUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
