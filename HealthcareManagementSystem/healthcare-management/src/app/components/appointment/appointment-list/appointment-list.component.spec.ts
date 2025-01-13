import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AppointmentListComponent } from './appointment-list.component';
import { AppointmentService, UpdateStatusPayload } from '../../../services/appointment/appointment.service';
import { AuthService } from '../../../services/auth.service';
import { DoctorService } from '../../../services/doctor/doctor.service';
import { PatientService } from '../../../services/patient/patient.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppointmentStatus } from '../../../AppointmentStatus';
import { UserRole } from '../../../UserRole';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Definirea modelelor (simplificate)
interface Appointment {
  id: string;
  appointmentDate: string;
  doctorId: string;
  patientId: string;
  status: AppointmentStatus;
  doctorName?: string;
  patientName?: string;
}

interface MedicalHistory {
  appointmentId: string;
  diagnosis: string;
  medication: string;
  notes: string;
}

// MOCK pentru AppointmentService
class MockAppointmentService {
  getAppointmentsByPatientId(patientId: string) {
    const appointments: Appointment[] = [
      { id: 'appt1', appointmentDate: new Date('2025-03-15T09:00:00').toISOString(), doctorId: 'doc1', patientId: patientId, status: AppointmentStatus.Scheduled },
      { id: 'appt2', appointmentDate: new Date('2025-03-16T10:00:00').toISOString(), doctorId: 'doc2', patientId: patientId, status: AppointmentStatus.Scheduled }
    ];
    return of(appointments);
  }
  getAppointmentsByDoctorId(doctorId: string) {
    const appointments: Appointment[] = [
      { id: 'appt3', appointmentDate: new Date('2025-03-15T11:00:00').toISOString(), doctorId: doctorId, patientId: 'pat1', status: AppointmentStatus.Scheduled }
    ];
    return of(appointments);
  }
  updateAppointmentStatus(payload: UpdateStatusPayload) {
    return of({ success: true });
  }
  submitMedicalHistory(formData: FormData) {
    return of({ success: true });
  }
}

// MOCK pentru AuthService
class MockAuthService {
  getUserRole() {
    return UserRole.Patient; // Va putea fi modificat pentru teste
  }
  getUserId() {
    return 'testUserId';
  }
}

// MOCK pentru DoctorService
class MockDoctorService {
  getDoctorById(id: string) {
    return of({ userId: id, firstName: 'Doc', lastName: 'Test' });
  }
}

// MOCK pentru PatientService
class MockPatientService {
  getPatientById(id: string) {
    return of({ userId: id, firstName: 'Pat', lastName: 'Test' });
  }
}

// MOCK pentru Router
class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

// MOCK pentru MatSnackBar
class MockMatSnackBar {
  open(message: string, action: string, config: any) {}
}

