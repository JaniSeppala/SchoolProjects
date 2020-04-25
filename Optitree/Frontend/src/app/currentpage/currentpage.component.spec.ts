import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentpageComponent } from './currentpage.component';

describe('CurrentpageComponent', () => {
  let component: CurrentpageComponent;
  let fixture: ComponentFixture<CurrentpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentpageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
