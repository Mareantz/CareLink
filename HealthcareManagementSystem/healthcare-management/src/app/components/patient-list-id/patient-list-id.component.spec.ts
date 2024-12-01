import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientListIdComponent } from './patient-list-id.component';

describe('PatientListIdComponent', () => {
  let component: PatientListIdComponent;
  let fixture: ComponentFixture<PatientListIdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientListIdComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PatientListIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
