import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor/doctor.service';
import { PatientService } from '../../services/patient/patient.service';
import { MedicalHistoryService } from '../../services/medical-history/medical-history.service';
import { BreakpointObserver, BreakpointState, Breakpoints } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { UserRole } from '../../UserRole';

// Interfețe pentru utilizatori (pentru test, simplificate)
interface Patient {
  userId: string;
  firstName: string;
  lastName: string;
}
interface Doctor {
  userId: string;
  firstName: string;
  lastName: string;
}
interface MedicalHistory {
  appointmentId: string;
  diagnosis: string;
  medication: string;
  notes: string;
  dateRecorded: string;
}

// Mock-uri pentru serviciile injectate
class MockAuthService {
  private token: string | null = 'dummy-token';
  private role: UserRole = UserRole.None; // folosim enumul nou
  private userId: string = 'user123';

  getToken(): string | null {
    return this.token;
  }
  getUserRole(): UserRole {
    return this.role;
  }
  getUserId(): string | null {
    return this.userId;
  }
  logout(): void {
    this.token = null;
  }
  // Metode utilitare pentru teste
  setUserRole(role: UserRole) {
    this.role = role;
  }
  setToken(token: string | null) {
    this.token = token;
  }
}

class MockPatientService {
  getPatientById(userId: string) {
    const dummyPatient: Patient = { userId, firstName: 'Jane', lastName: 'Doe' };
    return of(dummyPatient);
  }
}

class MockDoctorService {
  getDoctorById(userId: string) {
    const dummyDoctor: Doctor = { userId, firstName: 'John', lastName: 'Smith' };
    return of(dummyDoctor);
  }
}

class MockMedicalHistoryService {
  getMedicalHistoryByPatientId(userId: string) {
    const histories: MedicalHistory[] = [
      { appointmentId: 'a1', diagnosis: 'D1', medication: 'M1', notes: 'N1', dateRecorded: '2021-10-12T00:00:00Z' },
      { appointmentId: 'a2', diagnosis: 'D2', medication: 'M2', notes: 'N2', dateRecorded: '2021-10-10T00:00:00Z' }
    ];
    return of(histories);
  }
}

