import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DoctorService } from './doctor.service';
import { AuthService } from '../auth.service';
import { currentEnvironment } from '../../environment.prod';
import { Doctor } from '../../models/doctor.model';
import { HttpErrorResponse } from '@angular/common/http';

describe('DoctorService', () => {
  let service: DoctorService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  const apiURL = currentEnvironment.apiUrl + '/api/v1/Doctors';

  beforeEach(() => {
    // Creăm un spy pentru AuthService
    const spy = jasmine.createSpyObj('AuthService', ['getAuthHeaders']);
    // Simulăm un header de autorizare
    spy.getAuthHeaders.and.returnValue({ Authorization: 'Bearer token' });

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        DoctorService,
        { provide: AuthService, useValue: spy }
      ]
    });
    service = TestBed.inject(DoctorService);
    httpMock = TestBed.inject(HttpTestingController);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('#getDoctors', () => {
    
    it('ar trebui să gestioneze erorile din getDoctors', () => {
      const errorMessage = 'Error Code: 500\nMessage: Internal Server Error';
      service.getDoctors().subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe(errorMessage);
        }
      });

      const req = httpMock.expectOne(apiURL);
      expect(req.request.method).toBe('GET');
      req.flush('Internal Server Error', { status: 500, statusText: 'Server Error' });
    });
  });

  describe('#updateDoctor', () => {
   

    it('ar trebui să gestioneze erorile din updateDoctor', () => {
      const doctorId = '1';
      const updateData = { firstName: 'Updated', lastName: 'Doctor' };
      const errorMessage = 'Error Code: 404\nMessage: Not Found';
      
      service.updateDoctor(doctorId, updateData).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe(errorMessage);
        }
      });

      const req = httpMock.expectOne(`${apiURL}/${doctorId}`);
      expect(req.request.method).toBe('PUT');
      req.flush('Not Found', { status: 404, statusText: 'Not Found' });
    });
  });

  describe('#getDoctorById', () => {
    

    it('ar trebui să gestioneze erorile din getDoctorById', () => {
      const doctorId = '1';
      const errorMessage = 'Error Code: 400\nMessage: Bad Request';
      service.getDoctorById(doctorId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe(errorMessage);
        }
      });

      const req = httpMock.expectOne(`${apiURL}/${doctorId}`);
      expect(req.request.method).toBe('GET');
      req.flush('Bad Request', { status: 400, statusText: 'Bad Request' });
    });
  });
});
