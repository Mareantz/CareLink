import { TestBed } from '@angular/core/testing';

import { PatientRiskPredictionService } from './patient-risk-prediction.service';

describe('PatientRiskPredictionService', () => {
  let service: PatientRiskPredictionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PatientRiskPredictionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
