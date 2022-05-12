import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { NotAllowedComponent } from './not-allowed.component';

describe('NotAllowedComponent', () => {
  let component: NotAllowedComponent;
  let fixture: ComponentFixture<NotAllowedComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ NotAllowedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotAllowedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