class MockBreakpointObserver {
  private subject = new Subject<BreakpointState>();
  observe(queries: string[]) {
    return this.subject.asObservable();
  }
  emit(state: BreakpointState) {
    this.subject.next(state);
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let authService: MockAuthService;
  let patientService: MockPatientService;
  let doctorService: MockDoctorService;
  let medicalHistoryService: MockMedicalHistoryService;
  let breakpointObserver: MockBreakpointObserver;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardComponent, RouterTestingModule, NoopAnimationsModule],
      providers: [
        { provide: AuthService, useClass: MockAuthService },
        { provide: PatientService, useClass: MockPatientService },
        { provide: DoctorService, useClass: MockDoctorService },
        { provide: MedicalHistoryService, useClass: MockMedicalHistoryService },
        { provide: BreakpointObserver, useClass: MockBreakpointObserver },
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as unknown as MockAuthService;
    patientService = TestBed.inject(PatientService) as unknown as MockPatientService;
    doctorService = TestBed.inject(DoctorService) as unknown as MockDoctorService;
    medicalHistoryService = TestBed.inject(MedicalHistoryService) as unknown as MockMedicalHistoryService;
    breakpointObserver = TestBed.inject(BreakpointObserver) as unknown as MockBreakpointObserver;
    router = TestBed.inject(Router);
  });

  describe('ngOnInit', () => {
    it('ar trebui să redirecționeze către login dacă nu există token', () => {
      authService.setToken(null);
      const routerSpy = spyOn(router, 'navigate');
      component.ngOnInit();
      expect(routerSpy).toHaveBeenCalledWith(['/login']);
    });

    it('ar trebui să seteze navItems și displayName pentru un pacient', fakeAsync(() => {
      authService.setUserRole(UserRole.Patient);
      const patientSpy = spyOn(patientService, 'getPatientById').and.callThrough();
      component.ngOnInit();
      tick();
      // Pentru un pacient, navItems ar trebui să aibă 3 elemente
      expect(component.navItems.length).toBe(3);
      expect(component.navItems).toEqual([
        { label: 'Doctors', icon: 'local_hospital', route: '/doctors' },
        { label: 'Health Risk Calculator', icon: 'calculate', route: '/health-risk' },
        { label: 'My Appointments', icon: 'event', route: '/appointments' }
      ]);
      expect(patientSpy).toHaveBeenCalledWith('user123');
      expect(component.displayName).toBe('Jane Doe');
    }));

    it('ar trebui să seteze navItems și displayName pentru un doctor', fakeAsync(() => {
      authService.setUserRole(UserRole.Doctor);
      const doctorSpy = spyOn(doctorService, 'getDoctorById').and.callThrough();
      component.ngOnInit();
      tick();
      expect(component.navItems.length).toBe(3);
      expect(component.navItems).toEqual([
        { label: 'My Profile', icon: 'person', route: '/profile' },
        { label: 'My Patients', icon: 'group', route: '/patients' },
        { label: 'Appointments', icon: 'event', route: '/appointments' }
      ]);
      expect(doctorSpy).toHaveBeenCalledWith('user123');
      expect(component.displayName).toBe('John Smith');
    }));

    it('ar trebui să încarce istoricul medical pentru pacienți dacă rolul este patient', fakeAsync(() => {
      authService.setUserRole(UserRole.Patient);
      const historySpy = spyOn(medicalHistoryService, 'getMedicalHistoryByPatientId').and.callThrough();
      component.ngOnInit();
      tick();
      expect(historySpy).toHaveBeenCalledWith('user123');
      expect(component.medicalHistoryList.length).toBeGreaterThan(0);
      if (component.medicalHistoryList.length > 1) {
        const firstDate = new Date(component.medicalHistoryList[0].dateRecorded).getTime();
        const secondDate = new Date(component.medicalHistoryList[1].dateRecorded).getTime();
        expect(firstDate).toBeGreaterThan(secondDate);
      }
    }));
  });

  describe('userRoleString getter', () => {
    it('ar trebui să returneze "admin" pentru UserRole.Admin', () => {
      authService.setUserRole(UserRole.Admin);
      component.userRole = UserRole.Admin;
      expect(component.userRoleString).toBe('admin');
    });
    it('ar trebui să returneze "doctor" pentru UserRole.Doctor', () => {
      authService.setUserRole(UserRole.Doctor);
      component.userRole = UserRole.Doctor;
      expect(component.userRoleString).toBe('doctor');
    });
    it('ar trebui să returneze "patient" pentru UserRole.Patient', () => {
      authService.setUserRole(UserRole.Patient);
      component.userRole = UserRole.Patient;
      expect(component.userRoleString).toBe('patient');
    });
    it('ar trebui să returneze "none" pentru UserRole.None', () => {
      authService.setUserRole(UserRole.None);
      component.userRole = UserRole.None;
      expect(component.userRoleString).toBe('none');
    });
  });

  describe('Metode utilitare pentru atașamente', () => {
    it('getFileName ar trebui să returneze numele fișierului dintr-un path', () => {
      const path = 'attachments/folder/file123.pdf';
      expect(component.getFileName(path)).toBe('file123.pdf');
    });
    it('getFileName ar trebui să returneze "Attachment" dacă path-ul este gol', () => {
      expect(component.getFileName('')).toBe('Attachment');
    });
    it('getAttachmentUrl ar trebui să returneze URL-ul complet pentru un path de atașament', () => {
      const path = 'attachments/file123.pdf';
      expect(component.getAttachmentUrl(path)).toBe('https://localhost:7233/attachments/file123.pdf');
    });
    it('getAttachmentUrl ar trebui să returneze "#" dacă path-ul este gol', () => {
      expect(component.getAttachmentUrl('')).toBe('#');
    });
  });

  describe('logout', () => {
    it('ar trebui să apeleze logout() din AuthService', () => {
      const logoutSpy = spyOn(authService, 'logout').and.callThrough();
      component.logout();
      expect(logoutSpy).toHaveBeenCalled();
    });
  });
});
