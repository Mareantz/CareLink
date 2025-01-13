import { TestBed } from '@angular/core/testing';
import { MedicalHistoryService } from './medical-history.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { currentEnvironment } from '../../environment.prod';
import { AuthService } from '../auth.service';
import { MedicalHistory } from '../../models/medical-history.model';
import { HttpErrorResponse } from '@angular/common/http';

describe('MedicalHistoryService', () => {
  let service: MedicalHistoryService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  const apiURL = currentEnvironment.apiUrl + '/api/v1/MedicalHistory';

  beforeEach(() => {
    const spy = jasmine.createSpyObj('AuthService', ['getAuthHeaders']);
    // Simulăm header-ele de autorizare
    spy.getAuthHeaders.and.returnValue({ Authorization: 'Bearer token' });
    
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        MedicalHistoryService,
        { provide: AuthService, useValue: spy }
      ]
    });
    service = TestBed.inject(MedicalHistoryService);
    httpMock = TestBed.inject(HttpTestingController);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('#submitMedicalHistory', () => {
    it('ar trebui să posteze FormData și să returneze un răspuns de succes', () => {
      const dummyResponse = { success: true };
      // Creăm un FormData de test
      const formData = new FormData();
      formData.append('Diagnosis', 'Test Diagnosis');
      formData.append('Medication', 'Test Medication');
      formData.append('DateRecorded', '2021-10-10T00:00:00Z');
      formData.append('Notes', 'Test Notes');

      service.submitMedicalHistory(formData).subscribe(response => {
        expect(response).toEqual(dummyResponse);
      });

      const req = httpMock.expectOne(apiURL);
      expect(req.request.method).toBe('POST');
      expect(req.request.headers.get('Authorization')).toBe('Bearer token');
      req.flush(dummyResponse);
    });

    it('ar trebui să gestioneze eroarea la submitMedicalHistory', () => {
      const formData = new FormData();
      formData.append('Diagnosis', 'Test Diagnosis');

      service.submitMedicalHistory(formData).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to submit medical history.');
        }
      });

      const req = httpMock.expectOne(apiURL);
      expect(req.request.method).toBe('POST');
      req.flush('Error', { status: 500, statusText: 'Server Error' });
    });
  });

  describe('#getMedicalHistoryByPatientId', () => {
    
    it('ar trebui să gestioneze eroarea în getMedicalHistoryByPatientId', () => {
      const patientId = 'patient123';
      const url = `${apiURL}/patient/${patientId}`;

      service.getMedicalHistoryByPatientId(patientId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to fetch medical history.');
        }
      });

      const req = httpMock.expectOne(url);
      expect(req.request.method).toBe('GET');
      req.flush('Error fetching data', { status: 500, statusText: 'Server Error' });
    });
  });
});
