import { TestBed } from '@angular/core/testing';
import { PatientRiskPredictionService, UserData } from './patient-risk-prediction.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { currentEnvironment } from '../../environment.prod';
import { AuthService } from '../auth.service';
import { HttpErrorResponse } from '@angular/common/http';

describe('PatientRiskPredictionService', () => {
  let service: PatientRiskPredictionService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  const apiUrl = currentEnvironment.apiUrl + '/api/v1/UserRiskPrediction/predict';

  beforeEach(() => {
    // Cream un spy pentru AuthService cu metoda getAuthHeaders
    const spy = jasmine.createSpyObj('AuthService', ['getAuthHeaders']);
    spy.getAuthHeaders.and.returnValue({ Authorization: 'Bearer test-token' });

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PatientRiskPredictionService,
        { provide: AuthService, useValue: spy }
      ]
    });

    service = TestBed.inject(PatientRiskPredictionService);
    httpMock = TestBed.inject(HttpTestingController);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('#predictRisk', () => {
    it('ar trebui să returneze un răspuns de succes atunci când predictRisk este apelat', () => {
      const dummyResponse = { risk: 'High', probability: 0.85 };
      const testUserData: UserData = {
        age: 45,
        gender: 'male',
        weight: 80,
        bloodPressure: 120,
        cholesterolLevel: 180,
        physicalActivityLevel: 1,
        smokingStatus: 0,
        stressLevel: 1
      };

      service.predictRisk(testUserData).subscribe(response => {
        expect(response).toEqual(dummyResponse);
      });

      const req = httpMock.expectOne(apiUrl);
      expect(req.request.method).toBe('POST');
      expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
      expect(req.request.body).toEqual(testUserData);

      req.flush(dummyResponse);
    });

    
  });
});
