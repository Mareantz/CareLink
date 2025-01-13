import { TestBed } from '@angular/core/testing';
import { PatientService } from './patient.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { currentEnvironment } from '../../environment.prod';
import { AuthService } from '../auth.service';
import { Patient } from '../../models/patient.model';
import { HttpErrorResponse } from '@angular/common/http';

describe('PatientService', () => {
  let service: PatientService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  const apiURL = currentEnvironment.apiUrl + '/api/v1/Patients';

  beforeEach(() => {
    const spy = jasmine.createSpyObj('AuthService', ['getAuthHeaders']);
    // Simulăm header-ele de autorizare
    spy.getAuthHeaders.and.returnValue({ Authorization: 'Bearer token' });

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PatientService,
        { provide: AuthService, useValue: spy }
      ]
    });
    service = TestBed.inject(PatientService);
    httpMock = TestBed.inject(HttpTestingController);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('#getPatients', () => {
    

    it('ar trebui să gestioneze eroarea din getPatients', () => {
      service.getPatients().subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toContain('Error Code: 500');
          expect(error.message).toContain('Internal Server Error');
        }
      });

      const req = httpMock.expectOne(apiURL);
      expect(req.request.method).toBe('GET');
      req.flush('Internal Server Error', { status: 500, statusText: 'Internal Server Error' });
    });
  });

  

  describe('#updatePatient', () => {
    

    it('ar trebui să gestioneze eroarea din updatePatient', () => {
      const patientId = '1';
      const updateData = { firstName: 'Updated', lastName: 'Patient' };

      service.updatePatient(patientId, updateData).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toContain('Error Code: 404');
          expect(error.message).toContain('Not Found');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/${patientId}`);
      expect(req.request.method).toBe('PUT');
      req.flush('Not Found', { status: 404, statusText: 'Not Found' });
    });
  });

  describe('#getPatientById', () => {
   

    it('ar trebui să gestioneze eroarea din getPatientById', () => {
      const patientId = '1';

      service.getPatientById(patientId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toContain('Error Code: 400');
          expect(error.message).toContain('Bad Request');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/${patientId}`);
      expect(req.request.method).toBe('GET');
      req.flush('Bad Request', { status: 400, statusText: 'Bad Request' });
    });
  });

  describe('#deletePatientById', () => {
    it('ar trebui să șteargă pacientul cu un anumit ID', () => {
      const patientId = '1';
      service.deletePatientById(patientId).subscribe(response => {
        expect(response).toEqual({ success: true });
      });

      const req = httpMock.expectOne(`${apiURL}/${patientId}`);
      expect(req.request.method).toBe('DELETE');
      expect(req.request.headers.get('Authorization')).toBe('Bearer token');
      req.flush({ success: true });
    });

    it('ar trebui să gestioneze eroarea din deletePatientById', () => {
      const patientId = '1';

      service.deletePatientById(patientId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toContain('Error Code: 500');
          expect(error.message).toContain('Internal Server Error');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/${patientId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush('Internal Server Error', { status: 500, statusText: 'Internal Server Error' });
    });
  });

  describe('#getFilteredPatients', () => {
   

    it('ar trebui să gestioneze eroarea din getFilteredPatients', () => {
      const page = 1;
      const pageSize = 10;
      service.getFilteredPatients(page, pageSize).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toContain('Error Code: 500');
          expect(error.message).toContain('Internal Server Error');
        }
      });

      const req = httpMock.expectOne(request => request.url === `${apiURL}/filtered`);
      expect(req.request.method).toBe('GET');
      req.flush('Internal Server Error', { status: 500, statusText: 'Internal Server Error' });
    });
  });
});
