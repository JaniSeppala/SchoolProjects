import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkspaceTeachersComponent } from './workspace-teachers.component';

describe('WorkspaceTeachersComponent', () => {
  let component: WorkspaceTeachersComponent;
  let fixture: ComponentFixture<WorkspaceTeachersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkspaceTeachersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkspaceTeachersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
