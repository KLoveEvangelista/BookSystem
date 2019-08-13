import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorUpdateFormComponent } from './author-update-form.component';

describe('AuthorUpdateFormComponent', () => {
  let component: AuthorUpdateFormComponent;
  let fixture: ComponentFixture<AuthorUpdateFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthorUpdateFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorUpdateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
