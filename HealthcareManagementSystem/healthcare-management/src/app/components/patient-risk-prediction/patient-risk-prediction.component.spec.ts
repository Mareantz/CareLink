import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientRiskPredictionComponent } from './patient-risk-prediction.component';

describe('PatientRiskPredictionComponent', () => {
  let component: PatientRiskPredictionComponent;
  let fixture: ComponentFixture<PatientRiskPredictionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientRiskPredictionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PatientRiskPredictionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
