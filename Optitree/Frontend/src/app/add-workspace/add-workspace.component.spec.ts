import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddWorkspaceComponent } from './add-workspace.component';

describe('AddWorkspaceComponent', () => {
  let component: AddWorkspaceComponent;
  let fixture: ComponentFixture<AddWorkspaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddWorkspaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddWorkspaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
