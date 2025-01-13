import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AppointmentListComponent } from './appointment-list.component';
import { AppointmentService, UpdateStatusPayload } from '../../../services/appointment/appointment.service';
import { AuthService } from '../../../services/auth.service';
import { DoctorService } from '../../../services/doctor/doctor.service';
import { PatientService } from '../../../services/patient/patient.service';
import { MedicalHistoryService } from '../../../services/medical-history/medical-history.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { AppointmentStatus } from '../../../AppointmentStatus';
import { UserRole } from '../../../UserRole';

// Modele simplificate (după cum apar în componentă)
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

// Mocks pentru serviciile folosite

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

class MockAuthService {
  getUserRole() {
    return UserRole.Patient; // implicit pentru teste, se poate schimba
  }
  getUserId() {
    return 'testUserId';
  }
}

class MockDoctorService {
  getDoctorById(id: string) {
    return of({ userId: id, firstName: 'Doc', lastName: 'Test' });
  }
}

class MockPatientService {
  getPatientById(id: string) {
    return of({ userId: id, firstName: 'Pat', lastName: 'Test' });
  }
}

class MockMedicalHistoryService {
  submitMedicalHistory(formData: FormData) {
    return of({ success: true });
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

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
  let medicalHistoryService: MedicalHistoryService;
  let router: Router;
  let snackBar: MatSnackBar;
  let fb: FormBuilder;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AppointmentListComponent, // componenta este standalone
        ReactiveFormsModule,
        FormsModule,
        NoopAnimationsModule
      ],
      providers: [
        FormBuilder,
        { provide: AppointmentService, useClass: MockAppointmentService },
        { provide: AuthService, useClass: MockAuthService },
        { provide: DoctorService, useClass: MockDoctorService },
        { provide: PatientService, useClass: MockPatientService },
        { provide: MedicalHistoryService, useClass: MockMedicalHistoryService },
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
    medicalHistoryService = TestBed.inject(MedicalHistoryService);
    router = TestBed.inject(Router);
    snackBar = TestBed.inject(MatSnackBar);
    fb = TestBed.inject(FormBuilder);
    fixture.detectChanges();
  });

  // Testul de creare și inițializare
  it('ar trebui să creeze componenta și să seteze userRole în ngOnInit', () => {
    expect(component).toBeTruthy();
    expect(component.userRole).toBe(authService.getUserRole());
  });

  // Teste pentru fetchAppointments
  describe('fetchAppointments', () => {
    it('ar trebui să preia programările pentru un pacient și să populeze numele doctorilor', fakeAsync(() => {
      // În cazul unui pacient, getUserId este "testUserId"
      const getAppointmentsSpy = spyOn(appointmentService, 'getAppointmentsByPatientId').and.callThrough();
      const getDoctorSpy = spyOn(doctorService, 'getDoctorById').and.callThrough();
      const getPatientSpy = spyOn(patientService, 'getPatientById').and.callThrough();

      component.fetchAppointments();
      tick();

      expect(getAppointmentsSpy).toHaveBeenCalledWith('testUserId');
      // Pentru cele două programări, se extrag două id-uri de doctor, deci getDoctorById ar trebui să fie apelat de 2 ori.
      expect(getDoctorSpy).toHaveBeenCalledTimes(2);
      // Pentru patient, deși codul face map pe toți, din moment ce toate programările au același patientId,
      // se așteaptă un singur apel în funcție de implementare. (Ajustăm testul conform codului nostru.)
      expect(getPatientSpy).toHaveBeenCalledTimes(1);
      expect(component.appointments.length).toBe(2);
      component.appointments.forEach(appt => {
        expect(appt.doctorName).toBe('Doc Test');
      });
    }));

    it('ar trebui să preia programările pentru un doctor și să populeze numele pacienților', fakeAsync(() => {
      // Setăm rolul ca Doctor și userId corespunzător
      spyOn(authService, 'getUserRole').and.returnValue(UserRole.Doctor);
      spyOn(authService, 'getUserId').and.returnValue('doctor123');
      // Reinițializăm componenta
      component.ngOnInit();
      tick();

      const getAppointmentsSpy = spyOn(appointmentService, 'getAppointmentsByDoctorId').and.callThrough();
      const getDoctorSpy = spyOn(doctorService, 'getDoctorById').and.callThrough();
      const getPatientSpy = spyOn(patientService, 'getPatientById').and.callThrough();

      component.fetchAppointments();
      tick();

      expect(getAppointmentsSpy).toHaveBeenCalledWith('doctor123');
      // Pentru o singură programare, așteptăm un apel pentru fiecare serviciu
      expect(getPatientSpy).toHaveBeenCalledTimes(1);
      expect(getDoctorSpy).toHaveBeenCalledTimes(1);
      expect(component.appointments.length).toBe(1);
      component.appointments.forEach(appt => {
        expect(appt.patientName).toBe('Pat Test');
      });
    }));


  });

  // Teste pentru metodele utilitare
  describe('getStatusText și getStatusClass', () => {
    it('getStatusText ar trebui să returneze "Scheduled" pentru AppointmentStatus.Scheduled', () => {
      expect(component.getStatusText(AppointmentStatus.Scheduled)).toBe('Scheduled');
    });
    it('getStatusText ar trebui să returneze "Completed" pentru AppointmentStatus.Completed', () => {
      expect(component.getStatusText(AppointmentStatus.Completed)).toBe('Completed');
    });
    it('getStatusText ar trebui să returneze "Canceled" pentru AppointmentStatus.Canceled', () => {
      expect(component.getStatusText(AppointmentStatus.Canceled)).toBe('Canceled');
    });
    it('getStatusClass ar trebui să returneze clasa corespunzătoare status-ului', () => {
      expect(component.getStatusClass(AppointmentStatus.Scheduled)).toBe('status-scheduled');
      expect(component.getStatusClass(AppointmentStatus.Completed)).toBe('status-completed');
      expect(component.getStatusClass(AppointmentStatus.Canceled)).toBe('status-canceled');
      expect(component.getStatusClass(undefined as unknown as AppointmentStatus)).toBe('');
    });
  });




  // Test pentru onFileSelected
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

  });

  // Test pentru markAsCanceled și backToDashboard
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
