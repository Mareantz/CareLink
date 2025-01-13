import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor/doctor.service';
import { PatientService } from '../../services/patient/patient.service';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

// Interfețele pentru un utilizator (pentru simplitate)
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

// Mock-uri pentru serviciile injectate
class MockAuthService {
  private token: string | null = 'dummy-token';
  private role = 0; // 0: None, 1: Doctor, 2: Patient, 3: Admin
  private userId = 'user123';

  getToken(): string | null {
    return this.token;
  }
  getUserRole(): number {
    return this.role;
  }
  getUserId(): string | null {
    return this.userId;
  }
  logout(): void {
    this.token = null;
  }
  // Metodă utilitară pentru a schimba rolul
  setUserRole(role: number) {
    this.role = role;
  }
  // Metodă utilitară pentru a schimba tokenul
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

class MockBreakpointObserver {
  private subject = new Subject<BreakpointState>();
  observe(queries: string[]) {
    return this.subject.asObservable();
  }
  emit(state: BreakpointState) {
    this.subject.next(state);
  }
}

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let authService: MockAuthService;
  let patientService: MockPatientService;
  let doctorService: MockDoctorService;
  let breakpointObserver: MockBreakpointObserver;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardComponent, RouterTestingModule, NoopAnimationsModule],
      providers: [
        { provide: AuthService, useClass: MockAuthService },
        { provide: PatientService, useClass: MockPatientService },
        { provide: DoctorService, useClass: MockDoctorService },
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

    });
  
  describe('logout', () => {
    it('ar trebui să apeleze logout() din AuthService', () => {
      const logoutSpy = spyOn(authService, 'logout').and.callThrough();
      component.logout();
      expect(logoutSpy).toHaveBeenCalled();
    });
  });
});