describe('AppointmentListComponent', () => {
  let component: AppointmentListComponent;
  let fixture: ComponentFixture<AppointmentListComponent>;
  let appointmentService: AppointmentService;
  let authService: AuthService;
  let doctorService: DoctorService;
  let patientService: PatientService;
  let router: Router;
  let snackBar: MatSnackBar;
  let fb: FormBuilder;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AppointmentListComponent,
        ReactiveFormsModule,
        FormsModule,
        NoopAnimationsModule // Pentru a evita erorile legate de animații
      ],
      providers: [
        FormBuilder,
        { provide: AppointmentService, useClass: MockAppointmentService },
        { provide: AuthService, useClass: MockAuthService },
        { provide: DoctorService, useClass: MockDoctorService },
        { provide: PatientService, useClass: MockPatientService },
        { provide: Router, useClass: MockRouter },
        { provide: MatSnackBar, useClass: MockMatSnackBar }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointmentListComponent);
    component = fixture.componentInstance;
    appointmentService = TestBed.inject(AppointmentService);
    authService = TestBed.inject(AuthService);
    doctorService = TestBed.inject(DoctorService);
    patientService = TestBed.inject(PatientService);
    router = TestBed.inject(Router);
    snackBar = TestBed.inject(MatSnackBar);
    fb = TestBed.inject(FormBuilder);
    fixture.detectChanges();
  });

  it('ar trebui să creeze componenta și să seteze rolul utilizatorului în ngOnInit', () => {
    expect(component).toBeTruthy();
    expect(component.userRole).toBe(UserRole.Patient);
  });

  describe('fetchAppointments', () => {
    it('ar trebui să preia programările pentru un pacient și să populeze numele doctorilor', fakeAsync(() => {
      const getAppointmentsSpy = spyOn(appointmentService, 'getAppointmentsByPatientId').and.callThrough();
      const getDoctorSpy = spyOn(doctorService, 'getDoctorById').and.callThrough();
      const getPatientSpy = spyOn(patientService, 'getPatientById').and.callThrough();

      component.fetchAppointments();
      tick();

      expect(getAppointmentsSpy).toHaveBeenCalledWith('testUserId');
      // Pentru două programări, se apelează getDoctorById de 2 ori (pentru doc1 și doc2)
      expect(getDoctorSpy).toHaveBeenCalledTimes(2);
      // Se apelează și patientService – chiar dacă pentru rolul Patient nu se folosește patientMap,
      // codul execută apelul pentru fiabilitate
      expect(getPatientSpy).toHaveBeenCalledTimes(2);
      expect(component.appointments.length).toBe(2);
      component.appointments.forEach(appt => {
        expect(appt.doctorName).toBe('Doc Test');
      });
    }));

    it('ar trebui să preia programările pentru un doctor și să populeze numele pacienților', fakeAsync(() => {
      spyOn(authService, 'getUserRole').and.returnValue(UserRole.Doctor);
      spyOn(authService, 'getUserId').and.returnValue('doctor123');

      // Reinițializăm componenta pentru rol Doctor
      component.ngOnInit();
      tick();

      const getAppointmentsSpy = spyOn(appointmentService, 'getAppointmentsByDoctorId').and.callThrough();
      const getDoctorSpy = spyOn(doctorService, 'getDoctorById').and.callThrough();
      const getPatientSpy = spyOn(patientService, 'getPatientById').and.callThrough();

      component.fetchAppointments();
      tick();

      expect(getAppointmentsSpy).toHaveBeenCalledWith('doctor123');
      // În acest caz, pentru o programare unică, se apelează getPatientById de 1 dată,
      // dar se apelează și getDoctorById de 1 dată (deoarece codul nu diferențiază complet)
      expect(getPatientSpy).toHaveBeenCalledTimes(1);
      expect(getDoctorSpy).toHaveBeenCalledTimes(1);
      expect(component.appointments.length).toBe(1);
      component.appointments.forEach(appt => {
        expect(appt.patientName).toBe('Pat Test');
      });
    }));

    it('ar trebui să trateze lipsa patientId sau doctorId și să nu aibă fetchObservable', () => {
      // Simulăm situația în care getUserId() returnează null
      spyOn(authService, 'getUserId').and.returnValue(null);
      spyOn(console, 'error');
      // Apelăm fetchAppointments
      component.fetchAppointments();
      // Deoarece getUserId() este null, nu se va apela niciun serviciu pentru fetch
      expect(console.error).toHaveBeenCalled();
      // Lista de programări rămâne goală
      expect(component.appointments.length).toBe(0);
    });
  });

  describe('getStatusText și getStatusClass', () => {
    it('getStatusText ar trebui să returneze "Canceled" pentru status Canceled', () => {
      expect(component.getStatusText(AppointmentStatus.Canceled)).toBe('Canceled');
    });
    it('getStatusText ar trebui să returneze "Completed" pentru status Completed', () => {
      expect(component.getStatusText(AppointmentStatus.Completed)).toBe('Completed');
    });
    it('getStatusText ar trebui să returneze "Scheduled" pentru status Scheduled', () => {
      expect(component.getStatusText(AppointmentStatus.Scheduled)).toBe('Scheduled');
    });
    it('getStatusClass ar trebui să returneze clasa corespunzătoare status-ului', () => {
      expect(component.getStatusClass(AppointmentStatus.Scheduled)).toBe('status-scheduled');
      expect(component.getStatusClass(AppointmentStatus.Completed)).toBe('status-completed');
      expect(component.getStatusClass(AppointmentStatus.Canceled)).toBe('status-canceled');
      expect(component.getStatusClass(undefined as unknown as AppointmentStatus)).toBe('');
    });
  });

  describe('cancelAppointment', () => {
    it('ar trebui să anuleze o programare și să refacă fetchAppointments după succes', fakeAsync(() => {
      const updateStatusSpy = spyOn(appointmentService, 'updateAppointmentStatus').and.callThrough();
      const snackSpy = spyOn(snackBar, 'open');
      const fetchSpy = spyOn(component, 'fetchAppointments').and.callThrough();

      component.cancelAppointment('appt1');
      tick();

      const expectedPayload: UpdateStatusPayload = { appointmentId: 'appt1', newStatus: 2 };
      expect(updateStatusSpy).toHaveBeenCalledWith(expectedPayload);
      expect(snackSpy).toHaveBeenCalledWith('Appointment canceled successfully.', 'Close', { duration: 3000 });
      expect(fetchSpy).toHaveBeenCalled();
    }));

    it('ar trebui să trateze eroarea la anularea unei programări', fakeAsync(() => {
      spyOn(appointmentService, 'updateAppointmentStatus').and.returnValue(throwError(() => new Error('Cancel error')));
      const snackSpy = spyOn(snackBar, 'open');
      spyOn(console, 'error');

      component.cancelAppointment('appt1');
      tick();

      expect(console.error).toHaveBeenCalledWith('Error canceling appointment:', jasmine.any(Error));
      expect(snackSpy).toHaveBeenCalledWith('Failed to cancel appointment.', 'Close', { duration: 3000 });
    }));
  });



  describe('onFileSelected', () => {
    it('ar trebui să adauge fișierele selectate până la limita de 5', () => {
      const snackSpy = spyOn(snackBar, 'open');
      const file1 = new File(['content'], 'file1.txt', { type: 'text/plain' });
      const file2 = new File(['content'], 'file2.txt', { type: 'text/plain' });
      const file3 = new File(['content'], 'file3.txt', { type: 'text/plain' });
      const event = { target: { files: [file1, file2, file3] } } as unknown as Event;
      component.selectedFiles = [];
      component.onFileSelected(event);
      expect(component.selectedFiles.length).toBe(3);
      expect(snackSpy).not.toHaveBeenCalled();
    });

    it('ar trebui să afișeze un snackBar dacă se depășește limita de 5 fișiere', () => {
      const snackSpy = spyOn(snackBar, 'open');
      // Simulăm existența a 4 fișiere selectate deja
      const fileExisting = new File(['existing'], 'existing.txt', { type: 'text/plain' });
      component.selectedFiles = [fileExisting, fileExisting, fileExisting, fileExisting];
      // Evenimentul aduce încă 2 fișiere (total 6 > 5)
      const fileNew1 = new File(['new'], 'new1.txt', { type: 'text/plain' });
      const fileNew2 = new File(['new'], 'new2.txt', { type: 'text/plain' });
      const event = { target: { files: [fileNew1, fileNew2] } } as unknown as Event;
      component.onFileSelected(event);
      expect(snackSpy).toHaveBeenCalledWith('You can only upload a maximum of 5 files.', 'Close', { duration: 3000 });
      expect(component.selectedFiles.length).toBe(4);
    });
  });

  describe('markAsCanceled și backToDashboard', () => {
    it('markAsCanceled ar trebui să apeleze cancelAppointment cu ID-ul specificat', () => {
      spyOn(component, 'cancelAppointment');
      component.markAsCanceled('appt1');
      expect(component.cancelAppointment).toHaveBeenCalledWith('appt1');
    });
    it('backToDashboard ar trebui să navigheze la ruta /dashboard', () => {
      const routerSpy = spyOn(router, 'navigate').and.callThrough();
      component.backToDashboard();
      expect(routerSpy).toHaveBeenCalledWith(['/dashboard']);
    });
  });
});
