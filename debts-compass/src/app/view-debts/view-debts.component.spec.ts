import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDebtsComponent } from './view-debts.component';

describe('ViewDebtsComponent', () => {
  let component: ViewDebtsComponent;
  let fixture: ComponentFixture<ViewDebtsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewDebtsComponent]
    });
    fixture = TestBed.createComponent(ViewDebtsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
