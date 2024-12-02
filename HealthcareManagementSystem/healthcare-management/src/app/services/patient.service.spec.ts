import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PatientService } from './patient.service';
import { Patient } from '../models/patient.model';

describe('PatientService', () => {
  let service: PatientService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PatientService]
    });
    service = TestBed.inject(PatientService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch patients', () => {
    const mockPatients: Patient[] = [
      { id: '1', firstName: 'John', lastName: 'Doe', dateOfBirth: new Date(), gender: 'Male', address: '123 Street' }
    ];

    service.getPatients().subscribe(patients => {
      expect(patients.length).toBe(1);
      expect(patients).toEqual(mockPatients);
    });

    const req = httpMock.expectOne(service['apiURL']);
    expect(req.request.method).toBe('GET');
    req.flush(mockPatients);
  });

  it('should create a patient', () => {
    const newPatient: Patient = { id: '2', firstName: 'Jane', lastName: 'Doe', dateOfBirth: new Date(), gender: 'Female', address: '456 Avenue' };

    service.createPatient(newPatient).subscribe(response => {
      expect(response).toEqual(newPatient);
    });

    const req = httpMock.expectOne(service['apiURL']);
    expect(req.request.method).toBe('POST');
    req.flush(newPatient);
  });

  it('should update a patient', () => {
    const updatedPatient: Patient = { id: '1', firstName: 'John', lastName: 'Doe', dateOfBirth: new Date(), gender: 'Male', address: '123 Street' };

    service.updatePatient('1', updatedPatient).subscribe(response => {
      expect(response).toEqual(updatedPatient);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/1`);
    expect(req.request.method).toBe('PUT');
    req.flush(updatedPatient);
  });

  it('should fetch patient by ID', () => {
    const mockPatient: Patient = { id: '1', firstName: 'John', lastName: 'Doe', dateOfBirth: new Date(), gender: 'Male', address: '123 Street' };

    service.getPatientById('1').subscribe(patient => {
      expect(patient).toEqual(mockPatient);
    });

    const req = httpMock.expectOne(`${service['apiURL']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPatient);
  });

});