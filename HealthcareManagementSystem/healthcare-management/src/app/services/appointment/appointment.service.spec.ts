import { TestBed } from '@angular/core/testing';
import { AppointmentService, UpdateStatusPayload } from './appointment.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { currentEnvironment } from '../../environment.prod';
import { AuthService } from '../auth.service';
import { Appointment } from '../../models/appointment.model';
import { AppointmentPost } from '../../models/appointmentPost.model';
import { HttpErrorResponse } from '@angular/common/http';

describe('AppointmentService', () => {
  let service: AppointmentService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthService>;

  const apiURL = currentEnvironment.apiUrl + '/api/v1/Appointments';

  beforeEach(() => {
    const spy = jasmine.createSpyObj('AuthService', ['getAuthHeaders']);
    // Setați header-ele de autorizare simulate
    spy.getAuthHeaders.and.returnValue({ Authorization: 'Bearer test-token' });

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AppointmentService,
        { provide: AuthService, useValue: spy }
      ]
    });
    service = TestBed.inject(AppointmentService);
    httpMock = TestBed.inject(HttpTestingController);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('#getAppointmentsByDoctorAndDate', () => {
    it('ar trebui să returneze o listă de programări pentru un anumit doctor și dată', () => {
      const doctorId = 'doc123';
      const testDate = new Date('2025-03-15T09:00:00'); // pentru test
      // formatul generat de metoda formatDate: YYYY.MM.DD (ex: 2025.03.15)
      const formattedDate = '2025.03.15';
      const dummyResponse = { data: [
        { id: 'appt1', appointmentDate: '2025-03-15T09:00:00', doctorId: doctorId, patientId: 'pat1', status: 0 },
        { id: 'appt2', appointmentDate: '2025-03-15T10:00:00', doctorId: doctorId, patientId: 'pat2', status: 0 }
      ]};

      service.getAppointmentsByDoctorAndDate(doctorId, testDate).subscribe((appointments: Appointment[]) => {
        expect(appointments.length).toBe(2);
        expect(appointments[0].id).toBe('appt1');
      });

      const req = httpMock.expectOne(`${apiURL}/doctor/${doctorId}/date/${formattedDate}`);
      expect(req.request.method).toBe('GET');
      expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
      req.flush(dummyResponse);
    });

    it('ar trebui să gestioneze eroarea în getAppointmentsByDoctorAndDate', () => {
      const doctorId = 'doc123';
      const testDate = new Date('2025-03-15T09:00:00');
      const formattedDate = '2025.03.15';

      service.getAppointmentsByDoctorAndDate(doctorId, testDate).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to fetch appointments.');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/doctor/${doctorId}/date/${formattedDate}`);
      req.flush('Error fetching data', {
        status: 500,
        statusText: 'Internal Server Error'
      });
    });
  });

  describe('#getAppointmentsByPatientId', () => {
    it('ar trebui să returneze o listă de programări pentru un anumit pacient', () => {
      const patientId = 'pat123';
      const dummyResponse = { data: [
        { id: 'appt1', appointmentDate: '2025-03-15T11:00:00', doctorId: 'doc1', patientId: patientId, status: 0 }
      ]};

      service.getAppointmentsByPatientId(patientId).subscribe((appointments: Appointment[]) => {
        expect(appointments.length).toBe(1);
        expect(appointments[0].id).toBe('appt1');
      });

      const req = httpMock.expectOne(`${apiURL}/patient/${patientId}`);
      expect(req.request.method).toBe('GET');
      expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
      req.flush(dummyResponse);
    });

    it('ar trebui să gestioneze eroarea în getAppointmentsByPatientId', () => {
      const patientId = 'pat123';

      service.getAppointmentsByPatientId(patientId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to fetch appointments.');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/patient/${patientId}`);
      req.flush('Error', { status: 400, statusText: 'Bad Request' });
    });
  });

  describe('#getAppointmentsByDoctorId', () => {
    it('ar trebui să returneze o listă de programări pentru un anumit doctor', () => {
      const doctorId = 'doc123';
      const dummyResponse = { data: [
        { id: 'appt1', appointmentDate: '2025-03-15T12:00:00', doctorId: doctorId, patientId: 'pat1', status: 0 }
      ]};

      service.getAppointmentsByDoctorId(doctorId).subscribe((appointments: Appointment[]) => {
        expect(appointments.length).toBe(1);
        expect(appointments[0].id).toBe('appt1');
      });

      const req = httpMock.expectOne(`${apiURL}/doctor/${doctorId}`);
      expect(req.request.method).toBe('GET');
      expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
      req.flush(dummyResponse);
    });

    it('ar trebui să gestioneze eroarea în getAppointmentsByDoctorId', () => {
      const doctorId = 'doc123';

      service.getAppointmentsByDoctorId(doctorId).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to fetch appointments.');
        }
      });

      const req = httpMock.expectOne(`${apiURL}/doctor/${doctorId}`);
      req.flush('Error', { status: 404, statusText: 'Not Found' });
    });
  });

 

   

  describe('#updateAppointmentStatus', () => {
    it('ar trebui să actualizeze statusul unei programări', () => {
      const payload: UpdateStatusPayload = { appointmentId: 'appt1', newStatus: 2 };
      const url = `${apiURL}/update-status`;

      service.updateAppointmentStatus(payload).subscribe(response => {
        expect(response).toEqual({ success: true });
      });

      const req = httpMock.expectOne(url);
      expect(req.request.method).toBe('PUT');
      expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
      expect(req.request.body).toEqual(payload);
      req.flush({ success: true });
    });

    it('ar trebui să gestioneze eroarea în updateAppointmentStatus', () => {
      const payload: UpdateStatusPayload = { appointmentId: 'appt1', newStatus: 2 };
      const url = `${apiURL}/update-status`;

      service.updateAppointmentStatus(payload).subscribe({
        next: () => fail('Ar trebui să fi eșuat'),
        error: (error: Error) => {
          expect(error.message).toBe('Failed to update appointment status.');
        }
      });

      const req = httpMock.expectOne(url);
      req.flush('Update error', { status: 400, statusText: 'Bad Request' });
    });
  });
});
 