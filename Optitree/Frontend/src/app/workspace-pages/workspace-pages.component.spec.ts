import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkspacePagesComponent } from './workspace-pages.component';

describe('WorkspacePagesComponent', () => {
  let component: WorkspacePagesComponent;
  let fixture: ComponentFixture<WorkspacePagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkspacePagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkspacePagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
