import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfileComponent } from './profile.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor/doctor.service';
import { PatientService } from '../../services/patient/patient.service';
import { AuthService } from '../../services/auth.service';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { UserRole } from '../../UserRole';

// Creăm mock-uri pentru serviciile injectate:

class MockDoctorService {
  getDoctorById(id: string) {
    return of({
      firstName: 'John',
      lastName: 'Doe',
      specialization: 'Cardiology',
      bio: 'Experienced cardiologist'
    });
  }
  updateDoctor(id: string, doctorData: any) {
    return of(doctorData);
  }
}

class MockPatientService {
  getPatientById(id: string) {
    return of({
      firstName: 'Jane',
      lastName: 'Smith',
      specialization: '',
      bio: 'Patient bio'
    });
  }
  updatePatient(id: string, patientData: any) {
    return of(patientData);
  }
}

class MockAuthService {
  // În funcție de test, putem schimba detaliile utilizatorului
  private userDetails: any = null;
  setUserDetails(details: any) {
    this.userDetails = details;
  }
  getUserDetails() {
    return this.userDetails;
  }
}

class MockRouter {
  navigate(commands: any[]) {
    return commands;
  }
}

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let authService: MockAuthService;
  let doctorService: DoctorService;
  let patientService: PatientService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ProfileComponent, // deoarece este standalone
        ReactiveFormsModule,
        NoopAnimationsModule // pentru a evita probleme cu animațiile
      ],
      providers: [
        FormBuilder,
        { provide: DoctorService, useClass: MockDoctorService },
        { provide: PatientService, useClass: MockPatientService },
        { provide: AuthService, useClass: MockAuthService },
        { provide: Router, useClass: MockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as unknown as MockAuthService;
    doctorService = TestBed.inject(DoctorService);
    patientService = TestBed.inject(PatientService);
    router = TestBed.inject(Router);
  });

  describe('ngOnInit', () => {
    it('ar trebui să obțină detalii de doctor și să populeze formularul dacă rolul este Doctor', () => {
      // Setăm detaliile utilizatorului cu rol de Doctor
      authService.setUserDetails({ id: '123', role: UserRole.Doctor });
      spyOn(doctorService, 'getDoctorById').and.callThrough();
      
      component.ngOnInit();
      // Verificăm ca userId și userRole să fie setate
      expect(component.userId).toBe('123');
      expect(component.userRole).toBe(UserRole.Doctor);
      expect(doctorService.getDoctorById).toHaveBeenCalledWith('123');

      // După ce se obține detaliul doctorului, formularul este populat
      fixture.detectChanges();
      expect(component.profileForm.value.firstName).toBe('John');
      expect(component.profileForm.value.lastName).toBe('Doe');
      expect(component.profileForm.value.specialization).toBe('Cardiology');
      expect(component.profileForm.value.bio).toBe('Experienced cardiologist');
    });

    it('ar trebui să obțină detalii de pacient și să populeze formularul dacă rolul este Patient', () => {
      // Setăm detaliile utilizatorului cu rol de Patient
      authService.setUserDetails({ id: '456', role: UserRole.Patient });
      spyOn(patientService, 'getPatientById').and.callThrough();

      component.ngOnInit();
      expect(component.userId).toBe('456');
      expect(component.userRole).toBe(UserRole.Patient);
      expect(patientService.getPatientById).toHaveBeenCalledWith('456');

      fixture.detectChanges();
      expect(component.profileForm.value.firstName).toBe('Jane');
      expect(component.profileForm.value.lastName).toBe('Smith');
      // Câmpurile lipsă vor fi string gol
      expect(component.profileForm.value.specialization).toBe('');
      expect(component.profileForm.value.bio).toBe('Patient bio');
    });

    it('ar trebui să nu facă nimic dacă userDetails este null', () => {
      authService.setUserDetails(null);
      component.ngOnInit();
      expect(component.userId).toBeNull();
      expect(component.userRole).toBe(UserRole.None);
    });
  });

  describe('populateForm', () => {
    it('ar trebui să populeze formularul cu datele primite', () => {
      const data = {
        firstName: 'Alice',
        lastName: 'Wonderland',
        specialization: 'Neurology',
        bio: 'Expert in neurology'
      };
      component.profileForm.reset();
      component['populateForm'](data);
      expect(component.profileForm.value.firstName).toBe('Alice');
      expect(component.profileForm.value.lastName).toBe('Wonderland');
      expect(component.profileForm.value.specialization).toBe('Neurology');
      expect(component.profileForm.value.bio).toBe('Expert in neurology');
    });
  });

  describe('onSubmit', () => {
    beforeEach(() => {
      // Setăm userDetails pentru a putea actualiza
      authService.setUserDetails({ id: '789', role: UserRole.Doctor });
      // Putem apela direct ngOnInit pentru a seta userId și userRole
      component.ngOnInit();
    });

    it('ar trebui să actualizeze doctorul și să navigheze la dashboard atunci când formularul este valid și rolul este Doctor', () => {
      // Populăm formularul cu valori valide
      component.profileForm.setValue({
        firstName: 'Bob',
        lastName: 'Marley',
        specialization: 'General Medicine',
        bio: 'Some bio'
      });

      spyOn(doctorService, 'updateDoctor').and.callThrough();
      const routerSpy = spyOn(router, 'navigate').and.callThrough();

      component.onSubmit();

      // Se apelează updateDoctor cu id-ul și datele formularului
      expect(doctorService.updateDoctor).toHaveBeenCalledWith('789', {
        id: '789',
        firstName: 'Bob',
        lastName: 'Marley',
        specialization: 'General Medicine',
        bio: 'Some bio'
      });
      expect(routerSpy).toHaveBeenCalledWith(['/dashboard']);
    });

    it('ar trebui să actualizeze pacientul și să navigheze la /patients atunci când formularul este valid și rolul este Patient', () => {
      // Modificăm detaliile utilizatorului pentru a fi pacient
      authService.setUserDetails({ id: '101', role: UserRole.Patient });
      component.ngOnInit(); // reinitializează userId și userRole

      component.profileForm.setValue({
        firstName: 'Clara',
        lastName: 'Oswald',
        specialization: '',
        bio: 'Patient bio'
      });

      spyOn(patientService, 'updatePatient').and.callThrough();
      const routerSpy = spyOn(router, 'navigate').and.callThrough();

      component.onSubmit();

      expect(patientService.updatePatient).toHaveBeenCalledWith('101', {
        id: '101',
        firstName: 'Clara',
        lastName: 'Oswald',
        specialization: '',
        bio: 'Patient bio'
      });
      expect(routerSpy).toHaveBeenCalledWith(['/patients']);
    });

    it('ar trebui să afișeze eroare în consolă dacă formularul nu este valid', () => {
      spyOn(console, 'error');
      // Nu completăm formularul astfel încât să fie invalid
      component.profileForm.setValue({
        firstName: '', // required field
        lastName: '',
        specialization: '',
        bio: ''
      });

      component.onSubmit();

      expect(console.error).toHaveBeenCalledWith('Form is invalid');
    });

    it('ar trebui să afișeze eroare în consolă dacă userId este null', () => {
      spyOn(console, 'error');
      // Setăm formularul valid, dar resetăm userId-ul
      component.userId = null;
      component.profileForm.setValue({
        firstName: 'Test',
        lastName: 'User',
        specialization: '',
        bio: ''
      });
      component.onSubmit();
      expect(console.error).toHaveBeenCalledWith('User ID is null');
    });
  });

  describe('toggleEditMode', () => {
    it('ar trebui să inverseze valoarea lui isEditMode', () => {
      component.isEditMode = false;
      component.toggleEditMode();
      expect(component.isEditMode).toBeTrue();

      component.toggleEditMode();
      expect(component.isEditMode).toBeFalse();
    });
  });

  describe('getFormFields', () => {
    it('ar trebui să returneze lista de chei ale controalelor formularului', () => {
      const formFields = component.getFormFields();
      expect(formFields).toContain('firstName');
      expect(formFields).toContain('lastName');
      expect(formFields).toContain('specialization');
      expect(formFields).toContain('bio');
    });
  });
});
