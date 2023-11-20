import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchUsersDialog } from './search-users.dialog';

describe('SearchUsersComponent', () => {
  let component: SearchUsersDialog;
  let fixture: ComponentFixture<SearchUsersDialog>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchUsersDialog]
    });
    fixture = TestBed.createComponent(SearchUsersDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
