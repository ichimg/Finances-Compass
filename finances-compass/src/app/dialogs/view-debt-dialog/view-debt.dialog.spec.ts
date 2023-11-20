import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDebtDialog } from './view-debt.dialog';

describe('ViewDebtDialog', () => {
  let component: ViewDebtDialog;
  let fixture: ComponentFixture<ViewDebtDialog>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewDebtDialog]
    });
    fixture = TestBed.createComponent(ViewDebtDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
